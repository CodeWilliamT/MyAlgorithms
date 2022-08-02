using System;
using System.Collections;
using System.Collections.Specialized;
using System.Data;
using System.Data.SQLite;
using System.Configuration;

namespace ToolKits.DataManage.SqlFile
{
    ///<summary>
    ///数据库连接工具类
    ///</summary>
    public class SqlFile
    {
        #region 字段
        private string connectionString = "";
        /// <summary>
        ///数据库连接字符串(web.config来配置)，可以动态更改SQLString支持多数据库.   
        /// </summary>
        public string ConnectionString
        {
            get
            {
                return connectionString;
            }
            set
            {
                connectionString = value;
            }

        }
        #endregion

        #region 构造器
        /// <summary>
        /// 构造函数
        /// </summary>
        public SqlFile()
        { }
        /// <summary>
        /// 构造函数
        /// </summary>
        public SqlFile(string connectionString)
        {
            this.connectionString = connectionString;
        }
        #endregion

        #region  执行简单SQL语句

        /// <summary>
        /// 执行SQL语句，返回影响的记录数
        /// </summary>
        /// <param name="SQLString">SQL语句</param>
        /// <returns>影响的记录数</returns>
        public int ExecuteSql(string SQLString)
        {
            SQLiteConnectionStringBuilder connstr = new SQLiteConnectionStringBuilder();
            connstr.DataSource = connectionString;
            using (SQLiteConnection connection = new SQLiteConnection(connstr.ToString()))
            {
                using (SQLiteCommand cmd = new SQLiteCommand(SQLString, connection))
                {
                    try
                    {
                        connection.Open();
                        int rows = cmd.ExecuteNonQuery();
                        return rows;
                    }
                    catch (System.Data.SQLite.SQLiteException E)
                    {
                        connection.Close();
                        throw new Exception(E.Message);
                    }
                }
            }
        }

        /// <summary>
        /// 执行SQL语句，设置命令的执行等待时间
        /// </summary>
        /// <param name="SQLString"></param>
        /// <param name="Times"></param>
        /// <returns></returns>
        public int ExecuteSqlByTime(string SQLString, int Times)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                using (SQLiteCommand cmd = new SQLiteCommand(SQLString, connection))
                {
                    try
                    {
                        connection.Open();
                        cmd.CommandTimeout = Times;
                        int rows = cmd.ExecuteNonQuery();
                        return rows;
                    }
                    catch (System.Data.SQLite.SQLiteException E)
                    {
                        connection.Close();
                        throw new Exception(E.Message);
                    }
                }
            }
        }

        /// <summary>
        /// 执行多条SQL语句，实现数据库事务。  被ImageDataMgr调用   执行后退出
        /// </summary>
        /// <param name="SQLStringList">多条SQL语句</param>        
        public void ExecuteSqlTran(ArrayList SQLStringList)
        {

            SQLiteConnectionStringBuilder connstr = new SQLiteConnectionStringBuilder();
            connstr.DataSource = connectionString;
            using (SQLiteConnection conn = new SQLiteConnection(connstr.ToString()))
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand();
                cmd.Connection = conn;
                SQLiteTransaction tx = conn.BeginTransaction();
                cmd.Transaction = tx;
                try
                {
                    for (int n = 0; n < SQLStringList.Count; n++)
                    {
                        string strsql = SQLStringList[n].ToString();
                        if (strsql.Trim().Length > 1)
                        {
                            cmd.CommandText = strsql;
                            cmd.ExecuteNonQuery();
                        }
                    }

                    tx.Commit();
                }
                catch (System.Data.SQLite.SQLiteException E)
                {
                    tx.Rollback();
                    throw new Exception(E.Message);
                }

            }
        }

        /// <summary>
        /// 执行多条SQL语句，实现数据库事务。    //比productParam三次调用  执行后结束
        /// </summary>
        /// <param name="SQLList">多条SQL语句</param>        
        public bool ExecuteSqlTran(System.Collections.Generic.List<string> SQLList)
        {

            SQLiteConnectionStringBuilder connstr = new SQLiteConnectionStringBuilder();
            connstr.DataSource = connectionString;
            using (SQLiteConnection conn = new SQLiteConnection(connstr.ToString()))
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand();
                cmd.Connection = conn;
                SQLiteTransaction tx = conn.BeginTransaction();
                cmd.Transaction = tx;
                try
                {
                    for (int n = 0; n < SQLList.Count; n++)
                    {
                        string strsql = SQLList[n];
                        if (strsql.Trim().Length > 1)
                        {
                            cmd.CommandText = strsql;
                            cmd.ExecuteNonQuery();
                        }
                    }
                    tx.Commit();
                    return true;
                }
                catch (System.Data.SQLite.SQLiteException ex)
                {
                    tx.Rollback();
                    //Wayrise.WR800.LogHelper.LogException(ex);
                    return false;
                }
            }
        }

        /// <summary>
        /// 以事务的方式执行单条语句  被调用一次   执行后结束
        /// </summary>
        /// <param name="strSQL"></param>
        public bool ExecuteSqlTran(string strSQL)
        {

            SQLiteConnectionStringBuilder connstr = new SQLiteConnectionStringBuilder();
            connstr.DataSource = connectionString;
            using (SQLiteConnection conn = new SQLiteConnection(connstr.ToString()))
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand();
                cmd.Connection = conn;
                SQLiteTransaction tx = conn.BeginTransaction();
                cmd.Transaction = tx;
                try
                {
                    cmd.CommandText = strSQL;
                    cmd.ExecuteNonQuery();
                    tx.Commit();
                    return true;
                }
                catch (System.Data.SQLite.SQLiteException ex)
                {
                    tx.Rollback();
                    //Wayrise.WR800.LogHelper.LogException(ex);
                    return false;
                }
            }
        }

        /// <summary>
        /// 执行带一个存储过程参数的的SQL语句。
        /// </summary>
        /// <param name="SQLString">SQL语句</param>
        /// <param name="content">参数内容,比如一个字段是格式复杂的文章，有特殊符号，可以通过这个方式添加</param>
        /// <returns>影响的记录数</returns>
        public int ExecuteSql(string SQLString, string content)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                SQLiteCommand cmd = new SQLiteCommand(SQLString, connection);
                SQLiteParameter myParameter = new SQLiteParameter("@content", DbType.String);
                myParameter.Value = content;
                cmd.Parameters.Add(myParameter);
                try
                {
                    connection.Open();
                    int rows = cmd.ExecuteNonQuery();
                    return rows;
                }
                catch (System.Data.SQLite.SQLiteException E)
                {
                    throw new Exception(E.Message);
                }
                finally
                {
                    cmd.Dispose();
                    connection.Close();
                }
            }
        }

        /// <summary>
        /// 执行带一个存储过程参数的的SQL语句。
        /// </summary>
        /// <param name="SQLString">SQL语句</param>
        /// <param name="content">参数内容,比如一个字段是格式复杂的文章，有特殊符号，可以通过这个方式添加</param>
        /// <returns>影响的记录数</returns>
        public object ExecuteSqlGet(string SQLString, string content)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                SQLiteCommand cmd = new SQLiteCommand(SQLString, connection);
                SQLiteParameter myParameter = new SQLiteParameter("@content", DbType.String);
                myParameter.Value = content;
                cmd.Parameters.Add(myParameter);
                try
                {
                    connection.Open();
                    object obj = cmd.ExecuteScalar();
                    if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                    {
                        return null;
                    }
                    else
                    {
                        return obj;
                    }
                }
                catch (System.Data.SQLite.SQLiteException E)
                {
                    throw new Exception(E.Message);
                }
                finally
                {
                    cmd.Dispose();
                    connection.Close();
                }
            }
        }

        /// <summary>
        /// 向数据库里插入图像格式的字段(和上面情况类似的另一种实例)
        /// </summary>
        /// <param name="strSQL">SQL语句</param>
        /// <param name="fs">图像字节,数据库的字段类型为image的情况</param>
        /// <returns>影响的记录数</returns>
        public int ExecuteSqlInsertImg(string strSQL, byte[] fs)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                SQLiteCommand cmd = new SQLiteCommand(strSQL, connection);
                SQLiteParameter myParameter = new SQLiteParameter("@fs", DbType.Binary);
                myParameter.Value = fs;
                cmd.Parameters.Add(myParameter);
                try
                {
                    connection.Open();
                    int rows = cmd.ExecuteNonQuery();
                    return rows;
                }
                catch (System.Data.SQLite.SQLiteException E)
                {
                    throw new Exception(E.Message);
                }
                finally
                {
                    cmd.Dispose();
                    connection.Close();
                }
            }
        }

        /// <summary>
        /// 执行一条计算查询结果语句，返回查询结果（object）。
        /// </summary>
        /// <param name="SQLString">计算查询结果语句</param>
        /// <returns>查询结果（object）</returns>
        public object GetSingle(string SQLString)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                using (SQLiteCommand cmd = new SQLiteCommand(SQLString, connection))
                {
                    try
                    {
                        connection.Open();
                        object obj = cmd.ExecuteScalar();
                        if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                        {
                            return null;
                        }
                        else
                        {
                            return obj;
                        }
                    }
                    catch (System.Data.SQLite.SQLiteException e)
                    {
                        connection.Close();
                        throw new Exception(e.Message);
                    }
                }
            }
        }

        /// <summary>
        /// 执行查询语句，返回SQLiteDataReader(使用该方法切记要手工关闭SQLiteDataReader和连接)
        /// </summary>
        /// <param name="strSQL">查询语句</param>
        /// <returns>SQLiteDataReader</returns>
        public SQLiteDataReader ExecuteReader(string strSQL)
        {
            SQLiteConnectionStringBuilder connstr = new SQLiteConnectionStringBuilder();
            connstr.DataSource = connectionString;
            SQLiteConnection conn = new SQLiteConnection(connstr.ToString());
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand(strSQL);
                cmd.Connection = conn;
                try
                {
                    SQLiteDataReader myReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    return myReader;
                }
                catch (System.Data.SQLite.SQLiteException E)
                {
                    throw new Exception(E.Message);
                }
            }
        }

        /// <summary>
        /// 执行查询语句，返回DataSet   执行后退出
        /// </summary>
        /// <param name="SQLString">查询语句</param>
        /// <returns>DataSet</returns>
        public DataSet Query(string SQLString)
        {
            SQLiteConnectionStringBuilder connstr = new SQLiteConnectionStringBuilder();
            connstr.DataSource = connectionString;

            using (SQLiteConnection connection = new SQLiteConnection(connstr.ToString()))
            //using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {

                DataSet ds = new DataSet();
                try
                {
                    //connection.Open();
                    SQLiteDataAdapter command = new SQLiteDataAdapter(SQLString, connection);
                    command.Fill(ds, "ds");
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                return ds;
            }
        }

        /// <summary>
        /// 执行查询语句，返回DataSet,设置命令的执行等待时间
        /// </summary>
        /// <param name="SQLString"></param>
        /// <param name="Times"></param>
        /// <returns></returns>
        public DataSet Query(string SQLString, int Times)
        {
            SQLiteConnectionStringBuilder connstr = new SQLiteConnectionStringBuilder();
            connstr.DataSource = connectionString;
            using (SQLiteConnection connection = new SQLiteConnection(connstr.ToString()))
            {
                DataSet ds = new DataSet();
                try
                {
                    //DataAdapter获取数据不需要将设置连接的状态为打开
                    //connection.Open();
                    SQLiteDataAdapter command = new SQLiteDataAdapter(SQLString, connection);
                    command.SelectCommand.CommandTimeout = Times;
                    command.Fill(ds, "ds");
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                return ds;
            }
        }

        #endregion

        #region 执行带参数的SQL语句
        /*
    ///// <summary>
    ///// 执行多条SQL语句，实现数据库事务。
    ///// </summary>
    ///// <param name="SQLStringList">SQL语句的哈希表（key为sql语句，value是该语句的SQLiteParameter[]）</param>
    //public void ExecuteSqlTran(Hashtable SQLStringList)
    //{
    //    using (SQLiteConnection conn = new SQLiteConnection(connectionString))
    //    {
    //        conn.Open();
    //        using (SQLiteTransaction trans = conn.BeginTransaction())
    //        {
    //            SQLiteCommand cmd = new SQLiteCommand();
    //            try
    //            {
    //                //循环
    //                foreach (DictionaryEntry myDE in SQLStringList)
    //                {
    //                    string cmdText = myDE.Key.ToString();
    //                    SQLiteParameter[] cmdParms = (SQLiteParameter[])myDE.Value;
    //                    PrepareCommand(cmd, conn, trans, cmdText, cmdParms);
    //                    int val = cmd.ExecuteNonQuery();
    //                    cmd.Parameters.Clear();

    //                    trans.Commit();
    //                }
    //            }
    //            catch
    //            {
    //                trans.Rollback();
    //                throw;
    //            }
    //        }
    //    }
    //}

    /// <summary>
    /// 执行一条计算查询结果语句，返回查询结果（object）。
    /// </summary>
    /// <param name="SQLString">计算查询结果语句</param>
    /// <param name="cmdParms">参数结构，暂时未使用</param>
    /// <returns>查询结果（object）</returns>
    public object GetSingle(string SQLString, params SQLiteParameter[] cmdParms)
    {
        using (SQLiteConnection connection = new SQLiteConnection(connectionString))
        {
            using (SQLiteCommand cmd = new SQLiteCommand())
            {
                try
                {
                    PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                    object obj = cmd.ExecuteScalar();
                    cmd.Parameters.Clear();
                    if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                    {
                        return null;
                    }
                    else
                    {
                        return obj;
                    }
                }
                catch (System.Data.SQLite.SQLiteException e)
                {
                    throw new Exception(e.Message);
                }
            }
        }
    }
    */
        /// <summary>
        /// 执行查询语句，返回SQLiteDataReader (使用该方法切记要手工关闭SQLiteDataReader和连接)  被productParamMgr一次调用  执行后需要查询
        /// </summary>
        /// <param name="SQLString">查询语句</param>
        /// <param name="cmdParms">参数名称结构</param>
        /// <returns>SQLiteDataReader</returns>
        public SQLiteDataReader ExecuteReader(string SQLString, params SQLiteParameter[] cmdParms)
        {
            SQLiteConnectionStringBuilder connstr = new SQLiteConnectionStringBuilder();
            connstr.DataSource = connectionString;
            SQLiteConnection connection = new SQLiteConnection(connstr.ToString());
            SQLiteCommand cmd = new SQLiteCommand();
            PrepareCommand(cmd, connection, null, SQLString, cmdParms);
            try
            {
                SQLiteDataReader sReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                return sReader;
            }
            catch (System.Data.SQLite.SQLiteException e)
            {
                connection.Close();
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// 对数据库执行command进行预处理
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="conn"></param>
        /// <param name="trans"></param>
        /// <param name="cmdText"></param>
        /// <param name="cmdParms"></param>
        public void PrepareCommand(SQLiteCommand cmd, SQLiteConnection conn,
                         SQLiteTransaction trans, string cmdText, SQLiteParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            if (trans != null)
                cmd.Transaction = trans;
            cmd.CommandType = CommandType.Text;//cmdType;
            if (cmdParms != null)
            {
                foreach (SQLiteParameter parameter in cmdParms)
                {
                    if ((parameter.Direction == ParameterDirection.InputOutput
                            || parameter.Direction == ParameterDirection.Input) &&
                        (parameter.Value == null))
                    {
                        parameter.Value = DBNull.Value;
                    }
                    cmd.Parameters.Add(parameter);
                }
            }
        }

        #endregion

        #region 参数转换
        /// <summary>
        /// 放回一个SQLiteParameter
        /// </summary>
        /// <param name="name">参数名字</param>
        /// <param name="type">参数类型</param>
        /// <param name="size">参数大小</param>
        /// <param name="value">参数值</param>
        /// <returns>SQLiteParameter的值</returns>
        public SQLiteParameter MakeSQLiteParameter(string name, DbType type, int size, object value)
        {
            SQLiteParameter parm = new SQLiteParameter(name, type, size);
            parm.Value = value;
            return parm;
        }
        /// <summary>
        /// 在数据库中添加一个表
        /// </summary>
        public void CreateTable(string strSQL)
        {
            ExecuteSql(strSQL);
        }
        /// <summary>
        /// 判断数据库中的某一个表是否存在
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public bool TableIsExists(string tableName)
        {
            string strSQL = @"select count(*) as num from sqlite_master where type='table' and name='" + tableName + "'";

            SQLiteConnectionStringBuilder connstr = new SQLiteConnectionStringBuilder();
            connstr.DataSource = connectionString;
            using (SQLiteConnection conn = new SQLiteConnection(connstr.ToString()))
            {
                try
                {
                    conn.Open();
                    SQLiteCommand cmd = new SQLiteCommand(strSQL);
                    cmd.Connection = conn;
                    object obj = cmd.ExecuteScalar();
                    if (obj.ToString() == "0")
                        return false;
                    else
                        return true;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    conn.Close();
                }
            }
        }
        /// <summary>
        /// 判断字段中是否有该项内容
        /// </summary>
        /// <param name="strSQL"></param>
        /// <returns></returns>
        public bool ContentIsExists(string strSQL)
        {
            SQLiteConnectionStringBuilder connstr = new SQLiteConnectionStringBuilder();
            connstr.DataSource = connectionString;
            using (SQLiteConnection conn = new SQLiteConnection(connstr.ToString()))
            {
                try
                {
                    conn.Open();
                    SQLiteCommand cmd = new SQLiteCommand(strSQL);
                    cmd.Connection = conn;
                    object obj = cmd.ExecuteScalar();
                    if (obj == null || obj.ToString() == "0")
                        return false;
                    else
                        return true;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    conn.Close();
                }
            }
        }
        #endregion
    }
}
