using System;
using System.Data;
using Geometry;
using MySql.Data.MySqlClient;

namespace TriangleDBSerializer
{
    public class MariaDBSerializer
    {
        public static bool CheckConnection(string connectionString)
        {
            var connection = new MySqlConnection(connectionString);
            var successFlag = true;
            
            try
            {
                connection.Open();
            }
            catch (MySqlException)
            {
                successFlag = false;
            }
            finally
            {
                connection.Close();
            }

            return successFlag;
        }
        
        public MariaDBSerializer(string connectionString)
        {
            _connectionSting = connectionString;
        }

        public void ResetConnection(string newConnectionString)
        {
            _connectionSting = newConnectionString;
        }

        public bool IsExistsInDatabase(int key)
        {
            bool result = false;
            var connection = new MySqlConnection(_connectionSting);
            try
            {
                connection.Open();
                using (MySqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = @"SELECT * FROM Triangle WHERE TriangleID = @triangleID";
                    command.Parameters.Add("@triangleID", MySqlDbType.Int32).Value = key;

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        result = reader.HasRows;
                    }
                }
            }
            catch (Exception)
            {
                result = false;
            }
            finally
            {
                connection.Close();
            }

            return result;
        }

        public bool Serialize(Triangle t, int key)
        {
            
            if (IsExistsInDatabase(key))
                return false;

            bool result = true;
            var connection = new MySqlConnection(_connectionSting);
            try
            {
                connection.Open();
                using (MySqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = @"INSERT INTO Triangle VALUE (@triangleID, @x1, @y1, @x2, @y2, @x3, @y3)";
                    command.Parameters.Add("@triangleID", MySqlDbType.Int32).Value = key;
                    command.Parameters.Add("@x1", MySqlDbType.Double).Value = t.A.X;
                    command.Parameters.Add("@y1", MySqlDbType.Double).Value = t.A.Y;
                    command.Parameters.Add("@x2", MySqlDbType.Double).Value = t.B.X;
                    command.Parameters.Add("@y2", MySqlDbType.Double).Value = t.B.Y;                    
                    command.Parameters.Add("@x3", MySqlDbType.Double).Value = t.C.X;
                    command.Parameters.Add("@y3", MySqlDbType.Double).Value = t.C.Y;
                    using (command.ExecuteReader())
                    {
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                result = false;
            }
            finally
            {
                connection.Close();
            }

            return result;
        }

        public Triangle Deserialize(int key)
        {
            if (!IsExistsInDatabase(key))
                return null;
            
            var connection = new MySqlConnection(_connectionSting);
            Triangle result = null;
            
            try
            {
                connection.Open();
                using (MySqlCommand command = connection.CreateCommand())
                {
                    command.CommandText =
                        @"SELECT X1, Y1, X2, Y2, X3, Y3 FROM Triangle WHERE TriangleID = @triangleID";
                    command.Parameters.Add("@triangleID", MySqlDbType.Int32);
                    command.Parameters["@triangleID"].Value = key;

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();
                            var x1 = double.Parse(reader[0].ToString());
                            var y1 = double.Parse(reader[1].ToString());
                            var x2 = double.Parse(reader[2].ToString());
                            var y2 = double.Parse(reader[3].ToString());
                            var x3 = double.Parse(reader[4].ToString());
                            var y3 = double.Parse(reader[5].ToString());
                            result = new Triangle(x1, y1, x2, y2, x3, y3);
                        }
                    }
                }
            }
            catch (Exception)
            {
                result = null;
            }
            finally
            {
                connection.Close();
            }

            return result;
        }

        public bool RemoveIfExists(int key)
        {
            if (!IsExistsInDatabase(key))
                return false;
            
            var result = true;
            var connection = new MySqlConnection(_connectionSting);
            try
            {
                connection.Open();
                using (MySqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = @"DELETE FROM Triangle WHERE TriangleID = @triangleID";
                    command.Parameters.Add("@triangleID", MySqlDbType.Int32).Value = key;
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception)
            {
                result = false;
            }
            finally
            {
                connection.Close();
            }

            return result;
        }

        private string _connectionSting;
        
        
    }
}