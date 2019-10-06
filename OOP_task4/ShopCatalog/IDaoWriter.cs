using System.Collections.Generic;

namespace ShopCatalog
{
    public interface IDaoWriter
    {
        void CreateShop(int shopId, string shopName, string shopAddress);

        void CreateProduct(string productName);

        void AddProductToShop(int shopId, string productName, double price, int quantity);

        void UpdatePrice(int shopId, string productName, double price);

        void UpdateQuantity(int shopId, string productName, int quantity);
        
        bool BuyProducts(int shopId, (string, int)[] productsData);
        
    }
}