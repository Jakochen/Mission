using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utility
{
    public class SqlDbInfo
    {


        public static string UserId { get { return "sa"; } }
        public static string Password { get { return "12qazWSX"; } }
        public static string DataSource { get { return "ERIC-PIC2"; } }
        public static string DataBase { get { return "WpfTest"; } }

        public static string ConnectString { get { return GetConnectString(); } }

        private static string GetConnectString() => ConfigurationManager.AppSettings["ConStr"];
        //{
        //    string connectString
        //        = "Server=" + DataSource + ";"
        //        + "Database=" + DataBase + ";"
        //        + "User Id=" + UserId + ";"
        //        + "Password=" + Password + ";";
        //    return connectString;
        //}


    }
}
