using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DB;

namespace BizService
{
    public class UserInfo
    {
        public string UserName;
        public string Rank;
    }

    public class Account
    {
        DBHelper dB = new DBHelper();
        public  UserInfo GetSingleAccount(string acc, string pw)
        {

            //資料庫連接
            dB.OpenSqlConnection();

            //取得資料
            UserInfo user = null;
            SqlDataReader sqlDataReader = dB.GetSqlDataReader(acc, pw);
            while (sqlDataReader.Read())
            {
                user = new UserInfo();
                user.UserName = sqlDataReader["UserName"].ToString();
                user.Rank = sqlDataReader["Rank"].ToString();
            }
            dB.CloseSqlConnection();
            return user;
        }

        public int UpdateAccount(string acc, string pw)
        {
            //資料庫連接
            dB.OpenSqlConnection();

            int i_isupdate = dB.UpdateTable(acc, pw);
            dB.CloseSqlConnection();
            return i_isupdate;
        }
    }
}
