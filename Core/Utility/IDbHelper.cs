using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utility
{
    public delegate bool ReadDataCallBack<T>(List<T> entityList, IDataReader reader, int number);

    /// <summary>
    /// 資料庫存取輔助功能
    /// </summary>
    public interface IDbHelper
    {

        #region Common
        IDbConnection GetConncection();
        IDataParameter InputParameter(string name, object value);

        #endregion Common

        #region GetDataTable

        DataTable GetDataTable(String sql, IDbConnection conn = null);
        DataTable GetDataTable(String sql, List<IDataParameter> parameters, IDbConnection conn = null);
        DataTable GetDataTableSp(String storeProcedureName, IDbConnection conn = null);
        DataTable GetDataTableSp(String storeProcedureName, List<IDataParameter> parameters, IDbConnection conn = null);
        DataTable GetDataTable(IDbCommand command);

        #endregion

        #region ExecuteSQL

        int ExecuteSQL(String sql, IDbConnection conn = null);
        int ExecuteSQL(String sql, List<IDataParameter> parameters, IDbConnection conn = null);
        int ExecuteSp(String sql, IDbConnection conn = null);
        int ExecuteSp(String sql, List<IDataParameter> parameters, IDbConnection conn = null);
        int ExecuteSQL(IDbCommand command);

        #endregion

        #region ExecuteScalar

        object ExecuteScalar(String sql, IDbConnection conn = null);
        object ExecuteScalar(String sql, List<IDataParameter> parameters, IDbConnection conn = null);

        string ExecuteScalarString(String sql, IDbConnection conn = null);
        string ExecuteScalarString(String sql, List<IDataParameter> parameters, IDbConnection conn = null);

        int ExecuteScalarInt(String sql, IDbConnection conn = null);
        int ExecuteScalarInt(String sql, List<IDataParameter> parameters, IDbConnection conn = null);

        decimal ExecuteScalarDecimal(String sql, IDbConnection conn = null);
        decimal ExecuteScalarDecimal(String sql, List<IDataParameter> parameters, IDbConnection conn = null);

        DateTime ExecuteScalarDateTime(String sql, IDbConnection conn = null);
        DateTime ExecuteScalarDateTime(String sql, List<IDataParameter> parameters, IDbConnection conn = null);


        #endregion

        #region GetDataReader

        IDataReader GetDataReader(String sql, IDbConnection conn);
        IDataReader GetDataReader(String sql, List<IDataParameter> parameters, IDbConnection conn);
        IDataReader GetDataReader(IDbCommand command);

        #endregion

        #region ExecuteReader With CallBack

        int ExecuteReader<T>(String sql, List<T> entityList, ReadDataCallBack<T> ReadFuncCallBack, IDbConnection conn = null);
        int ExecuteReader<T>(String sql, List<IDataParameter> parameters, List<T> entityList, ReadDataCallBack<T> ReadFuncCallBack, IDbConnection conn = null);
        int ExecuteReader<T>(IDbCommand command, List<T> entityList, ReadDataCallBack<T> ReadFuncCallBack);

        #endregion

        #region Paging (DataTable, DataReader)

        DataTable GetDataTable(String sql, Pageable pageable, IDbConnection conn = null);
        DataTable GetDataTable(String sql, List<IDataParameter> parameters, Pageable pageable, IDbConnection conn = null);

        IDataReader GetDataReader(String sql, Pageable pageable, IDbConnection conn);
        IDataReader GetDataReader(String sql, List<IDataParameter> parameters, Pageable pageable, IDbConnection conn);

        int ExecuteReader<T>(String sql, Pageable pageable, List<T> entityList, ReadDataCallBack<T> ReadFuncCallBack, IDbConnection conn = null);
        int ExecuteReader<T>(String sql, List<IDataParameter> parameters, Pageable pageable, List<T> entityList, ReadDataCallBack<T> ReadFuncCallBack, IDbConnection conn = null);

        #endregion

        #region GetDynamicList
        List<dynamic> GetDynamicList(String sql, IDbConnection conn = null);
        List<dynamic> GetDynamicList(String sql, List<IDataParameter> parameters = null, IDbConnection conn = null);
        List<dynamic> GetDynamicList(IDbCommand command);
        #endregion
    }
}
