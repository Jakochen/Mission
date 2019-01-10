using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Models.DataModel
{
    public class LogInfo
    {
        public int StatusCode { get; set; }

        public string Controller { get; set; }

        public string Action { get; set; }
        public string IP { get; set; }
        public DateTime DateTime { get; set; }
        public string RequestData { get; set; }
        public  string Error { get; set; }
    }
}
