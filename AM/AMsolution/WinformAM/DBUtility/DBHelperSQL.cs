using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections;
using System.Diagnostics;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Configuration;
using System.IO;
using WinformAM.Tool;
using System.Reflection;

namespace WinformAM.DBUtility
{
    public class DBHelperSQL : IDisposable
    {

        LogManager logManager = new LogManager();
        private SqlConnection connection;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// 
        public DBHelperSQL()
        {
            try
            {
                if (connection == null)
                {
                    connection = new SqlConnection(System.Configuration.ConfigurationSettings.AppSettings["connection"]);
                }
            }
            catch (Exception ex)
            {
                logManager.WriteErrLog("", MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// 带参数的构造函数，通过特定的连接字符串连接数据库
        /// </summary>
        /// <param name="connectionString">指定的数据库连接字符串</param>

        //public static DBHelperSQL(string connectionString)
        //{
        //    try
        //    {
        //        if (connection == null)
        //        {
        //            connection = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings[connectionString]);
        //        }
        //    }
        //    catch (Exception exception)
        //    {
        //        Log.WriteLog(">DALBase-->Database-->Database(string)", exception);
        //    }
        //}


        /// <summary>
        /// 打开数据库
        /// </summary>

        public void Open()
        {
            try
            {
                if (connection != null && connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }

            }
            catch (Exception exception)
            {
                logManager.WriteErrLog("", MethodInfo.GetCurrentMethod().Name, exception.Message);
            }
        }


        /// <summary>
        /// 关闭数据库
        /// </summary>

        public void Close()
        {
            try
            {
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            catch (Exception exception)
            {
                logManager.WriteErrLog("", MethodInfo.GetCurrentMethod().Name, exception.Message);
            }

        }


        /// <summary>
        /// 释放数据库连接
        /// </summary>

        public void Dispose()
        {
            if (connection != null)
            {
                connection.Dispose();
                connection = null;
            }
        }

        #region 执行SQL语句开始

        /// <summary>
        /// 执行不带参数的非查询SQL语句：适应于update , delete ,add 三类SQL语句
        /// </summary>
        /// <param name="SQL"></param>
        /// <param name="callMethodName"></param>
        /// <returns></returns>
        /// 
        public bool ExecuteNonQuery(StringBuilder SQL, string callMethodName)
        {
            string sql = SQL.ToString();

            bool result = true;

            try
            {
                Open();
                SqlCommand command = new SqlCommand(sql, connection);
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                result = false;
                logManager.WriteErrLog("",callMethodName+MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
            finally
            {
                Close();
            }

            return result;
        }


        /// <summary>
        /// 执行带有参数SQL语句：适应于update , delete ,add 三类SQL语句
        /// </summary>
        /// <param name="SQL"></param>
        /// <param name="prams"></param>
        /// <param name="callMethodName"></param>
        /// <returns></returns>

        public bool ExecuteNonQuery(StringBuilder SQL, SqlParameter[] prams, string callMethodName)
        {
            string sql = SQL.ToString();

            bool result = true;
            try
            {
                Open();
                SqlCommand command = CreateCommandForSQL(sql, prams);
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                result = false;
                logManager.WriteErrLog("", callMethodName + MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
            finally
            {
                Close();
            }

            return result;
        }

        /// <summary>
        /// 执行不带参数的查询SQL语句
        /// </summary>
        /// <param name="SQL"></param>
        /// <param name="callMethodName"></param>
        /// <returns></returns>

        public DataTable ExecuteQuery(StringBuilder SQL, string callMethodName)
        {
            string sql = SQL.ToString();

            SqlDataReader dataReader = null;
            DataTable dataTable = new DataTable();

            try
            {
                SqlCommand cmd = CreateCommandForSQL(sql, null);
                dataReader = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                //System.Data.CommandBehavior.CloseConnection意味着：在执行该命令时，如果关闭关联的 DataReader 对象，则关联的 Connection 对象也将关闭。             
                dataTable.Load(dataReader);
            }
            catch (Exception ex)
            {
                dataTable = null;
                logManager.WriteErrLog("", callMethodName + MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
            finally
            {

                if (dataReader != null)
                {
                    dataReader.Close();
                }
            }

            return dataTable;
        }


        /// <summary>
        /// 执行带有参数查询SQL语句
        /// </summary>
        /// <param name="SQL"></param>
        /// <param name="prams"></param>
        /// <param name="callMethodName"></param>
        /// <returns></returns>

        public DataTable ExecuteQuery(StringBuilder SQL, SqlParameter[] prams, string callMethodName)
        {

            string sql = SQL.ToString();

            SqlDataReader dataReader = null;
            DataTable dataTable = new DataTable();

            try
            {
                SqlCommand cmd = CreateCommandForSQL(sql, prams);
                dataReader = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                dataTable.Load(dataReader);
            }
            catch (Exception ex)
            {
                dataTable = null;
                logManager.WriteErrLog("", callMethodName + MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
            finally
            {
                if (dataReader != null)
                {
                    dataReader.Close();
                }
            }

            return dataTable;

        }


        /// <summary>
        /// 执行一组非查询SQL语句
        /// </summary>
        /// <param name="SqlStrings">一组SQL语句</param>
        /// <param name="callMethodName"></param>
        /// <returns></returns>

        public bool ExecuteNonQuerySQLS(ArrayList SqlStrings, String callMethodName)
        {
            bool result = true;

            Open();
            SqlCommand cmd = new SqlCommand();
            SqlTransaction trans = connection.BeginTransaction();
            cmd.Connection = connection;
            cmd.Transaction = trans;

            try
            {
                foreach (String sql in SqlStrings)
                {
                    cmd.CommandText = sql;
                    cmd.ExecuteNonQuery();
                }

                trans.Commit();
            }
            catch (Exception ex)
            {
                result = false;
                trans.Rollback();
                logManager.WriteErrLog("", callMethodName + MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
            finally
            {
                Close();
            }

            return result;

        }

        /// <summary>
        /// 执行一组带有参数的非查询SQL语句
        /// </summary>
        /// <param name="SqlStrings"></param>
        /// <param name="prams"></param>
        /// <param name="callMethodName"></param>
        /// <returns></returns>


        public bool ExecuteNonQuerySQLS(ArrayList SqlStrings, ArrayList prams, String callMethodName)
        {

            bool result = true;

            Open();
            SqlCommand cmd = new SqlCommand();
            SqlTransaction trans = connection.BeginTransaction();
            cmd.Connection = connection;
            cmd.Transaction = trans;

            try
            {

                int sqlCount = SqlStrings.Count;

                for (int i = 0; i < sqlCount; i++)
                {

                    cmd.CommandText = SqlStrings[i].ToString();

                    if (prams[i] != null)
                    {
                        foreach (SqlParameter parameter in (SqlParameter[])prams[i])
                            cmd.Parameters.Add(parameter);
                    }

                    cmd.Parameters.Add(
                        new SqlParameter("ReturnValue", SqlDbType.Int, 4,
                        ParameterDirection.ReturnValue, false, 0, 0,
                        string.Empty, DataRowVersion.Default, null));

                    cmd.ExecuteNonQuery();

                    cmd.Parameters.Clear();

                }

                trans.Commit();
            }
            catch (Exception ex)
            {
                result = false;
                trans.Rollback();
                logManager.WriteErrLog("", callMethodName + MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
            finally
            {
                Close();
            }

            return result;

        }
        #endregion 执行SQL语句结束

        //////////////////////////////////////////////////////////////////////////////////////////////////////
        /*  描    述： 创建Sqlcommand对象，为执行存储过程做准备。
         *  输入参数： procName-->存储过程名;prams-->存储过程参数
         *  返 回 值： Sqlcommand-->Sqlcommand对象*/


        private SqlCommand CreateCommandForSQL(string sql, SqlParameter[] prams)
        {
            Open();
            SqlCommand cmd = new SqlCommand(sql, connection);
            cmd.CommandType = CommandType.Text;

            if (prams != null)
            {
                foreach (SqlParameter parameter in prams)
                    cmd.Parameters.Add(parameter);
            }

            cmd.Parameters.Add(
                new SqlParameter("ReturnValue", SqlDbType.Int, 4,
                ParameterDirection.ReturnValue, false, 0, 0,
                string.Empty, DataRowVersion.Default, null));

            return cmd;
        }



        /// <summary>
        /// 创建输入类型的参数
        /// </summary>
        /// <param name="ParamName">参数名</param>
        /// <param name="DbType">参数类型</param>
        /// <param name="Size">参数大小</param>
        /// <param name="Value">参数值</param>
        /// <returns></returns>

        public static SqlParameter MakeInParam(string ParamName, SqlDbType DbType, int Size, object Value)
        {
            return MakeParam(ParamName, DbType, Size, ParameterDirection.Input, Value);
        }


        /// <summary>
        /// 创建输出类型的参数
        /// </summary>
        /// <param name="ParamName">参数名</param>
        /// <param name="DbType">参数类型</param>
        /// <param name="Size">参数大小</param>
        /// <returns></returns>

        public static SqlParameter MakeOutParam(string ParamName, SqlDbType DbType, int Size)
        {
            return MakeParam(ParamName, DbType, Size, ParameterDirection.Output, null);
        }


        /// <summary>
        /// 创建参数
        /// </summary>
        /// <param name="ParamName">参数名</param>
        /// <param name="DbType">参数类型</param>
        /// <param name="Size">参数大小</param>
        /// <param name="Direction">参数方向</param>
        /// <param name="Value">参数值</param>
        /// <returns></returns>

        public static SqlParameter MakeParam(string ParamName, SqlDbType DbType, Int32 Size, ParameterDirection Direction, object Value)
        {
            SqlParameter param;

            if (Size > 0)
                param = new SqlParameter(ParamName, DbType, Size);
            else
                param = new SqlParameter(ParamName, DbType);

            param.Direction = Direction;
            if (!(Direction == ParameterDirection.Output && Value == null))
                param.Value = Value;

            return param;
        }

        /// <summary>
        /// 执行插入多个表的操作，第一个表(SQLONE)的主键为自动增长，若id=-1则插入失败。
        /// 执行连锁的插入操作。即利用插入语句SQLONE得到的ID作为后面插入语句的外键之一。
        /// 
        /// sqls和prams示例： 
        ///     int areaid = 123;       // 注：与未知id对应的idvalue设置为@_ID_VALUE，此处未知id为UserID。
        ///     string sql = "insert into T_UserArea(UserID, AreaID) values(@_ID_VALUE, @areaid)";
        ///     SqlParameter[] pram =
        ///     {
        ///              null, // 注：与未知id对应参数设置为空。
        ///              db.MakeInParam("@areaid", SqlDbType.Int, 4, areaid)
        ///      };
        ///      sqls.Add(sql);
        ///      prams.Add(pram);       
        /// </summary>
        /// <param name="SQLONE">执行的第一条SQL语句</param>
        /// <param name="pramsOne">SQLONE语句对应的参数</param>
        /// <param name="sqls">连锁执行的SQL语句链表</param>
        /// <param name="prams">与sqls中每条SQL语句相对应的参数</param>
        /// <param name="callMethodName"></param>
        /// <returns>int  (1)执行成功，返回执行第一条语句所插入的一条记录的ID值。(2) 执行失败，返回-1。</returns>

        public int ExecuteReturnID(StringBuilder SQLONE, SqlParameter[] pramsOne, ArrayList sqls, ArrayList prams, string callMethodName)
        {
            string sqlOne = SQLONE.ToString();
            int id = -1; // 初始化id值为-1

            // 检查参数中SQL语句sqls和参数prams是否对应，不对应则不执行操作，返回-1。
            if (sqls.Count != prams.Count)
            {
                return id;
            }

            // 在第一条insert语句后加入select语句，以获得插入记录的ID值。
            sqlOne += ";select CAST( SCOPE_IDENTITY() AS INT) ";

            SqlCommand cmd = new SqlCommand();

            try
            {
                Open();

                cmd.Connection = connection;
                cmd.CommandText = sqlOne;

                for (int i = 0; i < pramsOne.Length; i++) // 插入对应参数。
                {
                    cmd.Parameters.Add(pramsOne[i]);
                }

                cmd.Transaction = connection.BeginTransaction(); // 事务开始

                id = (Int32)cmd.ExecuteScalar(); // 获得插入记录的ID。

                // 从sqls和prams中获得需执行的SQL语句和与之对应的参数，逐条执行。
                for (int i = 0; i < sqls.Count; i++)
                {
                    string sqlTwo = sqls[i].ToString();
                    SqlParameter[] pramsTwo = (SqlParameter[])prams[i];

                    cmd.CommandText = sqlTwo;

                    for (int j = 0; j < pramsTwo.Length; j++)
                    {
                        // 未知ID在SQL中约定使用@_ID_VALUE代替其值。
                        // 写入的参数组中，与未知ID对应项参数项约定设置为null，
                        // 此处将为null的参数替换为执行第一条insert语句获得插入记录的ID。
                        if (pramsTwo[j] == null)
                        {
                            pramsTwo[j] = MakeInParam("@_ID_VALUE", SqlDbType.Int, 4, id.ToString());
                        }
                        cmd.Parameters.Add(pramsTwo[j]);
                    }

                    cmd.ExecuteNonQuery();
                    // 执行多条insert语句可能会有同名参数，因此执行完一条后清除cmd中的参数。
                    cmd.Parameters.Clear();
                }

                cmd.Transaction.Commit();

            }
            catch (Exception ex)
            {
                id = -1; // 出现异常后为id赋值为-1，操作失败。
                //cmd.Transaction.Rollback();
                cmd.Cancel();
                logManager.WriteErrLog("", callMethodName + MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
            finally
            {
                Close();
            }
            return id;
        }
    }
}
