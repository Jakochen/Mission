using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Models.DataModel
{
    public class LogExtError:LogError
    {
        public LogExtError()
    : base("不存在")
        {
            ErrorId = "doesnt_exist";
            StatusCode = (int)HttpStatusCode.NotFound;
        }
    }
}
