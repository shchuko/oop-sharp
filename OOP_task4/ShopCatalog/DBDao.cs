using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace ShopCatalog
{
    class DBDao : IDao
    {
        internal DBDao(string hostname, string port, string user, string password, string databaseName, 
            string sslMode = "none")
        {    
            string connectionString = $"server={hostname};port={port};user id={user}; password={password}; " +
                                      $"database={databaseName}; SslMode={sslMode}"; 
            _connection = new MySqlConnection(connectionString);

            try
            {
                _connection.Open();
            }
            catch (MySqlException e)
            {
                throw new DatabaseConnectErrorException();
            }
            finally
            {
                _connection.Close();
            }
        }
        
        public void CreateShop(int shopId, string shopName, string shopAddress)
        {
            throw new System.NotImplementedException();
        }

        public void CreateProduct(string productName)
        {
            throw new System.NotImplementedException();
        }

        public (int, string, string)[] GetShops()
        {
            throw new System.NotImplementedException();
        }

        public string[] GetProducts()
        {
            string[] shopList = null;

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

                    command.CommandText = @"SELECT ProductName FROM Product";
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

        public void PurchaseProduct(int shopId, string productName, int count)
        {
            throw new System.NotImplementedException();
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

        public void BuyProducts(int shopId, List<string> productsNames, List<int> productsCounts)
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

        internal void Dispose()
        {
            throw new System.NotImplementedException();
        }

        private MySqlConnection _connection;
    }
}