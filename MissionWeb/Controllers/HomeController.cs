using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;


namespace MissionWeb.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Login()
        {
            ViewBag.Title = "Mission Login Page";
            ViewBag.Message = "請登入";
            return View();
        }

        public ActionResult Index()
        {
            PageInfo();
            return View();
        }

        public ActionResult About()
        {
            PageInfo();
            ViewBag.Message = "Your application description page.";
            return View();
        }

        public ActionResult Contact()
        {
            PageInfo();
            ViewBag.Message = "Your contact page.";
            return View();
        }
    }
}