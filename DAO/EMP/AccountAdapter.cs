using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Core.Utility;
using Models.DataModel;
using DataTableExtensions = Core.Utility.DataTableExtensions;

namespace DAO.EMP
{
    /// <summary>
    /// 使用 Table Name : Employees
    /// </summary>
    public class AccountAdapter<T, TDb> 
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

        public DataTable IsLogonUser(string username)
        {
            string sql = @"select IsLogon from Employees where UserName = @username";
            List<IDataParameter> parameters = new List<IDataParameter>();
            parameters.Add(_IdbHelp.InputParameter("username", username));

            return _IdbHelp.GetDataTable(sql, parameters);
        }

        public Employees Login(string act, string pw)
        {
            Employees emp = new Employees();
            string sql = @"Select * from Employees where UserName=@acc and Password = @pw";

            List<IDataParameter> parameters = new List<IDataParameter>();
            parameters.Add(_IdbHelp.InputParameter("acc", act));
            parameters.Add(_IdbHelp.InputParameter("pw", pw));
            DataTable dt = _IdbHelp.GetDataTable(sql, parameters);
            if (dt.Rows.Count > 0)
            {
                return DataTableExtensions.ToList<Employees>(dt).ToList()[0];
            }
            else
            {
                return null;
            }
        }

        public int UpdateAccountLoginState(string acc, string pw, string LogonState)
        {
            string sql = @"Update Employees Set IsLogon = @LogonState, LastLogonTime=SYSDATETIME() where UserName=@acc and Password=@pw";

            List<IDataParameter> parameters = new List<IDataParameter>();

            parameters.Add(_IdbHelp.InputParameter("acc", acc));
            parameters.Add(_IdbHelp.InputParameter("pw", pw));
            parameters.Add(_IdbHelp.InputParameter("LogonState", LogonState));

            return _IdbHelp.ExecuteSQL(sql, parameters);
        }
    }
}
