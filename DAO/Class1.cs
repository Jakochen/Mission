using Models.DataModel;
using Models.WebModel;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace DAO
{
    public class DataRepository : IDisposable
    {
        private readonly IDbConnection dbConnection;

        public DataRepository(string connectionString)
        {
            dbConnection = new SqlConnection(connectionString);
            dbConnection.Open();
        }

        /// <summary>
        /// get user token(待實作)
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public string GetToken(string userId)
        {
            string result = string.Empty;
            //  Dapper 使用示意
            //string sql = @"
            //SELECT UserToken 
            //FROM User_Token
            //WHERE UserId=@UserId;";
            //try
            //{
            //    result = dbConnection.QueryFirstOrDefault<string>(sql, new
            //    {
            //        UserId = userId
            //    });
            //}
            //catch (Exception ex)
            //{
            //    result = null;
            //}
            return result;
        }

        //public List<Product> GetProductByStore(ProductQuery query)
        //{
        //    List<Product> result = new List<Product>();
        //    //  Dapper 使用示意
        //    string sql = @"
        //    SELECT productId, productName, productPrice 
        //    FROM ProductMain
        //    WHERE storeId=@storeId ";
        //    try
        //    {
        //        if (query != null)
        //        {
        //            if (query.chkValid.HasValue)
        //            {
        //                sql += " and isEnabled=@isEnabled";
        //            }
        //            // 參數可以多給, SQL句沒有的條件就不會套用
        //            result = dbConnection.Query<Product>(sql, new
        //            {
        //                storeId = query.storeNo,
        //                isEnabled = (query.chkValid.HasValue ? query.chkValid.Value : false)
        //            }).ToList();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        result = new List<Product>();
        //    }
        //    return result;
        //}

        /// <summary>
        /// for IDispose
        /// </summary>
        public void Dispose()
        {
            try
            {
                if (dbConnection != null)
                    dbConnection.Close();
            }
            catch (Exception ex)
            {

            }
        }
    }
}
