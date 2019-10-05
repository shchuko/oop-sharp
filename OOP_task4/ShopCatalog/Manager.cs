using System;
using System.Reflection;
using ShopCatalog.Exceptions;

namespace ShopCatalog
{
    public static class Manager
    {
        /** Create service to operate with ShopCatalog
         * @param serviceEngineType Type of Service engine located in ShopCatalog.ServiceEngineTypes static class
         * @param arguments Arguments for service
         * @return new Service 
         */
        public static Service CreateService(Type serviceEngineType, string arguments)
        {
            IDao dao;
            try
            {
                var constructors = serviceEngineType.GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic);
                dao = (IDao) constructors[0].Invoke(new object[] { arguments });
            }
            catch (Exception e)
            {
                throw new ServiceCreationException(e.ToString());
            }
            
            return new Service(dao);
        }
        
    }
}