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

        int GetProductsCount(int shopId, string productName);

        string[] GetProductsForPrice(int shopId, double totalMaxPrice);

        int GetMinTotalShopId(List<string> productsNames, List<int> productsCounts);

        bool IsShopExists(int shopId);

        bool IsShopExists(string shopName);

        bool IsProductExists(string productName);
        
    }
}