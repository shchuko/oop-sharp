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
        
        public void CreateShop(int shopId, string shopName, string shopAddress)
        {
            TempShopCatalog catalog = ParseCsvFiles(_shopsDataFilepath, _productsDataFilepath);
            catalog.CreateShop(shopId, shopName, shopAddress);
            WriteDataToCsvFiles(catalog, _shopsDataFilepath, _productsDataFilepath);
        }

        public void CreateProduct(string productName)
        {
            TempShopCatalog catalog = ParseCsvFiles(_shopsDataFilepath, _productsDataFilepath);
            catalog.CreateProduct(productName);
            WriteDataToCsvFiles(catalog, _shopsDataFilepath, _productsDataFilepath);
        }

        public (int, string, string)[] GetShops()
        {
            TempShopCatalog catalog = ParseCsvFiles(_shopsDataFilepath, _productsDataFilepath);
            return catalog.GetShops();
        }

        public string[] GetProducts()
        {
            TempShopCatalog catalog = ParseCsvFiles(_shopsDataFilepath, _productsDataFilepath);
            return catalog.GetProducts();
        }

        public void AddProductToShop(int shopId, string productName, double price, int quantity)
        {
            TempShopCatalog catalog = ParseCsvFiles(_shopsDataFilepath, _productsDataFilepath);
            catalog.AddProductToShop(shopId, productName, price, quantity);
            WriteDataToCsvFiles(catalog, _shopsDataFilepath, _productsDataFilepath);
        }

        public void UpdatePrice(int shopId, string productName, double price)
        {
            TempShopCatalog catalog = ParseCsvFiles(_shopsDataFilepath, _productsDataFilepath);
            catalog.UpdatePrice(shopId, productName, price);
            WriteDataToCsvFiles(catalog, _shopsDataFilepath, _productsDataFilepath);
        }

        public void UpdateQuantity(int shopId, string productName, int quantity)
        {
            TempShopCatalog catalog = ParseCsvFiles(_shopsDataFilepath, _productsDataFilepath);
            catalog.UpdateQuantity(shopId, productName, quantity);
            WriteDataToCsvFiles(catalog, _shopsDataFilepath, _productsDataFilepath);
        }

        public int GetMinPriceShopId(string productName)
        {
            TempShopCatalog catalog = ParseCsvFiles(_shopsDataFilepath, _productsDataFilepath);
            return catalog.GetMinPriceShopId(productName);
        }

        public string GetShopName(int shopId)
        {
            TempShopCatalog catalog = ParseCsvFiles(_shopsDataFilepath, _productsDataFilepath);
            return catalog.GetShopName(shopId);
        }

        public string GetShopAddress(int shopId)
        {
            TempShopCatalog catalog = ParseCsvFiles(_shopsDataFilepath, _productsDataFilepath);
            return catalog.GetShopAddress(shopId);
        }

        public int GetProductQuantity(int shopId, string productName)
        {
            TempShopCatalog catalog = ParseCsvFiles(_shopsDataFilepath, _productsDataFilepath);
            return catalog.GetProductQuantity(shopId, productName);
        }

        public bool BuyProducts(int shopId, (string, int)[] productsData)
        {
            TempShopCatalog catalog = ParseCsvFiles(_shopsDataFilepath, _productsDataFilepath);
            bool result = catalog.BuyProducts(shopId, productsData);
            WriteDataToCsvFiles(catalog, _shopsDataFilepath, _productsDataFilepath);
            return result;
        }

        public (string, int)[] GetProductsForPrice(int shopId, double totalMaxPrice)
        {
            TempShopCatalog catalog = ParseCsvFiles(_shopsDataFilepath, _productsDataFilepath);
            return catalog.GetProductsForPrice(shopId, totalMaxPrice);
        }

        public double GetPurchaseTotal(int shopId, (string, int)[] productsData)
        {
            TempShopCatalog catalog = ParseCsvFiles(_shopsDataFilepath, _productsDataFilepath);
            return catalog.GetPurchaseTotal(shopId, productsData);
        }

        public int GetMinPurchaseTotalShopId((string, int)[] productsData)
        {
            TempShopCatalog catalog = ParseCsvFiles(_shopsDataFilepath, _productsDataFilepath);
            return catalog.GetMinPurchaseTotalShopId(productsData);
        }

        public bool IsShopExists(int shopId)
        {
            TempShopCatalog catalog = ParseCsvFiles(_shopsDataFilepath, _productsDataFilepath);
            return catalog.IsShopExists(shopId);
        }

        public bool IsShopExists(string shopName)
        {
            TempShopCatalog catalog = ParseCsvFiles(_shopsDataFilepath, _productsDataFilepath);
            return catalog.IsShopExists(shopName);
        }

        public bool IsProductExists(string productName)
        {
            TempShopCatalog catalog = ParseCsvFiles(_shopsDataFilepath, _productsDataFilepath);
            return catalog.IsProductExists(productName);
        }

        
        private readonly string _shopsDataFilepath;
        private readonly string _productsDataFilepath;
        
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