using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using ShopCatalog.MariaDBDao.Exceptions;

namespace ShopCatalog.MariaDBDao
{
    class MariaDBDao : IDao
    {
        internal MariaDBDao(string connectionString)
        {
            _connection = new MySqlConnection(connectionString);

            try
            {
                _connection.Open();
            }
            catch (MySqlException e)
            {
                throw new DatabaseConnectErrorException(e.ToString());
            }
            finally
            {
                _connection.Close();
            }
        }

        public (int, string, string)[] GetShops()
        {

            (int, string, string)[] shopsData;
            
            try
            {
                _connection.Open();
                using (MySqlCommand command = _connection.CreateCommand())
                {
                    command.CommandText = @"SELECT COUNT(ShopId) FROM Shop";
                    int shopCount;

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        reader.Read();
                        shopCount = int.Parse(reader[0].ToString());
                    }

                    command.CommandText = @"SELECT ShopID, ShopName, ShopAddress FROM Shop";
                    shopsData = new (int, string, string)[shopCount];
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        for (int i = 0; i < shopsData.Length; ++i)
                        {
                            reader.Read();
                            int shopId = int.Parse(reader[0].ToString());
                            string shopName = reader[1].ToString();
                            string shopAddress = reader[2].ToString();
                            shopsData[i].Item1 = shopId;
                            shopsData[i].Item2 = shopName;
                            shopsData[i].Item3 = shopAddress;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                _connection.Close();
                throw new DatabaseDataReadingException(e.ToString());
            }
            
            return shopsData; 
        }

        public string[] GetProducts()
        {
            string[] shopList;

            try
            {
                _connection.Open();
                using (MySqlCommand command = _connection.CreateCommand())
                {
                    command.CommandText = @"SELECT COUNT(ProductID) FROM Product";
                    int shopCount;

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        reader.Read();
                        shopCount = int.Parse(reader[0].ToString());
                    }

                    command.CommandText = @"SELECT DISTINCT ProductName FROM Product";
                    shopList = new string[shopCount];
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        for (int i = 0; i < shopList.Length; ++i)
                        {
                            reader.Read();
                            shopList[i] = reader[0].ToString();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                _connection.Close();
                throw new DatabaseDataReadingException(e.ToString());
            }
            
            return shopList;
        }
        
        public int GetMinPriceShopId(string productName)
        {
            throw new System.NotImplementedException();
        }

        public string GetShopName(int shopId)
        {
            throw new System.NotImplementedException();
        }

        public string GetShopAddress(int shopId)
        {
            throw new System.NotImplementedException();
        }

        public int GetProductsCount(int shopId, string productName)
        {
            throw new System.NotImplementedException();
        }

        public string[] GetProductsForPrice(int shopId, double totalMaxPrice)
        {
            throw new System.NotImplementedException();
        }

        public int GetMinTotalShopId(List<string> productsNames, List<int> productsCounts)
        {
            throw new System.NotImplementedException();
        }

        public bool IsShopExists(int shopId)
        {
            throw new NotImplementedException();
        }

        public bool IsShopExists(string shopName)
        {
            throw new NotImplementedException();
        }

        public bool IsProductExists(string productName)
        {
            throw new NotImplementedException();
        }
        
        public void CreateShop(int shopId, string shopName, string shopAddress)
        {
            throw new System.NotImplementedException();
        }

        public void CreateProduct(string productName)
        {
            throw new System.NotImplementedException();
        }

        public void AddProductToShop(int shopId, string productName, int count)
        {
            throw new System.NotImplementedException();
        }
        public void BuyProducts(int shopId, List<string> productsNames, List<int> productsCounts)
        {
            throw new System.NotImplementedException();
        }
        internal void Dispose()
        {
            throw new System.NotImplementedException();
        }

        private MySqlConnection _connection;
    }
}