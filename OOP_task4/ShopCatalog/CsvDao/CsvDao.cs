using System;
using System.IO;
using System.Text.RegularExpressions;
using ShopCatalog.CsvDao.CsvTempCatalog;
using ShopCatalog.CsvDao.Exceptions;
using ShopCatalog.Exceptions;

namespace ShopCatalog.CsvDao
{
    class CsvDao : IDao
    {
        /**Create CsvDao
         * @param connectionString filepath in format "shopData=<filepath>;productData=<filepath>"
         */
        internal CsvDao(string connectionString)
        {
            try
            {
                _shopsDataFilepath = ShopsDataFilepathRegex.Match(connectionString).Groups[2].Value;
                _productsDataFilepath = ProductsDataFilepathRegex.Match(connectionString).Groups[2].Value;
                File.OpenText(_shopsDataFilepath).Close();
                File.OpenText(_productsDataFilepath).Close();

            }
            catch (Exception)
            {
                throw new CsvFileConnectException("Incorrect filepath(s)");
            }
        }
        
        /** Create and add shop 
         * @param shopId Unique id of the shop
         * @param shopName Name of the shop
         * @param shopAddress Address of the shop
         */
        public void CreateShop(int shopId, string shopName, string shopAddress)
        {
            TempShopCatalog catalog = ParseCsvFiles(_shopsDataFilepath, _productsDataFilepath);
            catalog.CreateShop(shopId, shopName, shopAddress);
            WriteDataToCsvFiles(catalog, _shopsDataFilepath, _productsDataFilepath);
        }

        /** Create and add product 
         * @param productName Unique name of product
         */
        public void CreateProduct(string productName)
        {
            TempShopCatalog catalog = ParseCsvFiles(_shopsDataFilepath, _productsDataFilepath);
            catalog.CreateProduct(productName);
            WriteDataToCsvFiles(catalog, _shopsDataFilepath, _productsDataFilepath);
        }
        
        /** Get list of shops with full info
         * @return tuple(int, string, string) ShopID, ShopName, ShopAddress
         */
        public (int, string, string)[] GetShops()
        {
            TempShopCatalog catalog = ParseCsvFiles(_shopsDataFilepath, _productsDataFilepath);
            return catalog.GetShops();
        }

        /** Get list of products
         * @return array of products' names
         */
        public string[] GetProducts()
        {
            TempShopCatalog catalog = ParseCsvFiles(_shopsDataFilepath, _productsDataFilepath);
            return catalog.GetProducts();
        }

        /** Add the product to the shop with fixed price and quantity
         * @param shopID Shop id
         * @param productName Product name
         * @param price Product price
         * @param quantity Product quantity
         */
        public void AddProductToShop(int shopId, string productName, double price, int quantity)
        {
            TempShopCatalog catalog = ParseCsvFiles(_shopsDataFilepath, _productsDataFilepath);
            catalog.AddProductToShop(shopId, productName, price, quantity);
            WriteDataToCsvFiles(catalog, _shopsDataFilepath, _productsDataFilepath);
        }

        /** Update price for the product in the shop
         * @param shopID Shop id
         * @param productName Product name
         * @param price New price
         */
        public void UpdatePrice(int shopId, string productName, double price)
        {
            TempShopCatalog catalog = ParseCsvFiles(_shopsDataFilepath, _productsDataFilepath);
            catalog.UpdatePrice(shopId, productName, price);
            WriteDataToCsvFiles(catalog, _shopsDataFilepath, _productsDataFilepath);
        }
        
        /** Update quantity for the product in the shop
         * @param shopID Shop id
         * @param productName Product name
         * @param quantity New quantity
         */
        public void UpdateQuantity(int shopId, string productName, int quantity)
        {
            TempShopCatalog catalog = ParseCsvFiles(_shopsDataFilepath, _productsDataFilepath);
            catalog.UpdateQuantity(shopId, productName, quantity);
            WriteDataToCsvFiles(catalog, _shopsDataFilepath, _productsDataFilepath);
        }

        /** Get shop ID with minimal price for product, if quantity of product in the shop > 0
         * @return ShopId if product found, -1 if product not found or quantity == 0
         */
        public int GetMinPriceShopId(string productName)
        {
            TempShopCatalog catalog = ParseCsvFiles(_shopsDataFilepath, _productsDataFilepath);
            return catalog.GetMinPriceShopId(productName);
        }

        /**
         * Get shop name related to shop id
         * @param shopId Id of the shop
         * @return Shop name or empty string if shop does not exists
         */
        public string GetShopName(int shopId)
        {
            TempShopCatalog catalog = ParseCsvFiles(_shopsDataFilepath, _productsDataFilepath);
            return catalog.GetShopName(shopId);
        }

        /**
         * Get shop address related to shop id
         * @param shopId Id of the shop
         * @return Shop address or empty string if shop does not exists
         */
        public string GetShopAddress(int shopId)
        {
            TempShopCatalog catalog = ParseCsvFiles(_shopsDataFilepath, _productsDataFilepath);
            return catalog.GetShopAddress(shopId);
        }

        /** Get product quantity in the shop[shopId] by productName
         * @param shopId shop to search in
         * @param productName name of product to search
         * @return quantity of products or -1 if product or shop not exists
         */
        public int GetProductQuantity(int shopId, string productName)
        {
            TempShopCatalog catalog = ParseCsvFiles(_shopsDataFilepath, _productsDataFilepath);
            return catalog.GetProductQuantity(shopId, productName);
        }
        
        /** Get product price in the shop[shopId] by productName
         * @param shopId shop to search in
         * @param productName name of product to search
         * @return price of product or -1 if product or shop not exists
         */
        public double GetProductPrice(int shopId, string productName)
        {
            TempShopCatalog catalog = ParseCsvFiles(_shopsDataFilepath, _productsDataFilepath);
            return catalog.GetProductPrice(shopId, productName);
        }

        public bool BuyProducts(int shopId, (string, int)[] productsData)
        {
            TempShopCatalog catalog = ParseCsvFiles(_shopsDataFilepath, _productsDataFilepath);
            bool result = catalog.BuyProducts(shopId, productsData);
            WriteDataToCsvFiles(catalog, _shopsDataFilepath, _productsDataFilepath);
            return result;
        }
        
        /** Get info how many products you can buy for fixed price in the shop
         * @param shopId Shop ID to search products
         * @totalMaxPrice Maximum price that can be
         * @return Tuple (product name, quantity)
         */
        public (string, int)[] GetProductsForPrice(int shopId, double totalMaxPrice)
        {
            TempShopCatalog catalog = ParseCsvFiles(_shopsDataFilepath, _productsDataFilepath);
            return catalog.GetProductsForPrice(shopId, totalMaxPrice);
        }

        /** Calculate total sum of purchase
         * @param shopId Shop where purchase will be made
         * @param @param productsData array of (productName, quantity) tuples
         * @return Total sum of purchase or -1 if some product not exists/quantity is not enough
         */
        public double GetPurchaseTotal(int shopId, (string, int)[] productsData)
        {
            TempShopCatalog catalog = ParseCsvFiles(_shopsDataFilepath, _productsDataFilepath);
            return catalog.GetPurchaseTotal(shopId, productsData);
        }

        /** Get shop id with minimum total price for purchase
         * @param productsData array of (productName, quantity) tuples
         * @return shopId or -1 if there is no shop to buy all products of list
         */
        public int GetMinPurchaseTotalShopId((string, int)[] productsData)
        {
            TempShopCatalog catalog = ParseCsvFiles(_shopsDataFilepath, _productsDataFilepath);
            return catalog.GetMinPurchaseTotalShopId(productsData);
        }

        /** Checks if shop exists by shop id
         * @param shopId Id of the shop
         * @return true if exists, false if not
         */
        public bool IsShopExists(int shopId)
        {
            TempShopCatalog catalog = ParseCsvFiles(_shopsDataFilepath, _productsDataFilepath);
            return catalog.IsShopExists(shopId);
        }

        /** Checks if shop exists by shop name
          * @param shopName Name of the shop
          * @return true if exists, false if not
          */
        public bool IsShopExists(string shopName)
        {
            TempShopCatalog catalog = ParseCsvFiles(_shopsDataFilepath, _productsDataFilepath);
            return catalog.IsShopExists(shopName);
        }
        
        /** Checks if product exists 
         * @param productName Name of the product
         * @return true if exists, false if not
         */
        public bool IsProductExists(string productName)
        {
            TempShopCatalog catalog = ParseCsvFiles(_shopsDataFilepath, _productsDataFilepath);
            return catalog.IsProductExists(productName);
        }

        /** Checks if product exists in ths shop
          * @param shopId Id of the shop to check in 
          * @param productName Name of the product
          * @return true if exists, false if not
          */
        public bool IsShopContainsProduct(int shopId, string productName)
        {
            TempShopCatalog catalog = ParseCsvFiles(_shopsDataFilepath, _productsDataFilepath);
            return catalog.IsShopContainsProduct(shopId, productName);
        }


        private readonly string _shopsDataFilepath;
        private readonly string _productsDataFilepath;
        
        private static readonly Regex ShopsDataFilepathRegex = new Regex(@"\s*(;|^)\s*shopData\s*=\s*(.+?)(;|$)");
        private static readonly Regex ProductsDataFilepathRegex = new Regex(@"\s*(;|^)\s*productData\s*=\s*(.+?)(;|$)");

        private static TempShopCatalog ParseCsvFiles(string shopDataFilepath, string productsDataFilepath)
        {
            TempShopCatalog catalog = new TempShopCatalog();
            ParseShopData(ref catalog, shopDataFilepath);
            ParseProductData(ref catalog, productsDataFilepath);
            return catalog;
        }

        private static void ParseShopData(ref TempShopCatalog catalog, string shopDataFilepath)
        {
            using (var reader = File.OpenText(shopDataFilepath))
            {
                try
                {
                    while (!reader.EndOfStream)
                    {
                        string[] shopsData = reader.ReadLine().Split(',');
                        catalog.CreateShop(int.Parse(shopsData[0]), shopsData[1], shopsData[2]);
                    }
                }
                catch (Exception e)
                {
                    throw new DataReadingErrorException(e.ToString());
                }
            }
        }
        
        private static void ParseProductData(ref TempShopCatalog catalog, string productDataFilepath)
        {
            using (var reader = File.OpenText(productDataFilepath))
            {
                try
                {
                    while (!reader.EndOfStream)
                    {
                        string[] productsData = reader.ReadLine().Split(',');
                        string productName = productsData[0];
                        catalog.CreateProduct(productName);
                        for (int i = 1; i < productsData.Length - 2; i += 3)
                        {
                            int shopId = int.Parse(productsData[i]);
                            int quantity = int.Parse(productsData[i + 1]);
                            double price = double.Parse(productsData[i + 2]);
                            catalog.AddProductToShop(shopId, productName, price, quantity);
                        }
                    }
                }
                catch (Exception e)
                {
                    throw new DataReadingErrorException(e.ToString());
                }
            }
        }

        private static void WriteDataToCsvFiles(TempShopCatalog catalog, string shopDataFilepath,
            string productDataFilepath)
        {
            using (StreamWriter sw = new StreamWriter(shopDataFilepath, false))
            {
                foreach (string s in catalog.GetCsvShopsData())
                {
                    sw.WriteLine(s);
                }
            }
            
            using (StreamWriter sw = new StreamWriter(productDataFilepath, false))
            {
                foreach (string s in catalog.GetCsvShopProductsData())
                {
                    sw.WriteLine(s);
                }
            }
        }
    }
    
}