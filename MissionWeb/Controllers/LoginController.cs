using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Web.Security;
using Core.Utility;
using DBO.EMP;
using MissionWeb.Utility;
using Models.DataModel;
using NLog;

namespace MissionWeb.Controllers
{
    public class LoginController : Controller
    {
        AccountService accountService = new AccountService();

        public ActionResult Login()
        {
            ViewBag.Title = "Mission Login Page";
            ViewBag.Message = "請登入";
            return View();
        }

        [HttpPost]
        public ActionResult Login(string username, string Password)
        {
            //logger.Info("User: " + username + " Login Start");//寫log
            // 登入的密碼（以 SHA1 加密）
            Password = Encryption.GetSwcSH1(Password);
            //取得登入帳密比對結果
            var LoginData = accountService.GetSingleAccount(username, Password);

            if (LoginData == null)
            {
                TempData["Error"] = "您輸入的帳號不存在或者密碼錯誤!";
                ViewBag.Title = "Mission Login Page";
                ViewBag.Message = "請登入";
                return View();
            }
            else
            {
                //登入成功/變更登入者狀態
                accountService.UpdateAccountLoginState(username, Password, "1");
            }
            // 登入成功後清空所有 Session 資料
            Session.RemoveAll();

            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1,
              LoginData.UserName,//你想要存放在 User.Identy.Name 的值，通常是使用者帳號
              DateTime.Now,
              DateTime.Now.AddMinutes(30),
              true,//將管理者登入的 Cookie 設定成 Session Cookie
              LoginData.Rank.ToString(),//userdata看你想存放啥
              FormsAuthentication.FormsCookiePath);

            string encTicket = FormsAuthentication.Encrypt(ticket);

            Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));
            MenuUtility menuUtility = new MenuUtility();
            
            //取得共用資訊
            Session["UserName"] = username;
            Session["Logon"] = accountService.IsLogon(Session["UserName"].ToString());
            Session["MenuHtml"] = menuUtility.GetMenu();
            Session["MenuScript"] = menuUtility.GetMenuScript();

            return RedirectToAction("Index", "Home");

        }

        public ActionResult Logout()
        {
            Session.RemoveAll();
            return RedirectToAction("Login", "Login");
        }
    }
}
