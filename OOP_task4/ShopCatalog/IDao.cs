using System.Collections.Generic;

namespace ShopCatalog
{
    interface IDao
    {
        void CreateShop(int shopId, string shopName, string shopAddress);

        void CreateProduct(string productName);

        (int, string, string)[] GetShops();

        string[] GetProducts();
        
        void PurchaseProduct(int shopId, string productName, int count);

        int GetMinPriceShopId(string productName);

        string GetShopName(int shopId);

        string GetShopAddress(int shopId);

        int GetProductsCount(int shopId, string productName);

        void BuyProducts(int shopId, List<string> productsNames, List<int> productsCounts);

        string[] GetProductsForPrice(int shopId, double totalMaxPrice);

        int GetMinTotalShopId(List<string> productsNames, List<int> productsCounts);
        
    }
}