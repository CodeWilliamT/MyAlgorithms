using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SQLite;

namespace ToolKits.DataManage.Sql
{
    public abstract class DataAccess : SqlFile
    {
        public DataAccess()
            : base()
        {
        }
        public DataAccess(string dirDB)
            : base(dirDB)
        {
        }
        /// <summary>
        /// 返回数据表
        /// </summary>
        /// <param name="strsql"></param>
        /// <returns></returns>
        public DataTable GetData(string strsql)
        {
            try
            {
                return base.Query(strsql).Tables[0];
            }
            catch
            {
                return null;
            }
        }
        public new SQLiteDataReader ExecuteReader(string SQLString, params SQLiteParameter[] cmdParms)
        {
            return base.ExecuteReader(SQLString, cmdParms);
        }
        /// <summary>
        /// 以事务的方式更新数据到数据库
        /// </summary>
        /// <param name="SQLList"></param>
        protected void UpdateData(List<string> SQLList)
        {
            base.ExecuteSqlTran(SQLList);
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="SQLList"></param>
        protected void AddData(List<string> SQLList)
        {
            base.ExecuteSqlTran(SQLList);
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="Sql"></param>
        protected void AddData(string Sql)
        {
            base.ExecuteSqlTran(Sql);
        }
        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="Sql"></param>
        protected new void ExecuteSqlTran(string Sql)
        {
            base.ExecuteSqlTran(Sql);
        }
        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="Sql"></param>
        protected new int ExecuteSql(string Sql)
        {
            return base.ExecuteSql(Sql);
        }
        /// <summary>
        /// 创建表
        /// </summary>
        /// <param name="Sql"></param>
        protected new void CreateTable(string Sql)
        {
            base.CreateTable(Sql);
        }
        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="SQLList"></param>
        protected new void ExecuteSqlTran(List<string> SQLList)
        {
            base.ExecuteSqlTran(SQLList);
        }
        /// <summary>
        /// 判断表格是否存在
        /// </summary>
        /// <param name="strTableName"></param>
        /// <returns></returns>
        protected new bool TableIsExists(string strTableName)
        {
            return base.TableIsExists(strTableName);
        }
        /// <summary>
        /// 判断字段中是否有该项内容
        /// </summary>
        /// <param name="Sql"></param>
        /// <returns></returns>
        protected new bool ContentIsExists(string Sql)
        {
            return base.ContentIsExists(Sql);
        }
    }
}
