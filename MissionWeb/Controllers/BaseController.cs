using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MissionWeb.Controllers
{
    public class BaseController : Controller
    {
        public void PageInfo()
        {
            ViewData.Add("Logon", Session["Logon"]);
            ViewBag.DOM_TreeViewMenu = Session["MenuHtml"];
            ViewBag.DOM_TreeViewMenuScript = Session["MenuScript"];
        }
    }
}