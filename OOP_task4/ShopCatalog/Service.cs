using System;
using System.Collections.Generic;

namespace ShopCatalog
{
    public class Service
    {
        public string[] ExecuteCommand(string command)
        {
            
//            _dao.CreateShop(3, "Пятерка", "Мурманск");
            List<string> list = new List<string>();
            foreach (var shop in _dao.GetShops())
            {
                list.Add(shop.ToString());
            }
            return list.ToArray();
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