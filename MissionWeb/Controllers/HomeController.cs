using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Core.DB;
using BizService;

namespace MissionWeb.Controllers
{
    public class HomeController : Controller
    {
        int childcnt = 1; //子選單計數
        DBHelper dB = new DBHelper();//DB 操作
        public ActionResult Login()
        {
            ViewBag.Message = "請登入";
            return View();
        }

        public ActionResult Index()
        {
            ViewData.Add("Logon", IsLogon());
            ViewBag.DOM_TreeViewMenu = Session["MenuHtml"];
            ViewBag.DOM_TreeViewMenuScript = Session["MenuScript"];
            return View();
        }

        public ActionResult About()
        {
            ViewData.Add("Logon", IsLogon());
            ViewBag.DOM_TreeViewMenu = Session["MenuHtml"];
            ViewBag.DOM_TreeViewMenuScript = Session["MenuScript"];

            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewData.Add("Logon", IsLogon());
            ViewBag.DOM_TreeViewMenu = Session["MenuHtml"];
            ViewBag.DOM_TreeViewMenuScript = Session["MenuScript"];
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Login(string username, string Password)
        {
            Account account = new Account();
            // 登入的密碼（以 SHA1 加密）
            Password = FormsAuthentication.HashPasswordForStoringInConfigFile(Password, "SHA1");

            //這一條是去資料庫抓取輸入的帳號密碼的方法請自行實做
            var LoginData = account.GetSingleAccount(username, Password);

            if (LoginData == null)
            {
                TempData["Error"] = "您輸入的帳號不存在或者密碼錯誤!";
                return View();
            }
            else
            {
                account.UpdateAccount(username, Password);
            }
            // 登入時清空所有 Session 資料
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

            Session["UserName"] = username;
            Session["MenuHtml"] = GetMenu();
            Session["MenuScript"] = GetMenuScript();
            return RedirectToAction("Index", "Home");

        }

        private string IsLogon()
        {
            //判斷使用者是否已登入，以便許可載入選單
            DataTable dt = new DataTable();
            dt = dB.GetDataTableFromDb("select IsLogon from ArduinoWater_User where UserName = '" + Session["UserName"].ToString() + "'");
            return dt.Rows[0]["IsLogon"].ToString();
        }

        #region 取得 MENU
        private string PopulateMenuDataTable()
        {
            string DOM = "";
            DataTable dt = new DataTable();
            dt = dB.GetAllCategories();
            DOM = GetDOMTreeView(dt);

            return DOM;
        }

        private string PopulateMenuScript()
        {
            string DOMScript =
                "//MENU OPEN/CLOSE\n" +
                "$('.nav-toggle').click(function(e) {" +
                "e.preventDefault();" +
                "$(\"html\").toggleClass(\"openNav\");" +
                "$(\".nav-toggle\").toggleClass(\"active\");" +
                "});";

            for (int i = 1; i <= childcnt; i++)
            {
                DOMScript += "var amenuhover" + i + " = 0;\n";
                DOMScript += "$('.amenu-" + i + "').mouseover(function() {" +
                "if (amenuhover" + i + " == 0)" +
                "   {" +
                        "$('.amenu-" + i + " + span.icon + label + input.sub-menu-checkbox').click();" +
                "       amenuhover" + i + "++;" +
                    "}" +
                "});" +
            "$('.amenu-" + i + "').mouseout(function() {" +
                "amenuhover" + i + " = 0;" +
            "});";
            }
            return DOMScript;
        }

        private string GetDOMTreeView(DataTable dt)
        {
            string DOMTreeView = "";

            DOMTreeView += CreateTreeViewOuterParent(dt);
            DOMTreeView += CreateTreeViewMenu(dt, "0");
            DOMTreeView += "</ul>";

            return DOMTreeView;
        }

        /// <summary>
        /// 取得根目錄
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private string CreateTreeViewOuterParent(DataTable dt)
        {
            string DOMDataList = "<ul class='menu-dropdown'>";

            DataRow[] drs = dt.Select("MenuNumber = 0");

            foreach (DataRow row in drs)
            {
                DOMDataList += "<li><a href='" + row[3].ToString() + "'>" + row[2].ToString() + "</a><span class='icon'><i class='" + row[4].ToString() + "'></i></span></li>";
            }

            return DOMDataList;
        }

        /// <summary>
        /// 取得子目錄
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="ParentNumber"></param>
        /// <returns></returns>
        private string CreateTreeViewMenu(DataTable dt, string ParentNumber)
        {
            string DOMDataList = "";
            string menuNumber = "";//母選單(根)目錄
            string menuName = "";//選單項目名稱
            string uri = "";//選單連結網址
            string icon = "";//選單icon (class)

            DataRow[] drs = dt.Select("ParentNumber = " + ParentNumber);//選擇屬於此根目錄之子項

            foreach (DataRow row in drs)
            {
                menuNumber = row[0].ToString();
                menuName = row[2].ToString();
                uri = row[3].ToString();
                icon = row[4].ToString();

                DataRow[] drschild = dt.Select("ParentNumber = " + menuNumber);//取得屬於此子項之選單

                if (drschild.Count() != 0)
                {
                    string SubmenuNumber = "";
                    string SubmenuName = "";
                    string Suburi = "";
                    string Subicon = "";

                    DOMDataList += "<li class='menu-hasdropdown'>";
                    DOMDataList += "<a class='amenu-" + childcnt + "' href='" + uri + "'>" + menuName + "</a><span class='icon'><i class='" + icon + "'></i></span>" +
                                            "<label title = 'toggle menu' for= '" + menuName + "'>" +
                                            //"<span class='downarrow'><i class='fa fa-caret-down'></i></span>" +
                                            "</label>" +
                                            "<input type = 'checkbox' class='sub-menu-checkbox' id='" + menuName + "' /><ul class='sub-menu-dropdown'>";

                    foreach (DataRow Subrow in drschild)
                    {
                        SubmenuNumber = Subrow[0].ToString();
                        SubmenuName = Subrow[2].ToString();
                        Suburi = Subrow[3].ToString();
                        Subicon = Subrow[4].ToString();
                        DOMDataList += "<li><a href = '" + Suburi + "' >" + SubmenuName + "</a></li>";

                        //DOMDataList += CreateTreeViewMenu(dt, SubmenuNumber);
                    }

                    DOMDataList += "</ul></li>";
                    childcnt++;
                }
                else
                {
                    DOMDataList += "<li><a href='" + uri + "'>" + menuName + "</a><span class='icon'><i class='" + icon + "'></i></span></li>";
                }
            }

            return DOMDataList;
        }


        private string GetMenu()
        {
            string MenuHtmlStr = "";
            MenuHtmlStr = PopulateMenuDataTable();
            MenuHtmlStr += "</ul>";
            return MenuHtmlStr;
        }
        private string GetMenuScript()
        {
            string MenuScriptStr = "";
            MenuScriptStr = PopulateMenuScript();
            return MenuScriptStr;
        }
        #endregion
    }


}