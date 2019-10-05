using System;
using System.Collections.Generic;
using System.Data;
using System.Security;
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

        /** Get product quantity in the shop[shopId] by productName
         * @param shopId shop to search in
         * @param productName name of product to search
         * @return quantity of products or -1 if product or shop not exists
         */
        public int GetProductQuantity(int shopId, string productName)
        {
            int productId = GetProductId(productName);
            if (productId == -1)
                return -1;

            int quantity = -1;
            try
            {
                if (_connection.State != ConnectionState.Open)
                    _connection.Open();
                using (MySqlCommand command = _connection.CreateCommand())
                {
                    command.CommandText =
                        @"SELECT Quantity FROM ShopProduct WHERE ShopID = @shopID AND ProductID = @productID";
                    command.Parameters.Add("@shopID", MySqlDbType.Int32);
                    command.Parameters["@shopID"].Value = shopId;
                    command.Parameters.Add("@productID", MySqlDbType.Int32);
                    command.Parameters["@productID"].Value = productId;

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();
                            quantity = int.Parse(reader[0].ToString());
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

            return quantity;
        }

        /** Get shop ID with minimal price for product, if quantity of product in the shop > 0
         * @return ShopId if product found, -1 if product not found or quantity == 0
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
                            @"SELECT ShopID FROM ShopProduct WHERE ProductID = @productID AND Quantity > 0 ORDER BY Price LIMIT 1";
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

        /** Get info how many products you can buy for fixed price in the shop
         * @param shopId Shop ID to search products
         * @totalMaxPrice Maximum price that can be
         * @return Tuple (product name, quantity)
         */
        public (string, int)[] GetProductsForPrice(int shopId, double totalMaxPrice)
        {
            (string, int)[] resultTuples;
            try
            {
                using (MySqlCommand command = _connection.CreateCommand())
                {
                    command.CommandText =
                        @"SELECT ProductName, LEAST(Quantity, FLOOR(@maxPrice/Price))
                            FROM ShopProduct 
                                INNER JOIN Product ON ShopProduct.ProductID = Product.ProductID 
                            WHERE ShopID = @shopID
                                AND Price <= @maxPrice
                                AND Quantity > 0";
                    command.Parameters.Add("@shopID", MySqlDbType.Int32).Value = shopId;
                    command.Parameters.Add("@maxPrice", MySqlDbType.Double).Value = totalMaxPrice;
                    if (_connection.State != ConnectionState.Open)
                        _connection.Open();
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        List<(string, int)> tempData = new List<(string, int)>();
                        while (reader.Read())
                        {
                            string productName = reader[0].ToString();
                            int productQuantity = int.Parse(reader[1].ToString());
                            tempData.Add((productName, productQuantity));
                        }

                        resultTuples = tempData.ToArray();
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

            return resultTuples;
        }

        /** Calculate total sum of purchase
         * @param shopId Shop where purchase will be made
         * @param productNames List of products' names needed to buy
         * @param productsQuantities List of products' quantities related to productsNames
         * @return Total sum of purchase or -1 if some product not exists/quantity is not enough
         */
        public double GetPurchaseTotal(int shopId, List<string> productsNames, List<int> productsQuantities)
        {
            double sum = 0;

            try
            {
                using (MySqlCommand command = _connection.CreateCommand())
                {
                    command.Parameters.Add("@shopID", DbType.Int32);
                    command.Parameters.Add("@productID", DbType.Int32);
                    command.Parameters.Add("@productQuantity", DbType.Int32);
                    for (int i = 0; i < productsNames.Count; ++i)
                    {
                        int productId = GetProductId(productsNames[i]);
                        int productQuantity = productsQuantities[i];

                        if (productId == -1)
                        {
                            sum = -1;
                            break;
                        }
                        
                        command.CommandText =
                            @"SELECT Price FROM ShopProduct 
                                    WHERE ProductID = @productID 
                                        AND ShopID = @shopID
                                        AND Quantity >= @productQuantity";
                        command.Parameters["@shopID"].Value = shopId;
                        command.Parameters["@productID"].Value = productId;
                        command.Parameters["@productQuantity"].Value = productQuantity;
                        
                        if (_connection.State != ConnectionState.Open)
                            _connection.Open();
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            if (!reader.HasRows)
                            {
                                sum = -1;
                                break;
                            }

                            reader.Read();
                            sum += double.Parse(reader[0].ToString()) * productQuantity;
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

            return sum;
        }
        
        /** Get shop id with minimum total price for purchase
         * @param productsNames names of products
         * @param productsQuantities quantity of each product to purchase
         * @return shopId or -1 if there is no shop to buy all products of list
         */
        public int GetMinPurchaseTotalShopId(List<string> productsNames, List<int> productsQuantities)
        {
            int resultShopId = -1;
            double minSum = double.MaxValue;

            try
            {
                var shopsData = GetShops();
                foreach (var tuple in shopsData)
                {
                    int shopId = tuple.Item1;
                    double sum = GetPurchaseTotal(shopId, productsNames, productsQuantities);
                    if (sum >= 0 && sum < minSum)
                    {
                        minSum = sum;
                        resultShopId = shopId;
                    }
                }
            }
            catch (Exception e)
            {
                throw new DatabaseDataReadingException(e.ToString());
            }

            return resultShopId;
        }


        /** Checks if shop exists by shop id
         * @param shopId Id of the shop
         * @return true if exists, false if not
         */
        public bool IsShopExists(int shopId)
        {
            bool result;
            try
            {
                if (_connection.State != ConnectionState.Open)
                    _connection.Open();
                using (MySqlCommand command = _connection.CreateCommand())
                {
                    command.CommandText = @"SELECT ShopID FROM Shop WHERE ShopID = @shopID";
                    command.Parameters.Add("@shopID", MySqlDbType.Int32);
                    command.Parameters["@shopID"].Value = shopId;

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        result = reader.HasRows;
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

            return result;
        }

        /** Checks if shop exists by shop name
          * @param shopName Name of the shop
          * @return true if exists, false if not
          */
        public bool IsShopExists(string shopName)
        {
            bool result;
            try
            {
                if (_connection.State != ConnectionState.Open)
                    _connection.Open();
                using (MySqlCommand command = _connection.CreateCommand())
                {
                    command.CommandText = @"SELECT ShopID FROM Shop WHERE ShopName = @shopName";
                    command.Parameters.Add("@shopName", MySqlDbType.VarChar);
                    command.Parameters["@shopName"].Value = shopName;

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        result = reader.HasRows;
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

            return result;
        }

        /** Checks if product exists 
          * @param productName Name of the product
          * @return true if exists, false if not
          */
        public bool IsProductExists(string productName)
        {
            return GetProductId(productName) != -1;
        }

        public void CreateShop(int shopId, string shopName, string shopAddress)
        {
            throw new System.NotImplementedException();
        }

        public void CreateProduct(string productName)
        {
            throw new System.NotImplementedException();
        }

        public void AddProductToShop(int shopId, string productName, int quantity)
        {
            throw new System.NotImplementedException();
        }

        public bool BuyProducts(int shopId, List<string> productsNames, List<int> productsQuantities)
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