using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Models.DataModel;
using NLog;

namespace MissionWeb.Filter
{
    /// <summary>
    /// 使每個 action 皆可記錄動作資訊
    /// </summary>
    public class LogFilterAttribute : ActionFilterAttribute, IActionFilter
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();
        /// <summary>
        /// 在Action執行之後
        /// </summary>
        /// <param name="actionExecutedContext">Action執行後，產出的結果類別</param>
        void IActionFilter.OnActionExecuted(ActionExecutedContext actionExecutedContext)
        {
            base.OnActionExecuted(actionExecutedContext);

            string message = "";
            if (actionExecutedContext.Result is ViewResult view)
            {
                if (view.TempData["Error"] != null)
                {
                    message = view.TempData["Error"].ToString();

                    LogInfo log = new LogInfo()
                    {
                        StatusCode = actionExecutedContext.HttpContext.Response.StatusCode,
                        Controller = actionExecutedContext.ActionDescriptor.ControllerDescriptor.ControllerName,
                        Action = string.Concat(actionExecutedContext.ActionDescriptor.ActionName, " (Logged By: Custom Action Filter)"),
                        IP = actionExecutedContext.HttpContext.Request.UserHostAddress,
                        DateTime = actionExecutedContext.HttpContext.Timestamp,
                        RequestData = null,
                        Error = message
                    };

                    _logger.Info("{" + log.StatusCode + "," + log.Controller + "," + log.Action + "," + log.IP + "," + log.DateTime.ToLongTimeString() + "," + log.RequestData + "," + log.Error + "}");
                }
            }
        }
   
        /// <summary>
        /// 在Action執行之前
        /// </summary>
        /// <param name="actionContext">呼叫時所帶的Request內容</param>
        void IActionFilter.OnActionExecuting(ActionExecutingContext filterContext)
        {
            #region 取得 POST 資料
            string RequestData = "";
            if(filterContext.ActionParameters.Count>0)
            {
                int i = 1;
                RequestData += "RequestData: {";
                foreach(var temp in filterContext.ActionParameters)
                {
                    RequestData += " Key: " + temp.Key.ToString();
                    RequestData += " Value: " + temp.Value.ToString();
                    if(i == filterContext.ActionParameters.Count)
                    {
                        break;
                    }
                    else
                    {
                        RequestData += " | ";
                        i++;
                    }
                }
                RequestData += " }";
            }
            #endregion

            LogInfo log = new LogInfo()
            {
                StatusCode = filterContext.HttpContext.Response.StatusCode,
                Controller = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName,
                Action = string.Concat(filterContext.ActionDescriptor.ActionName, " (Logged By: Custom Action Filter)"),
                IP = filterContext.HttpContext.Request.UserHostAddress,
                DateTime = filterContext.HttpContext.Timestamp,
                RequestData = RequestData,
                Error = null
            };

            _logger.Info("{" + log.StatusCode + "," + log.Controller + "," + log.Action + "," + log.IP + "," + log.DateTime.ToLongTimeString() + "," + log.RequestData + "," + log.Error + "}");
        }
    }
}
