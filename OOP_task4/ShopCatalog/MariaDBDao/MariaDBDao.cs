using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using ShopCatalog.Exceptions;
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
         * @param @param productsData array of (productName, quantity) tuples
         * @return Total sum of purchase or -1 if some product not exists/quantity is not enough
         */
        public double GetPurchaseTotal(int shopId, (string, int)[] productsData)
        {
            double sum = 0;

            try
            {
                using (MySqlCommand command = _connection.CreateCommand())
                {
                    command.Parameters.Add("@shopID", DbType.Int32);
                    command.Parameters.Add("@productID", DbType.Int32);
                    command.Parameters.Add("@productQuantity", DbType.Int32);
                    for (int i = 0; i < productsData.Length; ++i)
                    {
                        int productId = GetProductId(productsData[i].Item1);
                        int productQuantity = productsData[i].Item2;

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
         * @param productsData array of (productName, quantity) tuples
         * @return shopId or -1 if there is no shop to buy all products of list
         */
        public int GetMinPurchaseTotalShopId((string, int)[] productsData)
        {
            int resultShopId = -1;
            double minSum = double.MaxValue;

            try
            {
                var shopsData = GetShops();
                foreach (var tuple in shopsData)
                {
                    int shopId = tuple.Item1;
                    double sum = GetPurchaseTotal(shopId, productsData);
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

        /** Create and add shop into Shop table
         * @param shopId Unique id of the shop
         * @param shopName Name of the shop
         * @param shopAddress Address of the shop
         */
        public void CreateShop(int shopId, string shopName, string shopAddress)
        {
            if (IsShopExists(shopId))
                throw new MissingDataConsistencyException($"ShopID '{shopId}' already exists");
            
            try
            {
                if (_connection.State != ConnectionState.Open)
                    _connection.Open();
                using (MySqlCommand command = _connection.CreateCommand())
                {
                    command.CommandText = @"INSERT INTO Shop VALUE (@shopID, @shopName, @shopAddress)";
                    command.Parameters.Add("@shopID", MySqlDbType.Int32).Value = shopId;
                    command.Parameters.Add("@shopName", MySqlDbType.VarChar).Value = shopName;
                    command.Parameters.Add("@shopAddress", MySqlDbType.VarChar).Value = shopAddress;
                    using (command.ExecuteReader())
                    {
                    }
                }
            }
            catch (Exception e)
            {
                throw new MissingDataConsistencyException(e.ToString());
            }
            finally
            {
                _connection.Close();
            }
        }

        /** Create and add product into Product table
         * @param productName Unique name of product
         */
        public void CreateProduct(string productName)
        {
            if (IsProductExists(productName))
                throw new MissingDataConsistencyException($"ProductName '{productName}' already exists");
            
            try
            {
                if (_connection.State != ConnectionState.Open)
                    _connection.Open();
                using (MySqlCommand command = _connection.CreateCommand())
                {
                    command.CommandText = @"INSERT INTO Product (ProductName) VALUE (@productName)";
                    command.Parameters.Add("@productName", MySqlDbType.VarChar).Value = productName;
                    using (command.ExecuteReader())
                    {
                    }
                }
            }
            catch (Exception e)
            {
                throw new MissingDataConsistencyException(e.ToString());
            }
            finally
            {
                _connection.Close();
            }
        }

        /** Add the product to the shop with fixed price and quantity
         * @param shopID Shop id
         * @param productName Product name
         * @param price Product price
         * @param quantity Product quantity
         */
        public void AddProductToShop(int shopId, string productName, double price, int quantity)
        {
            int productId = GetProductId(productName);
            try
            {
                if (_connection.State != ConnectionState.Open)
                    _connection.Open();
                using (MySqlCommand command = _connection.CreateCommand())
                {
                    command.CommandText =
                        @"INSERT INTO ShopProduct (ShopID, ProductID, Price, Quantity) VALUE (@shopID, @productID, @price, @quantity)";
                    command.Parameters.Add("@shopID", MySqlDbType.Int32).Value = shopId;
                    command.Parameters.Add("@productID", MySqlDbType.Int32).Value = productId;
                    command.Parameters.Add("@price", MySqlDbType.Double).Value = price;
                    command.Parameters.Add("@quantity", MySqlDbType.Int32).Value = quantity;

                    using (command.ExecuteReader())
                    {
                    }
                }
            }
            catch (Exception e)
            {
                throw new MissingDataConsistencyException(e.ToString());
            }
            finally
            {
                _connection.Close();
            }
        }

        /** Update price for the product in the shop
         * @param shopID Shop id
         * @param productName Product name
         * @param price New price
         */
        public void UpdatePrice(int shopId, string productName, double price)
        {
            int productId = GetProductId(productName);
            try
            {
                if (_connection.State != ConnectionState.Open)
                    _connection.Open();
                using (MySqlCommand command = _connection.CreateCommand())
                {
                    command.CommandText =
                        @"UPDATE ShopProduct SET Price = @price WHERE ShopID = @shopID AND ProductID = @productId";
                    command.Parameters.Add("@shopID", MySqlDbType.Int32).Value = shopId;
                    command.Parameters.Add("@productID", MySqlDbType.Int32).Value = productId;
                    command.Parameters.Add("@price", MySqlDbType.Double).Value = price;

                    using (command.ExecuteReader())
                    {
                    }
                }
            }
            catch (Exception e)
            {
                throw new MissingDataConsistencyException(e.ToString());
            }
            finally
            {
                _connection.Close();
            }
        }

        /** Update quantity for the product in the shop
         * @param shopID Shop id
         * @param productName Product name
         * @param quantity New quantity
         */
        public void UpdateQuantity(int shopId, string productName, int quantity)
        {
            int productId = GetProductId(productName);
            try
            {
                if (_connection.State != ConnectionState.Open)
                    _connection.Open();
                using (MySqlCommand command = _connection.CreateCommand())
                {
                    command.CommandText =
                        @"UPDATE ShopProduct SET Quantity = @quantity WHERE ShopID = @shopID AND ProductID = @productId";
                    command.Parameters.Add("@shopID", MySqlDbType.Int32).Value = shopId;
                    command.Parameters.Add("@productID", MySqlDbType.Int32).Value = productId;
                    command.Parameters.Add("@quantity", MySqlDbType.Int32).Value = quantity;

                    using (command.ExecuteReader())
                    {
                    }
                }
            }
            catch (Exception e)
            {
                throw new MissingDataConsistencyException(e.ToString());
            }
            finally
            {
                _connection.Close();
            }
        }


        /** Buy products in the shop, decreases quantities
         * @param shopId Shop where products needed to buy
         * @param @param productsData array of (productName, quantity) tuples
         * @return true if operation successful, else false
         */
        public bool BuyProducts(int shopId, (string, int)[] productsData)
        {
            if (GetPurchaseTotal(shopId, productsData) < 0)
                return false;
            
            (int, int)[] productIdQuantity = new (int, int)[productsData.Length];
            for (int i = 0; i < productIdQuantity.Length; ++i)
            {
                productIdQuantity[i].Item1 = GetProductId(productsData[i].Item1);
                productIdQuantity[i].Item2 = productsData[i].Item2;
            }
            
            try
            {
                if (_connection.State != ConnectionState.Open)
                    _connection.Open();
                using (MySqlCommand command = _connection.CreateCommand())
                {
                    foreach (var tuple in productIdQuantity)
                    {
                        int productId = tuple.Item1;
                        int quantityToBuy = tuple.Item2;
                        
                        command.CommandText =
                            @"UPDATE ShopProduct SET Quantity =  Quantity - @quantity WHERE ShopID = @shopID AND ProductID = @productId";
                        command.Parameters.Add("@shopID", MySqlDbType.Int32).Value = shopId;
                        command.Parameters.Add("@productID", MySqlDbType.Int32).Value = productId;
                        command.Parameters.Add("@quantity", MySqlDbType.Int32).Value = quantityToBuy;

                        using (command.ExecuteReader())
                        {
                        }
                    }
                    
                }
            }
            catch (Exception e)
            {
                throw new MissingDataConsistencyException(e.ToString());
            }
            finally
            {
                _connection.Close();
            }

            return true;
        }

        private readonly MySqlConnection _connection;

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