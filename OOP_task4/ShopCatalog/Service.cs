using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using ShopCatalog.Exceptions;

namespace ShopCatalog
{
    public class Service
    {
        public string[] ExecuteCommand(string command)
        {
            return PrintShops();
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

        
        private string[] PrintShops()
        {
            var shopsData = _dao.GetShops();
            string[] result = new string[shopsData.Length];
            for (var i = 0; i < shopsData.Length; i++)
            {
                result[i] = shopsData[i].ToString();
            }

            return result;
        }

        private string[] PrintProducts()
        {
            return _dao.GetProducts();
        }
        
        private string[] PrintProductsInShop(string args)
        {
            Regex regex = new Regex(@".*shop-id=(\d+?).*");
            if (!regex.IsMatch(args))
            {
                return new []{ "Incorrect shopId" };
            }
            int shopId = int.Parse(regex.Match(args).Groups[1].Value);

            string[] products = _dao.GetProducts();
            List<string> result = new List<string>();
            result.Add($"Products in the shop #{shopId} - {_dao.GetShopName(shopId)}:");
            foreach (string productName in products)
            {
                if (_dao.IsShopContainsProduct(shopId, productName))
                {
                    result.Add($"({productName}, {_dao.GetProductQuantity(shopId, productName)}," +
                               $" {_dao.GetProductPrice(shopId, productName)})");
                }
            }

            return result.ToArray();
        }
        
    }
}