using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DB
{
    public class DBHelper
    {
        #region Declaration  
        static SqlConnection connection;
        DataTable dt;
        #endregion
        public void OpenSqlConnection()
        {
            string connectionString = GetConnectionString();

            connection = new SqlConnection();
            connection.ConnectionString = connectionString;

            connection.Open();

            Console.WriteLine("State: {0}", connection.State);
            Console.WriteLine("ConnectionString: {0}",
                connection.ConnectionString);

        }


        public void CloseSqlConnection()
        {
            connection.Close();
        }
        private static string GetConnectionString() =>
            // To avoid storing the connection string in your code, 
            // you can retrieve it from a configuration file.
            //return @"Server=.\SQLEXPRESS;Database=arduinowater;"
            //    + "User Id = sa; Password =1qaz@WSX";
            System.Configuration.ConfigurationSettings.AppSettings["ConStr"];

        public  SqlDataReader GetSqlDataReader(string acc, string pw)
        {
            string queryString = "Select * from ArduinoWater_User where UserName=@acc and Password = @pw";
            SqlCommand command = new SqlCommand(queryString, connection);
            command.Parameters.AddWithValue("@acc", acc);
            command.Parameters.AddWithValue("@pw", pw);

            SqlDataReader sqlDataReader = command.ExecuteReader();
            return sqlDataReader;
        }

        public int UpdateTable(string acc, string pw)
        {
            string SqlStr = "Update ArduinoWater_User Set IsLogon = 1, LastLogonTime=SYSDATETIME() where UserName=@acc and Password=@pw";
            SqlCommand command = new SqlCommand(SqlStr, connection);
            command.Parameters.AddWithValue("@acc", acc);
            command.Parameters.AddWithValue("@pw", pw);

            return command.ExecuteNonQuery();
        }

        #region KeyMethods  
        public DataTable GetDataTableFromDb(string query)
        {
            SqlDataAdapter Adpt = new SqlDataAdapter(query, connection);
            dt = new DataTable();
            try
            {
                Adpt.Fill(dt);
            }
            catch (SqlException ex)
            {
                dt = null;
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }
            finally
            {
                if (connection != null)
                    if (connection.State == ConnectionState.Open) connection.Close();
                Adpt.Dispose();
            }
            return dt;
        }
        #endregion

        public DataTable GetAllCategories()
        {
            return GetDataTableFromDb("SELECT MenuNumber, ParentNumber, MenuName, Uri, Icon FROM Menu");
        }
    }
}
