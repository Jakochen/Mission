using Models.DataModel;
using Models.WebModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Core.Utility;

namespace DAO.SYS
{
    /// <summary>
    /// 使用 Table Name : Menu
    /// </summary>
    public class MenuAdapter<T, TDb> 
        where T : class, new()
        where TDb : IDbHelper, new()
    {
        TDb db = new TDb();

        #region Get Connection
        private IDbConnection _conn;

        public IDbConnection Conn
        {
            get { return _conn; }
            set { _conn = value; }
        }
        #endregion

        IDbHelper _IdbHelp = new SqlDbHelper();

        #region MENU
        /// <summary>
        /// 取得網站功能清單MENU
        /// </summary>
        /// <returns></returns>
        public DataTable GetAllMenuTree()
        {
            string sql = @"SELECT MenuNumber, ParentNumber, MenuName, Uri, Icon FROM Menu";
            return _IdbHelp.GetDataTable(sql);
        }
        #endregion
    }
}
