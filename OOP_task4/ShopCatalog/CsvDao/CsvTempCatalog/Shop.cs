namespace ShopCatalog.CsvDao.CsvTempCatalog
{
    internal struct Shop
    {
        Shop(int id, string name, string address)
        {
            Id = id;
            Name = name;
            Address = address;
        }
        
        internal int Id { get; }
        internal string Name { get; }
        internal string Address { get; }
   }
}