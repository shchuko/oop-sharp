using System.Collections.Generic;

namespace ShopCatalog.CsvDao
{
    class CsvDao : IDao
    {
        internal CsvDao(string shopFilePath, string productFilePath)
        {

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

        public void AddProductToShop(int shopId, string productName, int count)
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

        public void BuyProducts(int shopId, List<string> productsNames, List<int> productsCounts)
        {
            throw new System.NotImplementedException();
        }

        public string[] GetProductsForPrice(int shopId, double totalMaxPrice)
        {
            throw new System.NotImplementedException();
        }

        public double GetPurchaseTotal(int shopId, List<string> productsNames, List<int> productsQuantities)
        {
            throw new System.NotImplementedException();
        }

        public int GetMinPurchaseTotalShopId(List<string> productsNames, List<int> productsQuantities)
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
    }
}