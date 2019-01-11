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
        public static string ConnectString { get { return GetConnectString(); } }

        private static string GetConnectString() => ConfigurationManager.AppSettings["ConStr"];
    }
}
