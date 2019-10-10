using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using ShopCatalog.Exceptions;

namespace ShopCatalog
{
    public class Service
    {
        public string[] ExecuteCommand(string command)
        {
//            return ExecCreateShop("create-shop shop-id='5';shop-name='ПУД';shop-address='Симферополь'");
//            return ExecCreateProduct("create-product product-name='Сало украинское'");
//            return PrintProducts();
//            return ExecAddProductToShop(
//                "add-to-shop shop-id='5';product-name='Сало украинское';price='13.6';quantity='100'");
//            return PrintShopWithMinPrice("where-is-min-price product-name='Сало украинское'");
//            return PrintProductsForTotal("get-products-for-total shop-id='5';total='100000.5'");
            return PrintMinPriceShopId("where-is-cheeper products=[Шоколад ‘Аленка’,2|Телевизор PHILIPS,1]");
        }

        internal Service(IDao dao)
        {
            _dao = dao;
        }

        internal void ReconnectDao(IDao dao)
        {
            _dao = dao;
        }
    
        private IDao _dao;

        
        private string[] PrintShops()
        {
            var shopsData = _dao.GetShops();
            string[] result = new string[shopsData.Length];
            for (var i = 0; i < shopsData.Length; i++)
            {
                result[i] = shopsData[i].ToString();
            }

            return result;
        }

        private string[] PrintProducts()
        {
            return _dao.GetProducts();
        }
        
        private string[] PrintProductsInShop(string args)
        {
            Regex regex = new Regex(@".*print-shop-products\s+?shop-id='(\d+?)'.*");
            if (!regex.IsMatch(args))
            {
                return new []{ "Incorrect shopId" };
            }
            int shopId = int.Parse(regex.Match(args).Groups[1].Value);

            string[] products = _dao.GetProducts();
            List<string> result = new List<string>();
            result.Add($"Products in the shop #{shopId} - {_dao.GetShopName(shopId)}:");
            foreach (string productName in products)
            {
                if (_dao.IsShopContainsProduct(shopId, productName))
                {
                    result.Add($"({productName}, {_dao.GetProductQuantity(shopId, productName)}," +
                               $" {_dao.GetProductPrice(shopId, productName)})");
                }
            }

            return result.ToArray();
        }
        
        private string[] ExecCreateShop(string args)
        {
            Regex regex = new Regex(@".*create-shop\s+?shop-id='(\d+?)';\s*shop-name='(.+?)';\s*shop-address='(.+?)'\s*");
            if (!regex.IsMatch(args))
            {
                return new []{"Err. Incorrect shop data"};
            }

            var matcher = regex.Match(args);
            int shopId = int.Parse(matcher.Groups[1].Value);
            string shopName = matcher.Groups[2].Value;
            string shopAddress = matcher.Groups[3].Value;
            
            try
            {
                _dao.CreateShop(shopId, shopName, shopAddress);
            }
            catch (MissingDataConsistencyException)
            {
                return new[] {$"Err. Creation error: shop on ID={shopId} already exists"};
            }

            return new[] {"Creation successful, shop:", $"({shopId}, {shopName}, {shopAddress})"};
        }

        private string[] ExecCreateProduct(string args)
        {
            Regex regex = new Regex(@".*create-product\s*product-name='(.+?)'\s*");
            if (!regex.IsMatch(args))
            {
                return new []{"Err. Incorrect product data"};
            }

            var matcher = regex.Match(args);
            string productName = matcher.Groups[1].Value;

            try
            {
                _dao.CreateProduct(productName);
            }
            catch (MissingDataConsistencyException)
            {
                return new[] {$"Err. Creation error: ProductName='{productName}' already exists"};
            }
            return new[] {"Creation successful, product:", $"'{productName}'"};
        }

        private string[] ExecAddProductToShop(string args)
        {
            Regex regex = new Regex(
                @".*add-to-shop\s+?shop-id='(\d+?)';\s*product-name='(.+?)'\s*;\s*price='((\d+?)(\.(\d+?))?)';\s*quantity='(\d+?)'\s*");
            if (!regex.IsMatch(args))
            {
                return new []{"Err. Incorrect data format"};
            }
                
            var matcher = regex.Match(args);
            int shopId = int.Parse(matcher.Groups[1].Value);
            string productName = matcher.Groups[2].Value;
            if (!double.TryParse(matcher.Groups[3].Value, out var productPrice))
            {
                return new []{"Err. Incorrect data format"};
            }
            int productQuantity = int.Parse(matcher.Groups[7].Value);

            try
            {
                _dao.AddProductToShop(shopId, productName, productPrice, productQuantity);
            }
            catch (ShopNotExistsException)
            {
                return new[] {$"Err. Product adding error: shop on ShopId={shopId} not exists"};
            }
            catch (ProductNotExistsException)
            {
                return new[] {$"Err. Product adding error: product with ProductName={productName} not exists"};
            }
            catch (MissingDataConsistencyException)
            {
                return new[] {$"Err. Product already exists in shop. To update data use upd-product-* commands"};
            }

            return new[] 
            {
                $"Creation successful, product added to shop #{shopId} - {_dao.GetShopName(shopId)}",
                $"({productName}, {productPrice}, {productQuantity})"
            };
        }

        private string[] PrintShopWithMinPrice(string args)
        {
            Regex regex = new Regex(@".*where-is-min-price\s*product-name='(.+?)'\s*");
            if (!regex.IsMatch(args))
            {
                return new []{"Err. Incorrect data"};
            }

            var matcher = regex.Match(args);
            string productName = matcher.Groups[1].Value;
            
            int shopId;
            double price;
            try
            {
                shopId = _dao.GetMinPriceShopId(productName);
                price = _dao.GetProductPrice(shopId, productName);
            }
            catch (ProductNotExistsException)
            {
                return new[] {$"Err. Product with ProductName='{productName}' not exists"};
            }
            catch (ShopNotExistsException)
            {
                return new[] {$"Err. Product ProductName='{productName}' exists, but not found in any shop"};
            }
            
            return new[]
            {
                $"Minimum price for '{productName}' is in the shop #{shopId} {_dao.GetShopName(shopId)}. Price: {price}"
            };
        }

        private string[] PrintProductsForTotal(string args)
        {
            Regex regex = new Regex(@".*get-products-for-total\s+?shop-id='(\d+?)';\s*total='((\d+?)(\.(\d+?))?)'\s*");

            if (!regex.IsMatch(args))
            {
                return new []{"Err. Incorrect data"};
            }

            var matcher = regex.Match(args);
            int shopId = int.Parse(matcher.Groups[1].Value);
            if (!double.TryParse(matcher.Groups[2].Value, out var maxTotal))
            {
                return new []{"Err. Incorrect data format"};
            }

            (string, int)[] productsData;
            try
            {
                productsData = _dao.GetProductsForPrice(shopId, maxTotal);
            }
            catch (ShopNotExistsException)
            {
                return new[] {$"Err. ShopId={shopId} not exists"};
            }
            
            string[] result = new string[productsData.Length];
            for (var index = 0; index < productsData.Length; index++)
            {
                result[index] = $"Product: {productsData[index].Item1}, Max quantity: {productsData[index].Item2}";
            }

            return result;
        }

        private string[] PrintPurchaseTotal(string args)
        {
            Regex regex = new Regex(
                @".*get-total\s+?shop-id='(\d+?)'\s*;\s*products=\[(.+?)\]\s*");

            if (!regex.IsMatch(args))
            {
                return new []{"Err. Incorrect data format"};
            }

            var matcher = regex.Match(args);
            int shopId = int.Parse(matcher.Groups[1].Value);

            string[] productsData = matcher.Groups[2].Value.Split('|');
            (string, int)[] productsNamesQuantities = new (string, int)[productsData.Length];
            for (var i = 0; i < productsData.Length; i++)
            {
                string[] pair = productsData[i].Split(',');
                string productName = pair[0];
                if (!int.TryParse(pair[1], out var quantity))
                {
                    return new[] {"Err. Incorrect data format"};
                }
                productsNamesQuantities[i] = (productName, quantity);
            }

            double total;
            try
            {
                total = _dao.GetPurchaseTotal(shopId, productsNamesQuantities);
            }
            catch (ProductNotExistsException)
            {
                return new[] {$"Err. One of product names not exists"};
            }
            catch (ShopNotExistsException)
            {
                return new[] {$"Err. ShopId={shopId} not exists"};
            }

            if (total < 0)
            {
                return new[] {$"Not enough products in the shop"};
            }
            return new []{$"Total: {total}"};
        }

        private string[] ExecBuyProducts(string args)
        {
            Regex regex = new Regex(
                @".*buy-products\s+?shop-id='(\d+?)'\s*;\s*products=\[(.+?)\]\s*");

            if (!regex.IsMatch(args))
            {
                return new []{"Err. Incorrect data format"};
            }

            var matcher = regex.Match(args);
            int shopId = int.Parse(matcher.Groups[1].Value);

            string[] productsData = matcher.Groups[2].Value.Split('|');
            (string, int)[] productsNamesQuantities = new (string, int)[productsData.Length];
            for (var i = 0; i < productsData.Length; i++)
            {
                string[] pair = productsData[i].Split(',');
                string productName = pair[0];
                if (!int.TryParse(pair[1], out var quantity))
                {
                    return new[] {"Err. Incorrect data format"};
                }
                productsNamesQuantities[i] = (productName, quantity);
            }

            double total;
            bool result;
            try
            {
                total = _dao.GetPurchaseTotal(shopId, productsNamesQuantities);
                result = _dao.BuyProducts(shopId, productsNamesQuantities);
            }
            catch (ProductNotExistsException)
            {
                return new[] {$"Err. One of product names not exists"};
            }
            catch (ShopNotExistsException)
            {
                return new[] {$"Err. ShopId={shopId} not exists"};
            }

            if (!result)
            {
                return new[] {$"Buying error"};
            }
            return new []{$"Buying successful. Total: {total}"};
        }

        private string[] PrintMinPriceShopId(string args)
        {
            Regex regex = new Regex(
                @".*where-is-cheeper\s+?products=\[(.+?)\]\s*");

            if (!regex.IsMatch(args))
            {
                return new []{"Err. Incorrect data format"};
            }

            var matcher = regex.Match(args);

            string[] productsData = matcher.Groups[1].Value.Split('|');
            (string, int)[] productsNamesQuantities = new (string, int)[productsData.Length];
            for (var i = 0; i < productsData.Length; i++)
            {
                string[] pair = productsData[i].Split(',');
                string productName = pair[0];
                if (!int.TryParse(pair[1], out var quantity))
                {
                    return new[] {"Err. Incorrect data format"};
                }
                productsNamesQuantities[i] = (productName, quantity);
            }

            int shopId;
            try
            {
                shopId = _dao.GetMinPurchaseTotalShopId(productsNamesQuantities);
            }
            catch (ProductNotExistsException)
            {
                return new[] {$"Err. One of product names not exists"};
            }

            if (shopId == -1)
            {
                return new[] {$"No data found for your request"};
            }
            return new []{$"Minimum price of purchase can be in the shop #{shopId} {_dao.GetShopName(shopId)}"};
        }
    }
}