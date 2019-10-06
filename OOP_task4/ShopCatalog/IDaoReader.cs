using System.Collections.Generic;

namespace ShopCatalog
{
    interface IDaoReader
    {
        (int, string, string)[] GetShops();

        string[] GetProducts();

        int GetMinPriceShopId(string productName);

        string GetShopName(int shopId);

        string GetShopAddress(int shopId);

        int GetProductQuantity(int shopId, string productName);

        (string, int)[] GetProductsForPrice(int shopId, double totalMaxPrice);

        double GetPurchaseTotal(int shopId, (string, int)[] productsData);

        int GetMinPurchaseTotalShopId((string, int)[] productsData);

        bool IsShopExists(int shopId);

        bool IsShopExists(string shopName);

        bool IsProductExists(string productName);
        
    }
}