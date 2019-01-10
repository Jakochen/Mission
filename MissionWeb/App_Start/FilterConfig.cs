using MissionWeb.Filter;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace MissionWeb
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new LogFilterAttribute());//增加全域屬性(記錄每個action log)
        }
    }
}
