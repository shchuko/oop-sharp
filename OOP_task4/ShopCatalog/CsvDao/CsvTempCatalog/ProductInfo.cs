namespace ShopCatalog.CsvDao.CsvTempCatalog
{
    internal class ProductInfo
    {
        internal ProductInfo(int quantity, double price)
        {
            Quantity = quantity;
            Price = price;
        }
        internal double Price { get; set; }
        internal int Quantity { get; set; }
    }
}