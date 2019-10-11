using System.Collections.Generic;

namespace ShopCatalog
{
    interface IDaoReader
    {
        /** Get list of shops with full info
        * @return tuple(int, string, string) ShopID, ShopName, ShopAddress
        */
        (int, string, string)[] GetShops();

        /** Get list of products
        * @return array of products' names
        */
        string[] GetProducts();

        /** Get shop ID with minimal price for product, if quantity of product in the shop > 0
         * @return ShopId if product found, -1 if product not found or quantity == 0
         */
        int GetMinPriceShopId(string productName);

        /**
         * Get shop name related to shop id
         * @param shopId Id of the shop
         * @return Shop name or empty string if shop does not exists
         */
        string GetShopName(int shopId);

        /**
         * Get shop address related to shop id
         * @param shopId Id of the shop
         * @return Shop address or empty string if shop does not exists
         */
        string GetShopAddress(int shopId);

        /** Get product quantity in the shop[shopId] by productName
         * @param shopId shop to search in
         * @param productName name of product to search
         * @return quantity of products or -1 if product or shop not exists
         */
        int GetProductQuantity(int shopId, string productName);

        /** Get product price in the shop[shopId] by productName
         * @param shopId shop to search in
         * @param productName name of product to search
         * @return price of product or -1 if product or shop not exists
         */
        double GetProductPrice(int shopId, string productName);

        /** Get info how many products you can buy for fixed price in the shop
         * @param shopId Shop ID to search products
         * @totalMaxPrice Maximum price that can be
         * @return Tuple (product name, quantity)
         */
        (string, int)[] GetProductsForPrice(int shopId, double totalMaxPrice);

        /** Calculate total sum of purchase
         * @param shopId Shop where purchase will be made
         * @param @param productsData array of (productName, quantity) tuples
         * @return Total sum of purchase or -1 if some product not exists/quantity is not enough
         */
        double GetPurchaseTotal(int shopId, (string, int)[] productsData);

        /** Get shop id with minimum total price for purchase
         * @param productsData array of (productName, quantity) tuples
         * @return shopId or -1 if there is no shop to buy all products of list
         */
        int GetMinPurchaseTotalShopId((string, int)[] productsData);

        /** Checks if shop exists by shop id
         * @param shopId Id of the shop
         * @return true if exists, false if not
         */
        bool IsShopExists(int shopId);

        /** Checks if shop exists by shop name
          * @param shopName Name of the shop
          * @return true if exists, false if not
          */
        bool IsShopExists(string shopName);

        /** Checks if product exists 
          * @param productName Name of the product
          * @return true if exists, false if not
          */
        bool IsProductExists(string productName);

        /** Checks if product exists in ths shop
          * @param shopId Id of the shop to check in 
          * @param productName Name of the product
          * @return true if exists, false if not
          */
        bool IsShopContainsProduct(int shopId, string productName);

    }
}