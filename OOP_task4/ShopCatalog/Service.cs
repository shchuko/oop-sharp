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

//            List<string> productsNames = new List<string>();
//            List<int> productsQuantities = new List<int>();
//            productsNames.Add("Product1");
//            productsQuantities.Add(1);
//            productsNames.Add("Product9");
//            productsQuantities.Add(2);
//            productsNames.Add("Product3");
//            productsQuantities.Add(3);

//            int shopId = _dao.GetMinPurchaseTotalShopId(productsNames, productsQuantities);
//            string[] strData = 
//            {
////               _dao.GetMinTotalShopId(null, null).ToString()
//                shopId.ToString(),
//                (shopId != -1 ? _dao.GetPurchaseTotal(shopId, productsNames, productsQuantities) : shopId).ToString()
//            };

            List<string> list = new List<string>();
            var x = _dao.GetProductsForPrice(2, 10);

//            _dao.CreateShop(10, "Shop10", "Kiev");
//            _dao.CreateProduct("Product5");
//            _dao.AddProductToShop(1, "Product5", 10.5, 3);
//            _dao.UpdateQuantity(1, "Product5", 1);
            Console.WriteLine(_dao.GetPurchaseTotal(1, new[] {("Product5", 1)}));
            foreach (var valueTuple in x)
            {
                list.Add($"{valueTuple.Item1} - {valueTuple.Item2.ToString()}");
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