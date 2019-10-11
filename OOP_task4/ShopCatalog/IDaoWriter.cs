using System.Collections.Generic;

namespace ShopCatalog
{
    public interface IDaoWriter
    {
        /** Create and add shop 
         * @param shopId Unique id of the shop
         * @param shopName Name of the shop
         * @param shopAddress Address of the shop
         */
        void CreateShop(int shopId, string shopName, string shopAddress);

        /** Create and add product 
         * @param productName Unique name of product
         */
        void CreateProduct(string productName);

        /** Add the product to the shop with fixed price and quantity
         * @param shopID Shop id
         * @param productName Product name
         * @param price Product price
         * @param quantity Product quantity
         */
        void AddProductToShop(int shopId, string productName, double price, int quantity);

        /** Update price for the product in the shop
         * @param shopID Shop id
         * @param productName Product name
         * @param price New price
         */
        void UpdatePrice(int shopId, string productName, double price);

        /** Update quantity for the product in the shop
         * @param shopID Shop id
         * @param productName Product name
         * @param quantity New quantity
         */
        void UpdateQuantity(int shopId, string productName, int quantity);
        
        /** Buy products in the shop, decreases quantities
         * @param shopId Shop where products needed to buy
         * @param @param productsData array of (productName, quantity) tuples
         * @return true if operation successful, else false
         */
        bool BuyProducts(int shopId, (string, int)[] productsData);
        
    }
}