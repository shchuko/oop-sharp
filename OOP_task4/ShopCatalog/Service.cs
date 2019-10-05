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
            string[] strData =
            {
                _dao.GetProductsCount(1,"Product2").ToString(),
                _dao.GetProductsCount(2,"Product1").ToString(),

                _dao.GetProductsCount(5,"Product1").ToString(),
                _dao.GetProductsCount(1,"Product4").ToString(),
                _dao.GetProductsCount(5,"Product4").ToString()

            };
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