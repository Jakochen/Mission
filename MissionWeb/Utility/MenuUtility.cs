using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DBO.SYS;
using Models.DataModel;
using DataTableExtensions = Core.Utility.DataTableExtensions;

namespace MissionWeb.Utility
{
    public class MenuUtility
    {
        MenuService menuService = new MenuService();
        int childcnt = 1; //子選單計數
        #region 取得 MENU
        private string PopulateMenuDataTable()
        {
            string DOM = "";
            DataTable dt = new DataTable();
            dt = menuService.queryMenu();
            List<Menu> menus = DataTableExtensions.ToList<Menu>(dt).ToList();
            DOM = GetDOMTreeView(dt);

            return DOM;
        }

        public string PopulateMenuScript()
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


        public string GetMenu()
        {
            string MenuHtmlStr = "";
            MenuHtmlStr = PopulateMenuDataTable();
            MenuHtmlStr += "</ul>";
            return MenuHtmlStr;
        }
        public string GetMenuScript()
        {
            string MenuScriptStr = "";
            MenuScriptStr = PopulateMenuScript();
            return MenuScriptStr;
        }
        #endregion
    }
}
