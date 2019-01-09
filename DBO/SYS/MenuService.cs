using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Core.Utility;
using DAO.SYS;
using Models.DataModel;
using System.Data;

namespace DBO.SYS
{
    public class MenuService
    {
        MenuAdapter<Menu, SqlDbHelper> _MenuAdapter;

        public MenuService()
        {
            _MenuAdapter = new MenuAdapter<Menu, SqlDbHelper>();
        }

        /// <summary>
        /// 取得網站功能清單
        /// </summary>
        /// <param name="P_mon_vendor"></param>
        /// <returns></returns>
        public DataTable queryMenu()
        {
            return _MenuAdapter.GetAllMenuTree();
        }
    }
}
