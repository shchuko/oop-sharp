using System;
using System.Collections.Generic;

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

            List<string> productsNames = new List<string>();
            List<int> productsQuantities = new List<int>();
            productsNames.Add("Product1");
            productsQuantities.Add(1);
            productsNames.Add("Product9");
            productsQuantities.Add(2);
            productsNames.Add("Product3");
            productsQuantities.Add(3);

            int shopId = _dao.GetMinPurchaseTotalShopId(productsNames, productsQuantities);
            string[] strData = 
            {
//               _dao.GetMinTotalShopId(null, null).ToString()
                shopId.ToString(),
                (shopId != -1 ? _dao.GetPurchaseTotal(shopId, productsNames, productsQuantities) : shopId).ToString()
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