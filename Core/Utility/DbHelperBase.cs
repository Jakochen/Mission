using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utility
{
    /// <summary>
    /// 資料庫存取輔助功能
    /// </summary>
    public abstract class DbHelperBase : IDbHelper
    {

        #region Common
        public abstract IDbConnection GetConncection();

        public abstract IDataParameter InputParameter(string name, object value);

        protected abstract IDbCommand GetCommand(string sql, List<IDataParameter> parameters, IDbConnection conn, CommandType commandType);

        #endregion Common

        #region GetDataTable

        /// <summary>
        /// Get DataTable by sql
        /// <para>若conn=null , 則會自動open and close connection</para>
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="conn"></param>
        /// <returns></returns>
        public DataTable GetDataTable(String sql, IDbConnection conn = null)
        {
            return GetDataTable(sql, (List<IDataParameter>)null, conn);
        }

        /// <summary>
        /// Get DataTable by store procedure / function
        /// <para>1. 若conn=null , 則會自動open and close connection</para>
        /// <para>2. 當store procedure 發生錯誤時，不一定成功傳回cursor, 程式應對不同狀況或其它傳回值進行判斷</para>
        /// </summary>
        /// <param name="storeProcedureName"></param>
        /// <param name="conn"></param>
        /// <returns></returns>
        public DataTable GetDataTableSp(String storeProcedureName, IDbConnection conn = null)
        {
            return GetDataTableSp(storeProcedureName, null, conn);
        }

        /// <summary>
        /// Get DataTable by sql
        /// <para>若conn=null , 則會自動open and close connection</para>
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <param name="conn"></param>
        /// <returns></returns>
        public DataTable GetDataTable(String sql, List<IDataParameter> parameters, IDbConnection conn = null)
        {
            IDbCommand command = GetCommand(sql, parameters, conn, CommandType.Text);
            return GetDataTable(command);
        }

        /// <summary>
        /// Get DataTable by store procedure / function
        /// <para>1. 若conn=null , 則會自動open and close connection</para>
        /// <para>2. 當store procedure 發生錯誤時，不一定成功傳回cursor, 程式應對不同狀況或其它傳回值進行判斷</para>
        /// </summary>
        /// <param name="storeProcedureName"></param>
        /// <param name="parameters"></param>
        /// <param name="conn"></param>
        /// <returns></returns>
        public DataTable GetDataTableSp(String storeProcedureName, List<IDataParameter> parameters, IDbConnection conn = null)
        {
            IDbCommand command = GetCommand(storeProcedureName, parameters, conn, CommandType.StoredProcedure);
            return GetDataTable(command);
        }

        /// <summary>
        /// 若Command.Connection = null, 會自動open & close connection
        /// <para>(Note：command parameters is binded by Name)</para>
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public abstract DataTable GetDataTable(IDbCommand command);

        protected virtual DataTable GetDataTableByCommandType(String sql, List<IDataParameter> parameters, CommandType commandType, IDbConnection conn = null)
        {
            IDbCommand command = GetCommand(sql, parameters, conn, CommandType.Text);
            return GetDataTable(command);
        }

        #endregion

        #region ExecuteSQL

        /// <summary>
        /// Execurt Sql
        /// <para>若conn=null , 則會自動open and close connection</para>
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="conn"></param>
        /// <returns></returns>
        public int ExecuteSQL(String sql, IDbConnection conn = null)
        {
            return ExecuteSQL(sql, null, conn);
        }

        /// <summary>
        /// Execurt store procedure / function
        /// <para>若conn=null , 則會自動open and close connection</para>
        /// </summary>
        /// <param name="storeProcedureName"></param>
        /// <param name="conn"></param>
        /// <returns></returns>
        public int ExecuteSp(String storeProcedureName, IDbConnection conn = null)
        {
            return ExecuteSp(storeProcedureName, null, conn);
        }

        /// <summary>
        /// Execurt Sql with parameters
        /// <para>若conn=null , 則會自動open and close connection</para>
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <param name="conn"></param>
        /// <returns></returns>
        public int ExecuteSQL(String sql, List<IDataParameter> parameters, IDbConnection conn = null)
        {
            IDbCommand command = GetCommand(sql, parameters, conn, CommandType.Text);
            return ExecuteSQL(command);
        }

        /// <summary>
        /// Execurt store procedure / function
        /// <para>若conn=null , 則會自動open and close connection</para>
        /// </summary>
        /// <param name="storeProcedureName"></param>
        /// <param name="parameters"></param>
        /// <param name="conn"></param>
        /// <returns></returns>
        public int ExecuteSp(String storeProcedureName, List<IDataParameter> parameters, IDbConnection conn = null)
        {
            IDbCommand command = GetCommand(storeProcedureName, parameters, conn, CommandType.StoredProcedure);
            return ExecuteSQL(command);
        }

        public abstract int ExecuteSQL(IDbCommand command);

        #endregion

        #region ExecuteScalar

        /// <summary>
        /// ExecuteScalar
        /// <para>若conn=null , 則會自動open and close connection</para>
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="conn"></param>
        /// <returns></returns>
        public object ExecuteScalar(String sql, IDbConnection conn = null)
        {
            return ExecuteScalar(sql, null, conn);
        }


        /// <summary>
        /// ExecuteScalar TO STRING
        /// <para>若conn=null , 則會自動open and close connection</para>
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="conn"></param>
        /// <returns></returns>
        public string ExecuteScalarString(String sql, IDbConnection conn = null)
        {
            Object obj = ExecuteScalar(sql, conn);
            return (obj == null ? null : obj.ToString());
        }

        /// <summary>
        /// ExecuteScalar TO STRING
        /// <para>若conn=null , 則會自動open and close connection</para>
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <param name="conn"></param>
        /// <returns></returns>
        public string ExecuteScalarString(String sql, List<IDataParameter> parameters, IDbConnection conn = null)
        {
            Object obj = ExecuteScalar(sql, parameters, conn);
            return obj.ToString();
        }

        /// <summary>
        /// ExecuteScalar TO INT
        /// <para>若conn=null , 則會自動open and close connection</para>
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="conn"></param>
        /// <returns></returns>
        public int ExecuteScalarInt(String sql, IDbConnection conn = null)
        {
            return Convert.ToInt32(ExecuteScalar(sql, conn));
        }

        /// <summary>
        /// ExecuteScalar TO INT
        /// <para>若conn=null , 則會自動open and close connection</para>
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <param name="conn"></param>
        /// <returns></returns>
        public int ExecuteScalarInt(String sql, List<IDataParameter> parameters, IDbConnection conn = null)
        {
            return Convert.ToInt32(ExecuteScalar(sql, parameters, conn));
        }

        /// <summary>
        /// ExecuteScalar TO DECIMAL
        /// <para>若conn=null , 則會自動open and close connection</para>
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="conn"></param>
        /// <returns></returns>
        public decimal ExecuteScalarDecimal(String sql, IDbConnection conn = null)
        {
            return Convert.ToDecimal(ExecuteScalar(sql, conn));
        }

        /// <summary>
        /// ExecuteScalar TO DECIMAL
        /// <para>若conn=null , 則會自動open and close connection</para>
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <param name="conn"></param>
        /// <returns></returns>
        public decimal ExecuteScalarDecimal(String sql, List<IDataParameter> parameters, IDbConnection conn = null)
        {
            return Convert.ToDecimal(ExecuteScalar(sql, parameters, conn));
        }

        /// <summary>
        /// ExecuteScalar TO DATETIME
        /// <para>若conn=null , 則會自動open and close connection</para>
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="conn"></param>
        /// <returns></returns>
        public DateTime ExecuteScalarDateTime(String sql, IDbConnection conn = null)
        {
            return Convert.ToDateTime(ExecuteScalar(sql, conn));
        }

        /// <summary>
        /// ExecuteScalar TO DATETIME
        /// <para>若conn=null , 則會自動open and close connection</para>
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <param name="conn"></param>
        /// <returns></returns>
        public DateTime ExecuteScalarDateTime(String sql, List<IDataParameter> parameters, IDbConnection conn = null)
        {
            return Convert.ToDateTime(ExecuteScalar(sql, parameters, conn));
        }

        public abstract object ExecuteScalar(String sql, List<IDataParameter> parameters, IDbConnection conn = null);

        #endregion

        #region GetDataReader
        /// <summary>
        /// Get DataReader
        /// <para>** 必須自行傳入Connection</para>
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="conn"></param>
        /// <returns></returns>
        public IDataReader GetDataReader(String sql, IDbConnection conn)
        {
            return GetDataReader(sql, (List<IDataParameter>)null, conn);
        }

        /// <summary>
        /// Get DataReader With Parameter
        /// <para>** 必須自行傳入Connection</para>
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <param name="conn"></param>
        /// <returns></returns>
        public IDataReader GetDataReader(String sql, List<IDataParameter> parameters, IDbConnection conn)
        {
            IDbCommand command = GetCommand(sql, parameters, conn, CommandType.Text);
            return GetDataReader(command);
        }

        public abstract IDataReader GetDataReader(IDbCommand command);

        #endregion

        #region ExecuteReader With CallBack

        /// <summary>
        /// 將DataReader透過CallBack方式，轉至List
        /// <para>若conn=null , 則會自動open and close connection</para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="entityList"></param>
        /// <param name="ReadFuncCallBack"></param>
        /// <param name="conn"></param>
        /// <returns></returns>
        public int ExecuteReader<T>(String sql, List<T> entityList, ReadDataCallBack<T> ReadFuncCallBack, IDbConnection conn = null)
        {
            return ExecuteReader(sql, (List<IDataParameter>)null, entityList, ReadFuncCallBack, conn);
        }

        /// <summary>
        /// 將DataReader透過CallBack方式，轉至List
        /// <para>若conn=null , 則會自動open and close connection</para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <param name="entityList"></param>
        /// <param name="ReadFuncCallBack"></param>
        /// <param name="conn"></param>
        /// <returns></returns>
        public int ExecuteReader<T>(String sql, List<IDataParameter> parameters, List<T> entityList, ReadDataCallBack<T> ReadFuncCallBack, IDbConnection conn = null)
        {
            IDbCommand command = GetCommand(sql, parameters, conn, CommandType.Text);
            return ExecuteReader(command, entityList, ReadFuncCallBack);
        }

        public abstract int ExecuteReader<T>(IDbCommand command, List<T> entityList, ReadDataCallBack<T> ReadFuncCallBack);

        #endregion

        #region Paging (DataTable, DataReader)

        /// <summary>
        /// Get DataTable by sql
        /// <para>若conn=null , 則會自動open and close connection</para>
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="pageable"></param>
        /// <param name="conn"></param>
        /// <returns></returns>
        public DataTable GetDataTable(String sql, Pageable pageable, IDbConnection conn = null)
        {
            return GetDataTable(sql, null, pageable, conn);
        }

        /// <summary>
        /// Get DataTable by sql
        /// <para>若conn=null , 則會自動open and close connection</para>
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <param name="pageable"></param>
        /// <param name="conn"></param>
        /// <returns></returns>
        public DataTable GetDataTable(String sql, List<IDataParameter> parameters, Pageable pageable, IDbConnection conn = null)
        {
            string sqlPaging = GetPagingSql(sql, pageable);
            return GetDataTable(sqlPaging, parameters, conn);
        }

        /// <summary>
        /// Get DataReader
        /// <para>** 必須自行傳入Connection</para>
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="pageable"></param>
        /// <param name="conn"></param>
        /// <returns></returns>
        public IDataReader GetDataReader(String sql, Pageable pageable, IDbConnection conn)
        {
            return GetDataReader(sql, null, pageable, conn);
        }

        /// <summary>
        /// Get DataReader
        /// <para>** 必須自行傳入Connection</para>
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <param name="pageable"></param>
        /// <param name="conn"></param>
        /// <returns></returns>
        public IDataReader GetDataReader(String sql, List<IDataParameter> parameters, Pageable pageable, IDbConnection conn)
        {
            string sqlPaging = GetPagingSql(sql, pageable);
            return GetDataReader(sqlPaging, parameters, conn);
        }

        /// <summary>
        /// 將DataReader透過CallBack方式，轉至List
        /// <para>若conn=null , 則會自動open and close connection</para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="pageable"></param>
        /// <param name="entityList"></param>
        /// <param name="ReadFuncCallBack"></param>
        /// <param name="conn"></param>
        /// <returns></returns>
        public int ExecuteReader<T>(String sql, Pageable pageable, List<T> entityList, ReadDataCallBack<T> ReadFuncCallBack, IDbConnection conn = null)
        {
            return ExecuteReader(sql, null, pageable, entityList, ReadFuncCallBack, conn);
        }

        /// <summary>
        /// 將DataReader透過CallBack方式，轉至List
        /// <para>若conn=null , 則會自動open and close connection</para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <param name="pageable"></param>
        /// <param name="entityList"></param>
        /// <param name="ReadFuncCallBack"></param>
        /// <param name="conn"></param>
        /// <returns></returns>
        public int ExecuteReader<T>(String sql, List<IDataParameter> parameters, Pageable pageable, List<T> entityList, ReadDataCallBack<T> ReadFuncCallBack, IDbConnection conn = null)
        {
            string sqlPaging = GetPagingSql(sql, pageable, conn);
            return ExecuteReader(sql, parameters, entityList, ReadFuncCallBack, conn);
        }

        protected string GetPagingSql(string sql, Pageable pageable, IDbConnection conn = null)
        {
            pageable.TotalCount = GetTotalCount(sql, pageable, conn);
            return GetPageableSql(sql, pageable);
        }

        protected abstract int GetTotalCount(string sql, Pageable pageable, IDbConnection conn = null);
        protected abstract string GetPageableSql(string sql, Pageable pageable);

        #endregion

        #region GetDynamicList

        public List<dynamic> GetDynamicList(String sql, IDbConnection conn = null)
        {
            return GetDynamicList(sql, null, conn);
        }

        public List<dynamic> GetDynamicList(String sql, List<IDataParameter> parameters, IDbConnection conn = null)
        {
            IDbCommand command = GetCommand(sql, parameters, conn, CommandType.Text);
            return GetDynamicList(command);
        }

        public List<dynamic> GetDynamicList(IDbCommand command)
        {
            List<dynamic> list = new List<dynamic>();

            ExecuteReader(command, list, GetDynamicListCallBack);

            return list;
        }

        private bool GetDynamicListCallBack(List<dynamic> entityList, IDataReader reader, int number)
        {
            var expandoObject = new ExpandoObject() as IDictionary<string, object>;
            for (var i = 0; i < reader.FieldCount; i++)
            {
                Object value = (reader.IsDBNull(i)) ? null : reader[i];
                expandoObject.Add(reader.GetName(i), value);
            }
            entityList.Add(expandoObject);
            return false;
        }
        #endregion
    }
}
