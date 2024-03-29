using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShopCatalog.Exceptions;

namespace ShopCatalog.CsvDao.CsvTempCatalog
{
    internal class TempShopCatalog
    {

        internal string[] GetCsvShopsData()
        {
            string[] data = new string[_shops.Count];
            int counter = 0;
            foreach (var shop in _shops)
            {
                data[counter] = $"{shop.Key},{shop.Value.Name},{shop.Value.Address}";
                ++counter;
            }

            return data;
        }
        
        internal string[] GetCsvShopProductsData()
        {
            string[] result = new string[_products.Count];
            int counter = 0;
            foreach (string product in _products)
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(product + ',');
                foreach (KeyValuePair<int,ShopProducts> keyValuePair in _shopProducts)
                {
                    if (keyValuePair.Value.HasProduct(product))
                    {
                        stringBuilder.Append(keyValuePair.Key + ",");
                        stringBuilder.Append(keyValuePair.Value.GetQuantity(product) + ",");
                        stringBuilder.Append(keyValuePair.Value.GetPrice(product) + ",");
                    }
                }

                stringBuilder.Remove(stringBuilder.Length - 1, 1);
                result[counter] = stringBuilder.ToString();
                ++counter;
            }
            return result;
        }

        /** Get list of shops with full info
         * @return tuple(int, string, string) ShopID, ShopName, ShopAddress
         */
        internal (int, string, string)[] GetShops()
        {
            (int, string, string)[] data = new (int, string, string)[_shops.Count];
            int counter = 0;
            foreach (var shop in _shops)
            {
                data[counter].Item1 = shop.Key;
                data[counter].Item2 = shop.Value.Name;
                data[counter].Item3 = shop.Value.Address;
                ++counter;
            }

            return data;
        }

        /** Get list of products
         * @return array of products' names
         */
        internal string[] GetProducts()
        {
            return _products.ToArray();
        }

        /**
         * Get shop name related to shop id
         * @param shopId Id of the shop
         * @return Shop name or empty string if shop does not exists
         */
        internal string GetShopName(int shopId)
        {
            if (_shops.ContainsKey(shopId))
                return _shops[shopId].Name;
            
            throw new ShopNotExistsException();
        }
        
        /**
         * Get shop address related to shop id
         * @param shopId Id of the shop
         * @return Shop address or empty string if shop does not exists
         */
        internal string GetShopAddress(int shopId)
        {
            if (_shops.ContainsKey(shopId))
                return _shops[shopId].Address;
            
            throw new ShopNotExistsException();
        }
        
        /** Get product quantity in the shop[shopId] by productName
         * @param shopId shop to search in
         * @param productName name of product to search
         * @return quantity of products or -1 if product or shop not exists
         */
        internal int GetProductQuantity(int shopId, string productName)
        {
            if (!IsShopExists(shopId))
            {
                throw new ShopNotExistsException();
            }

            return _shopProducts[shopId].GetQuantity(productName);
        }

        /** Get product price in the shop[shopId] by productName
         * @param shopId shop to search in
         * @param productName name of product to search
         * @return price of product or -1 if product or shop not exists
         */
        public double GetProductPrice(int shopId, string productName)
        {
            if (!IsShopExists(shopId))
            {
                throw new ShopNotExistsException();
            }

            return _shopProducts[shopId].GetPrice(productName);
        }

        /** Get shop ID with minimal price for product, if quantity of product in the shop > 0
         * @return ShopId if product found, -1 if product not found or quantity == 0
         */
        internal int GetMinPriceShopId(string productName)
        {
            double price = Double.MinValue;
            int shopId = -1;
            foreach (var shopProduct in _shopProducts)
            {
                if (!shopProduct.Value.HasProduct(productName)) 
                    continue;

                double tempPrice = shopProduct.Value.GetPrice(productName);
                if (tempPrice < price || shopId == -1)
                {
                    price = tempPrice;
                    shopId = shopProduct.Key;
                }
            }

            return shopId;
        }

        /** Get info how many products you can buy for fixed price in the shop
         * @param shopId Shop ID to search products
         * @totalMaxPrice Maximum price that can be
         * @return Tuple (product name, quantity)
         */
        internal (string, int)[] GetProductsForPrice(int shopId, double totalMaxPrice)
        {
            if (!IsShopExists(shopId))
            {
                throw new ShopNotExistsException();
            }

            return _shopProducts[shopId].GetProductsForPrice(totalMaxPrice);

        }

        /** Calculate total sum of purchase
         * @param shopId Shop where purchase will be made
         * @param @param productsData array of (productName, quantity) tuples
         * @return Total sum of purchase or -1 if some product not exists/quantity is not enough
         */
        internal double GetPurchaseTotal(int shopId, (string, int)[] productsData)
        {
            if (!IsShopExists(shopId))
            {
                throw new ShopNotExistsException();
            }

            return _shopProducts[shopId].GetPurchaseTotal(productsData);
        }
        
        /** Get shop id with minimum total price for purchase
         * @param productsData array of (productName, quantity) tuples
         * @return shopId or -1 if there is no shop to buy all products of list
         */
        internal int GetMinPurchaseTotalShopId((string, int)[] productsData)
        {
            int shopId = -1;
            double sum = Double.MaxValue;
            
            foreach (var shopProduct in _shopProducts)
            {
                double tempSum = shopProduct.Value.GetPurchaseTotal(productsData);
                if (tempSum > 0 && tempSum < sum)
                {
                    sum = tempSum;
                    shopId = shopProduct.Key;
                }
            }

            return shopId;
        }
        
        


        /** Checks if shop exists by shop id
         * @param shopId Id of the shop
         * @return true if exists, false if not
         */
        internal bool IsShopExists(int shopId)
        {
            return _shops.ContainsKey(shopId);
        }

        /** Checks if shop exists by shop name
          * @param shopName Name of the shop
          * @return true if exists, false if not
          */
        internal bool IsShopExists(string shopName)
        {
            bool flag = false;
            foreach (var shop in _shops)
            {
                if (shop.Value.Name.Equals(shopName))
                {
                    flag = true;
                }
            }

            return flag;
        }

        /** Checks if product exists 
          * @param productName Name of the product
          * @return true if exists, false if not
          */
        internal bool IsProductExists(string productName)
        {
            return _products.Contains(productName);
        }

        /** Checks if product exists in ths shop
          * @param shopId Id of the shop to check in 
          * @param productName Name of the product
          * @return true if exists, false if not
          */
        internal bool IsShopContainsProduct(int shopId, string productName)
        {
            if (!IsShopExists(shopId))
            {
                throw new ShopNotExistsException();
            }

            if (!IsProductExists(productName))
            {
                throw new ProductNotExistsException();
            }

            return _shopProducts[shopId].HasProduct(productName);
        }
        
        
        /** Create and add shop into Shop table
         * @param shopId Unique id of the shop
         * @param shopName Name of the shop
         * @param shopAddress Address of the shop
         */
        internal void CreateShop(int shopId, string shopName, string shopAddress)
        {
            if (IsShopExists(shopId))
            {
                throw new MissingDataConsistencyException();
            }
            _shops.Add(shopId, new Shop(shopId, shopName, shopAddress));
            _shopProducts.Add(shopId, new ShopProducts());
        }

        /** Create and add product into Product table
         * @param productName Unique name of product
         */
        internal void CreateProduct(string productName)
        {
            if (IsProductExists(productName))
            {
                throw new MissingDataConsistencyException();
            }
            _products.Add(productName);
        }

        /** Add the product to the shop with fixed price and quantity
         * @param shopID Shop id
         * @param productName Product name
         * @param price Product price
         * @param quantity Product quantity
         */
        internal void AddProductToShop(int shopId, string productName, double price, int quantity)
        {
            if (!IsShopExists(shopId))
            {
                throw new ShopNotExistsException();
            }
            
            if (!IsProductExists(productName))
            {
                throw new ProductNotExistsException();
            }
            
            _shopProducts[shopId].AddProduct(productName, quantity, price);
        }

        /** Update price for the product in the shop
         * @param shopID Shop id
         * @param productName Product name
         * @param price New price
         */
        internal void UpdatePrice(int shopId, string productName, double price)
        {
            if (!IsShopExists(shopId))
            {
                throw new ShopNotExistsException();
            }

            _shopProducts[shopId].UpdateProductPrice(productName, price);
        }

        /** Update quantity for the product in the shop
         * @param shopID Shop id
         * @param productName Product name
         * @param quantity New quantity
         */
        internal void UpdateQuantity(int shopId, string productName, int quantity)
        {
            if (!IsShopExists(shopId))
            {
                throw new ShopNotExistsException();
            }

            _shopProducts[shopId].UpdateProductQuantity(productName, quantity);
        }


        /** Buy products in the shop, decreases quantities
         * @param shopId Shop where products needed to buy
         * @param @param productsData array of (productName, quantity) tuples
         * @return true if operation successful, else false
         */
        internal bool BuyProducts(int shopId, (string, int)[] productsData)
        {
            if (!IsShopExists(shopId))
            {
                throw new ShopNotExistsException();
            }

            return _shopProducts[shopId].BuyProducts(productsData);
        }

        // Products' names
        private HashSet<string> _products = new HashSet<string>();
        // Shop id, shop 
        private Dictionary<int, Shop> _shops = new Dictionary<int, Shop>();
        // Shop id, shop products data
        private Dictionary<int, ShopProducts> _shopProducts = new Dictionary<int, ShopProducts>();
    }
}