using System;
using System.Dynamic;
using System.Reflection;
using ShopCatalog.Exceptions;
using ShopCatalog.MariaDBDao;

namespace ShopCatalog
{
    public static class Manager
    {
        public static Service CreateService(Type serviceEngineType, string arguments)
        {
            IDao dao;
            try
            {
                var constructors = serviceEngineType.GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic);
                dao = (IDao) constructors[0].Invoke( new object[] {arguments} );
            }
            catch (Exception e)
            {
                throw new ServiceCreationException(e.ToString());
            }
            
            return new Service(dao);
        }
        
    }
}