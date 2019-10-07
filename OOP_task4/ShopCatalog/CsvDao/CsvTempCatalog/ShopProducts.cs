using System.Collections.Generic;
using ShopCatalog.Exceptions;

namespace ShopCatalog.CsvDao.CsvTempCatalog
{
    internal class ShopProducts
    {
        internal void AddProduct(string productName, int quantity, double price)
        {
            if (HasProduct(productName))
            {
                throw new MissingDataConsistencyException("Duplicate product name");
            }
            
            _productsInShop.Add(productName, new ProductInfo(quantity, price));
        }

        internal void UpdateProductPrice(string productName, double price)
        {
            if (!HasProduct(productName))
            {
                throw new MissingDataConsistencyException("Product not exists");
            }

            _productsInShop[productName].Price = price;
        }

        internal void UpdateProductQuantity(string productName, int quantity)
        {
            if (!HasProduct(productName))
            {
                throw new MissingDataConsistencyException("Product not exists");
            }

            _productsInShop[productName].Quantity = quantity;
        }

        internal bool HasProduct(string productName)
        {
            return _productsInShop.ContainsKey(productName);
        }

        internal int GetQuantity(string productName)
        {
            return _productsInShop[productName].Quantity;
        }

        internal double GetPrice(string productName)
        {
            return _productsInShop[productName].Price;
        }

        internal (string, string)[] GetCsvData()
        {
            (string, string)[] data = new (string, string)[_productsInShop.Count];
            int counter = 0;
            foreach (KeyValuePair<string,ProductInfo> productInfo in _productsInShop)
            {
                data[counter].Item1 = productInfo.Key;
                data[counter].Item2 = $"{productInfo.Value.Quantity},{productInfo.Value.Price}";
                ++counter;
            }

            return data;
        }
        
        private Dictionary<string, ProductInfo> _productsInShop = new Dictionary<string, ProductInfo>();
    }
}