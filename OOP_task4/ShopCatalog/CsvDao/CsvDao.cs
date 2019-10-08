using System;
using System.Dynamic;
using System.Text.RegularExpressions;
using ShopCatalog.CsvDao.CsvTempCatalog;
using ShopCatalog.CsvDao.Exceptions;

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
                System.IO.File.OpenText(_shopsDataFilepath).Close();
                System.IO.File.OpenText(_productsDataFilepath).Close();

            }
            catch (Exception)
            {
                throw new CsvFileConnectException("Incorrect filepath");
            }
        }
        
        public void CreateShop(int shopId, string shopName, string shopAddress)
        {
            throw new System.NotImplementedException();
        }

        public void CreateProduct(string productName)
        {
            throw new System.NotImplementedException();
        }

        public (int, string, string)[] GetShops()
        {
            throw new System.NotImplementedException();
        }

        public string[] GetProducts()
        {
            throw new System.NotImplementedException();
        }

        public void AddProductToShop(int shopId, string productName, double price, int quantity)
        {
            throw new System.NotImplementedException();
        }

        public void UpdatePrice(int shopId, string productName, double price)
        {
            throw new System.NotImplementedException();
        }

        public void UpdateQuantity(int shopId, string productName, int quantity)
        {
            throw new System.NotImplementedException();
        }

        public int GetMinPriceShopId(string productName)
        {
            throw new System.NotImplementedException();
        }

        public string GetShopName(int shopId)
        {
            throw new System.NotImplementedException();
        }

        public string GetShopAddress(int shopId)
        {
            throw new System.NotImplementedException();
        }

        public int GetProductQuantity(int shopId, string productName)
        {
            throw new System.NotImplementedException();
        }

        public bool BuyProducts(int shopId, (string, int)[] productsData)
        {
            throw new System.NotImplementedException();
        }

        public (string, int)[] GetProductsForPrice(int shopId, double totalMaxPrice)
        {
            throw new System.NotImplementedException();
        }

        public double GetPurchaseTotal(int shopId, (string, int)[] productsData)
        {
            throw new System.NotImplementedException();
        }

        public int GetMinPurchaseTotalShopId((string, int)[] productsData)
        {
            throw new System.NotImplementedException();
        }

        public bool IsShopExists(int shopId)
        {
            throw new System.NotImplementedException();
        }

        public bool IsShopExists(string shopName)
        {
            throw new System.NotImplementedException();
        }

        public bool IsProductExists(string productName)
        {
            throw new System.NotImplementedException();
        }

        
        private string _shopsDataFilepath;
        private string _productsDataFilepath;
        
        private static readonly Regex ShopsDataFilepathRegex = new Regex(@"\s*(;|^)\s*shopData\s*=\s*(.*?)(;|$)");
        private static readonly Regex ProductsDataFilepathRegex = new Regex(@"\s*(;|^)\s*productData\s*=\s*(.*?)(;|$)");

        private static TempShopCatalog ParseCsvFiles(string shopDataFilepath, string productsDataFilepath)
        {
            TempShopCatalog catalog = new TempShopCatalog();
            ParseShopData(ref catalog, shopDataFilepath);
            ParseProductData(ref catalog, productsDataFilepath);
            return catalog;
        }

        private static void ParseShopData(ref TempShopCatalog catalog, string shopDataFilepath)
        {
            throw new NotImplementedException();
        }
        
        private static void ParseProductData(ref TempShopCatalog catalog, string productDataFilepath)
        {
            throw new NotImplementedException();
        }
    }
    
}