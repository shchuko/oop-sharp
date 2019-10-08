using System;

namespace ShopCatalog
{
    public static class ServiceEngineTypes
    {
        public static readonly Type MariaDBEngineType = typeof(MariaDBDao.MariaDBDao);
        public static readonly Type CsvEngineType = typeof(CsvDao.CsvDao);
    }
}