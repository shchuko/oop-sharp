using System;

namespace ShopCatalog
{
    public static class Manager
    {
        public static Service CreateService(string daoData)
        {
//            throw new NotImplementedException();
            return new Service(new DBDao("localhost", "3306", "shopAdmin", "password",
                "ShopDB"));
        }

        public static void ReconnectServiceDao(Service service, string daoData)
        {
            throw new NotImplementedException();
        }
    }
}