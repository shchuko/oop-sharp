using System;

namespace ShopCatalog
{
    public class Service
    {
        public string[] ExecuteCommand(string command)
        {
            // TODO
            string[] result  = _dao.GetProducts();
            return result;
        }
        
        internal Service(IDao dao)
        {
            _dao = dao;
        }

        internal void ReconnectDao(IDao dao)
        {
            _dao = dao;
        }
    
        private IDao _dao;
    }
}