using System.Collections.Generic;

namespace ShopCatalog
{
    class CSVDao : IDao
    {
        CSVDao(string shopFilePath, string productFilePath, string shopProductFilepath)
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

        public void PurchaseProduct(int shopId, string productName, int count)
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

        public int GetProductsCount(int shopId, string productName)
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

        public int GetMinTotalShopId(List<string> productsNames, List<int> productsCounts)
        {
            throw new System.NotImplementedException();
        }
    }
}