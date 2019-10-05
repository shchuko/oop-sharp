using System.Collections.Generic;

namespace ShopCatalog
{
    public interface IDaoWriter
    {
        void CreateShop(int shopId, string shopName, string shopAddress);

        void CreateProduct(string productName);

        void AddProductToShop(int shopId, string productName, int count);
        
        bool BuyProducts(int shopId, List<string> productsNames, List<int> productsCounts);
        
    }
}