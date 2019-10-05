using System;

namespace ShopCatalog
{
    public class Service
    {
        public string[] ExecuteCommand(string command)
        {
            // TODO
//            var result  = _dao.GetShops();
//            string[] strData = new string[result.Length];
//            for (int i = 0; i < result.Length; ++i)
//            {
//                strData[i] = result[i].Item1 + "\t" + result[i].Item2 +  "\t" +result[i].Item3;
//            }
            string[] strData = {_dao.GetShopName(0), _dao.GetShopName(1), _dao.GetShopAddress(1)  };
            return strData;
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