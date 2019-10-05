using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using ShopCatalog.MariaDBDao.Exceptions;

namespace ShopCatalog.MariaDBDao
{
    class MariaDBDao : IDao
    {

        /** Main constructor
         * @param connectionString MySql connection string, check MySql library
         */
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

        /** Get list of shops with full info
         * @return tuple(int, string, string) ShopID, ShopName, ShopAddress
         */
        public (int, string, string)[] GetShops()
        {
            (int, string, string)[] shopsData;

            try
            {
                if (_connection.State != ConnectionState.Open)
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
                throw new DatabaseDataReadingException(e.ToString());
            }
            finally
            {
                _connection.Close();
            }
            
            return shopsData; 
        }

        /** Get list of products
         * @return array of products' names
         */
        public string[] GetProducts()
        {
            string[] shopList;

            try
            {
                if (_connection.State != ConnectionState.Open)
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
                throw new DatabaseDataReadingException(e.ToString());
            }
            finally
            {
                _connection.Close();
            }
            
            return shopList;
        }
        
        /** Get shop ID with minimal price for product, if count of product in the shop > 0
         * @return ShopId if product found, -1 if product not found or count == 0
         */
        public int GetMinPriceShopId(string productName)
        {
            int shopId = -1;
            try
            {
                using (MySqlCommand command = _connection.CreateCommand())
                {
                    int productId = GetProductId(productName);
                    if (productId != -1)
                    {
                        command.CommandText =
                            @"SELECT ShopID FROM ShopProduct WHERE ProductID = @productID AND Count > 0 ORDER BY Price LIMIT 1";
                        command.Parameters.Add("@productId", MySqlDbType.VarChar).Value = productId;

                        if (_connection.State != ConnectionState.Open)
                            _connection.Open();
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                reader.Read();
                                shopId = int.Parse(reader[0].ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw new DatabaseDataReadingException(e.ToString());
            }
            finally
            {
                _connection.Close();
            }
            
            return shopId;
        }

        /**
         * Get shop name related to shop id
         * @param shopId Id of the shop
         * @return Shop name or empty string if shop does not exists
         */
        public string GetShopName(int shopId)
        {
            string shopName = "";
            try
            {
                if (_connection.State != ConnectionState.Open)
                    _connection.Open();
                using (MySqlCommand command = _connection.CreateCommand())
                {
                    command.CommandText = @"SELECT ShopName FROM Shop WHERE ShopID = @shopID";
                    command.Parameters.Add("@shopID", MySqlDbType.Int32);
                    command.Parameters["@shopID"].Value = shopId;

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();
                            shopName = reader[0].ToString();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw new DatabaseDataReadingException(e.ToString());
            }
            finally
            {
                _connection.Close();
            }
            
            return shopName;
        }

        /**
         * Get shop address related to shop id
         * @param shopId Id of the shop
         * @return Shop address or empty string if shop does not exists
         */
        public string GetShopAddress(int shopId)
        {
            string shopAddress = "";
            try
            {
                if (_connection.State != ConnectionState.Open)
                    _connection.Open();
                using (MySqlCommand command = _connection.CreateCommand())
                {
                    command.CommandText = @"SELECT ShopAddress FROM Shop WHERE ShopID = @shopID";
                    command.Parameters.Add("@shopID", MySqlDbType.Int32);
                    command.Parameters["@shopID"].Value = shopId;

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();
                            shopAddress = reader[0].ToString();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw new DatabaseDataReadingException(e.ToString());
            }
            finally
            {
                _connection.Close();
            }
            
            return shopAddress;
        }

        /** Get product count in the shop[shopId] by productName
         * @param shopId shop to search in
         * @param productName name of product to search
         * @return count of products or -1 if product or shop not exists
         */
        public int GetProductsCount(int shopId, string productName)
        {
            int productId = GetProductId(productName);
            if (productId == -1)
                return -1;

            int count = -1;
            try
            {
                if (_connection.State != ConnectionState.Open)
                    _connection.Open();
                using (MySqlCommand command = _connection.CreateCommand())
                {
                    command.CommandText =
                        @"SELECT Count FROM ShopProduct WHERE ShopID = @shopID AND ProductID = @productID";
                    command.Parameters.Add("@shopID", MySqlDbType.Int32);
                    command.Parameters["@shopID"].Value = shopId;
                    command.Parameters.Add("@productID", MySqlDbType.Int32);
                    command.Parameters["@productID"].Value = productId;

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();
                            count = int.Parse(reader[0].ToString());
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw new DatabaseDataReadingException(e.ToString());
            }
            finally
            {
                _connection.Close();
            }
            
            return count;
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

        /** Get product id related to product name
         * @param productName name of product
         * @returns product id if product exists, else -1
         */
        private int GetProductId(string productName)
        {
            int productId = -1;
            try
            {
                if (_connection.State != ConnectionState.Open)
                    _connection.Open();
                using (MySqlCommand command = _connection.CreateCommand())
                {
                    command.CommandText = @"SELECT ProductID FROM Product WHERE ProductName = @productName";
                    command.Parameters.Add("@productName", MySqlDbType.VarChar);
                    command.Parameters["@productName"].Value = productName;

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();
                            productId = int.Parse(reader[0].ToString());
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw new DatabaseDataReadingException(e.ToString());
            }
            finally
            {
                _connection.Close();
            }
            
            return productId;
        }
    }
}