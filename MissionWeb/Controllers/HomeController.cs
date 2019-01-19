using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Xml;
using System.Xml.Linq;
using Models.WebModel;
using Core.Utility;

namespace MissionWeb.Controllers
{
    public class HomeController : BaseController
    {
        VmInfobar vmToolbar = new VmInfobar();
        public ActionResult Login()
        {
            ViewBag.Title = "Mission Login Page";
            ViewBag.Message = "請登入";
            return View();
        }

        public ActionResult Index()
        {
            PageInfo(ref vmToolbar);
            return View(vmToolbar);
        }

        public ActionResult About()
        {
            PageInfo(ref vmToolbar);
            ViewBag.Message = "Your application description page.";
            return View();
        }

        public ActionResult Contact()
        {
            PageInfo(ref vmToolbar);
            ViewBag.Message = "Your contact page.";
            return View();
        }

        public string GetinfobarDateTime()
        {
            return DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
        }

        public ActionResult SetCulture(string culture, string returnUrl)
        {
            // Validate input 
            culture = CultureHelper.GetImplementedCulture(culture);

            // Save culture in a cookie 
            HttpCookie cookie = Request.Cookies["_culture"];

            if (cookie != null)
            {
                // update cookie value 
                cookie.Value = culture;
            }
            else
            {
                // create cookie value 
                cookie = new HttpCookie("_culture");
                cookie.Value = culture;
                cookie.Expires = DateTime.Now.AddYears(1);
            }

            Response.Cookies.Add(cookie);
            return Redirect(returnUrl);
        }
    }
}