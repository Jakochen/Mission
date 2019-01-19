using Core.Utility;
using Models.WebModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MissionWeb.Controllers
{
    public class BaseController : Controller
    {
        /// <summary>
        /// 頁面共用程式碼
        /// </summary>
        public void PageInfo(ref VmInfobar vmToolbar)
        {
            vmToolbar = new VmInfobar();

            #region LoginStatus
            ViewData.Add("Logon", Session["Logon"]);
            #endregion

            #region MenuData
            ViewBag.DOM_TreeViewMenu = Session["MenuHtml"];
            ViewBag.DOM_TreeViewMenuScript = Session["MenuScript"];
            #endregion

            #region InfoBar
            vmToolbar.UserName = Session["UserName"].ToString();
            vmToolbar.dateTimeNow = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            #endregion
        }
    }
}