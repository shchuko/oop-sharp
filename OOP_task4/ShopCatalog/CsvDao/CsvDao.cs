using System;
using System.Dynamic;
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
            using (var reader = System.IO.File.OpenText(shopDataFilepath))
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
            using (var reader = System.IO.File.OpenText(productDataFilepath))
            {
                try
                {
                    while (!reader.EndOfStream)
                    {
                        string[] productsData = reader.ReadLine().Split(',');
                        string productName = productsData[0];
                        catalog.CreateProduct(productName);
                        for (int i = 1; i < productsData.Length; i += 3)
                        {
                            int shopId = int.Parse(productsData[i]);
                            double price = double.Parse(productsData[i + 1]);
                            int quantity = int.Parse(productsData[i + 2]);
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