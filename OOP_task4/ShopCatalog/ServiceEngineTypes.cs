using System;

namespace ShopCatalog
{
    public static class ServiceEngineTypes
    {
        public static readonly Type MariaDBEngineType = typeof(MariaDBDao.DBDao);
        public static readonly Type CsvEngineType = typeof(MariaDBDao.DBDao);
    }
}