using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Models.DataModel;
using System.Data;
using Core.Utility;
using DAO.EMP;

namespace DBO.EMP
{
    public class AccountService
    {
        AccountAdapter<Employees, SqlDbHelper> _AccountAdapter;

        public AccountService()
        {
            _AccountAdapter = new AccountAdapter<Employees, SqlDbHelper>();
        }

        /// <summary>
        /// 取得單一登入
        /// </summary>
        /// <param name="acc"></param>
        /// <param name="pw"></param>
        /// <returns></returns>
        public Employees GetSingleAccount(string acc, string pw)
        {
            Employees user = new Employees();
            user = _AccountAdapter.Login(acc, pw);
            return user;
        }

        /// <summary>
        /// 變更登入使用者登入狀態
        /// </summary>
        /// <param name="acc"></param>
        /// <param name="pw"></param>
        /// <param name="State"></param>
        /// <returns></returns>
        public int UpdateAccountLoginState(string acc, string pw,string State)
        {
            int i_isupdate = _AccountAdapter.UpdateAccountLoginState(acc, pw, State);
            return i_isupdate;
        }

        /// <summary>
        /// 判斷使用者是否已登入
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public string IsLogon(string username)
        {
            //判斷使用者是否已登入，以便許可載入選單
            return _AccountAdapter.IsLogonUser(username).Rows[0]["IsLogon"].ToString();
        }
    }
}
