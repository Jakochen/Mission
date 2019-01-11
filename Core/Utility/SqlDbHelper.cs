using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utility
{

    /// <summary>
    /// 資料庫存取輔助功能
    /// </summary>
    public class SqlDbHelper : DbHelperBase
    {

        #region Common
        /// <summary>
        /// 取得 Connection, 必須自行Close
        /// </summary>
        /// <returns></returns>
        public override IDbConnection GetConncection()
        {
            String connectionString = null;
            try
            {
                connectionString = SqlDbInfo.ConnectString;
            }
            catch (Exception)
            {
                throw new Exception(string.Format("找不到「ConnectionString」的設定"));
            }

            SqlConnection con = new SqlConnection();
            con.ConnectionString = connectionString;
            con.Open();

            return con;
        }

        /// <summary>
        /// Return a new Input Parameter
        /// <para>value 會考慮null值的狀況</para>
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override IDataParameter InputParameter(string name, object value)
        {
            SqlParameter p;
            if (value == null)
            {
                p = new SqlParameter(name, DBNull.Value);
            }
            else
            {
                p = new SqlParameter(name, value);
            }

            return p;
        }

        protected override IDbCommand GetCommand(string sql, List<IDataParameter> parameters, IDbConnection conn, CommandType commandType)
        {
            SqlCommand command = new SqlCommand(sql);
            command.CommandType = commandType;
            command.Connection = conn as SqlConnection;
            if (parameters != null)
            {
                foreach (SqlParameter parameter in parameters)
                {
                    command.Parameters.Add(parameter);
                }
            }
            return command;
        }

        #endregion Common

        #region Sql Parameters (for Sql)

        /// <summary>
        /// Return a new Input Parameter
        /// <para>value 會考慮null值的狀況</para>
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="oraType"></param>
        /// <returns></returns>
        public SqlParameter InputParameter(string name, object value, SqlDbType dbType)
        {
            SqlParameter p = ((SqlParameter)InputParameter(name, value));
            p.SqlDbType = dbType;

            return p;
        }

        /// <summary>
        /// Return a new Output Parameter
        /// </summary>
        /// <param name="name"></param>
        /// <param name="dbType"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public SqlParameter OutputParameter(string name, SqlDbType dbType, int size = 0)
        {
            if (size == 0 &&
                (dbType == SqlDbType.Char ||
                dbType == SqlDbType.NChar ||
                dbType == SqlDbType.VarChar ||
                dbType == SqlDbType.NVarChar))
            {
                size = 2000;
            }
            SqlParameter p = new SqlParameter(name, dbType, size);
            p.Direction = ParameterDirection.Output;

            return p;
        }

        /// <summary>
        /// Return a new ReturnValue Parameter
        /// </summary>
        /// <param name="orType"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public SqlParameter ReturnValueParameter(SqlDbType dbType, int size = 0)
        {
            if (size == 0 &&
                (dbType == SqlDbType.Char ||
                dbType == SqlDbType.NChar ||
                dbType == SqlDbType.VarChar ||
                dbType == SqlDbType.NVarChar))
            {
                size = 2000;
            }
            SqlParameter p = new SqlParameter();
            p.SqlDbType = dbType;
            p.Direction = ParameterDirection.ReturnValue;
            if (size > 0)
            {
                p.Size = size;
            }

            return p;
        }

        #endregion Sql Parameters

        #region GetDataTable

        /// <summary>
        /// 若Command.Connection = null, 會自動open & close connection
        /// <para>(Note：command parameters is binded by Name)</para>
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public override DataTable GetDataTable(IDbCommand command)
        {
            DataTable dt = new DataTable();

            SqlCommand SqlCommand = command as SqlCommand;
            if (SqlCommand == null)
            {
                return dt;
            }

            SqlConnection conn = SqlCommand.Connection;
            bool isConnection = conn != null;

            try
            {
                if (conn == null)
                {
                    conn = ((SqlConnection)GetConncection());
                    command.Connection = conn;
                }

                SqlDataAdapter adpter = new SqlDataAdapter(SqlCommand);
                adpter.Fill(dt);
                adpter.Dispose();


            }
            finally
            {
                if (!isConnection && conn != null)
                {
                    if (conn.State != ConnectionState.Closed)
                    {
                        conn.Close();
                    }
                    command.Connection = null;
                    conn.Dispose();
                }
            }

            return dt;
        }
        #endregion

        #region ExecuteSQL

        /// <summary>
        /// 若Command.Connection = null, 會自動open & close connection
        /// <para>(Note：command parameters is binded by Name)</para>
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public override int ExecuteSQL(IDbCommand command)
        {
            int returnCD = 0;
            SqlCommand SqlCommand = command as SqlCommand;
            if (SqlCommand == null)
            {
                return returnCD;
            }

            SqlConnection conn = SqlCommand.Connection;
            bool isConnection = conn != null;

            try
            {
                if (conn == null)
                {
                    conn = ((SqlConnection)GetConncection());
                    command.Connection = conn;
                }

                returnCD = command.ExecuteNonQuery();
            }
            finally
            {
                if (!isConnection && conn != null)
                {
                    if (conn.State != ConnectionState.Closed)
                    {
                        conn.Close();
                    }
                    command.Connection = null;
                    conn.Dispose();
                }
            }


            return returnCD;
        }
        #endregion

        #region ExecuteScalar

        /// <summary>
        /// ExecuteScalar with parameters
        /// <para>若conn=null , 則會自動open and close connection</para>
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <param name="conn"></param>
        /// <returns></returns>
        public override object ExecuteScalar(String sql, List<IDataParameter> parameters, IDbConnection conn = null)
        {
            Object result = null;
            IDbCommand command = GetCommand(sql, parameters, conn, CommandType.Text);

            bool isConnection = command.Connection != null;

            try
            {
                if (command.Connection == null)
                {
                    command.Connection = ((SqlConnection)GetConncection());
                }

                result = command.ExecuteScalar();
            }
            finally
            {
                if (!isConnection && command.Connection != null)
                {
                    if (command.Connection.State != ConnectionState.Closed)
                    {
                        command.Connection.Close();
                    }
                    command.Connection = null;
                }
            }

            return result;
        }


        #endregion

        #region GetDataReader

        /// <summary>
        /// Get DataReader by Command
        /// <para>** Command必須已設定Connection</para>
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public override IDataReader GetDataReader(IDbCommand command)
        {
            SqlDataReader reader = null;
            SqlCommand SqlCommand = command as SqlCommand;
            if (SqlCommand == null || command.Connection == null)
            {
                return reader;
            }

            reader = SqlCommand.ExecuteReader();

            return reader;
        }

        #endregion

        #region ExecuteReader With CallBack

        /// <summary>
        /// 將DataReader透過CallBack方式，轉至List
        /// <para>若conn=null , 則會自動open and close connection</para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="command"></param>
        /// <param name="entityList"></param>
        /// <param name="ReadFuncCallBack"></param>
        /// <returns></returns>
        public override int ExecuteReader<T>(IDbCommand command, List<T> entityList, ReadDataCallBack<T> ReadFuncCallBack)
        {
            int count = 0;
            SqlCommand SqlCommand = command as SqlCommand;
            if (SqlCommand == null)
            {
                return count;
            }

            SqlConnection conn = SqlCommand.Connection;
            bool isConnection = conn != null;

            SqlDataReader reader = null;
            try
            {
                if (conn == null)
                {
                    conn = ((SqlConnection)GetConncection());
                    SqlCommand.Connection = conn;
                }

                reader = SqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    count++;
                    bool bBreak = ReadFuncCallBack(entityList, reader, count);
                    if (bBreak)
                    {
                        break;
                    }
                }
            }
            finally
            {
                if (reader != null && !reader.IsClosed)
                {
                    reader.Close();
                }
                if (!isConnection && conn != null)
                {
                    if (conn.State != ConnectionState.Closed)
                    {
                        conn.Close();
                    }
                    command.Connection = null;
                    conn.Dispose();
                }
            }

            return count;
        }

        #endregion

        #region Paging (DataTable, DataReader)

        protected override int GetTotalCount(string sql, Pageable pageable, IDbConnection conn = null)
        {
            return 0;
        }

        protected override string GetPageableSql(string sql, Pageable pageable)
        {
            return sql;

        }

        #endregion


        #region Column To Value

        private String BuildSqlMessage(SqlCommand cmd)
        {
            StringBuilder s = new StringBuilder(cmd.CommandText);
            if (cmd.Parameters != null)
            {
                foreach (SqlParameter par in cmd.Parameters)
                {
                    s.Append("\r\n").Append(par.ParameterName).Append(" = ").Append(par.Value);
                }
            }
            return s.ToString();
        }
        public int CloumnToInt(DataRow row, string columnName)
        {
            return row.IsNull(columnName) ? 0 : System.Convert.ToInt32(row[columnName]);
        }

        public string CloumnToString(DataRow row, string columnName)
        {
            return row.IsNull(columnName) ? string.Empty : System.Convert.ToString(row[columnName]).TrimEnd();
        }

        public DateTime CloumnToDateTime(DataRow row, string columnName)
        {
            return row.IsNull(columnName) ? DateTime.MinValue : System.Convert.ToDateTime(row[columnName]);
        }

        #endregion

    }
}
