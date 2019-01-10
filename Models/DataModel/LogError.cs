using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Models.DataModel
{
    public class LogError : Exception
    {
        public string ErrorId { get; set; }

        public int StatusCode { get; set; }

        public LogError()
            : this("呼叫錯誤")
        {
        }

        public LogError(string errorMessage)
            : base(errorMessage)
        {
            ErrorId = "unknown_error";
            StatusCode = (int)HttpStatusCode.BadRequest;
        }

    }
}
