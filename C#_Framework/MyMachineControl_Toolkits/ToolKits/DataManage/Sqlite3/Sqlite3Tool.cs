using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;

namespace ToolKits.DataManage.SqlFile
{
    public class Sqlite3Tool
    {
        /// <summary>
        /// 数据源
        /// </summary>
        private string dataSource;
        private SQLiteConnection sqlConnection;
        private SQLiteConnectionStringBuilder sqlConBuilder;
        private SQLiteCommand sqlCommand;
        public SQLiteConnection SqlConnection
        {
            get { return this.sqlConnection; }
            private set { }
        }
        public Sqlite3Tool(string dataSource)
        {
            this.dataSource = dataSource;
            this.sqlConnection = new SQLiteConnection();
            this.sqlConBuilder = new SQLiteConnectionStringBuilder();
            this.sqlCommand = new SQLiteCommand();
            if (dataSource.Contains("\\\\"))
                dataSource = dataSource.Replace("\\", "\\\\");
            this.sqlConBuilder.DataSource = dataSource;
            this.sqlConnection.ConnectionString = this.sqlConBuilder.ToString();
        }
        /// <summary>
        /// 创建数据库文件
        /// </summary>
        /// <returns></returns>
        public bool CreateDB()
        {
            string folderPath = Directory.GetParent(this.dataSource).FullName;
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);
            //if (!File.Exists(this.dataSource))
                SQLiteConnection.CreateFile(this.dataSource);

            return true;
        }
        /// <summary>
        /// 连接数据库
        /// </summary>
        /// <returns></returns>
        public bool Open()
        {
            try
            {
                this.sqlConnection.Open();
                this.sqlCommand.Connection = this.sqlConnection;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        /// <summary>
        /// 关闭数据库
        /// </summary>
        /// <returns></returns>
        public bool Close()
        {
            try
            {
                this.sqlConnection.Close();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public void Dispose()
        {
            try
            {
                this.sqlConnection.Dispose();
            }
            catch (Exception)
            {
            }
        }
        /// <summary>
        /// 在数据库中创建一个表
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="coloumnNames">"val1,val2...valn"</param>
        /// <returns></returns>
        public bool CreateTable(string tableName, string columnNames)
        {
            try
            {
                string sql = "create table " + tableName + "(" + columnNames + ")";
                this.sqlCommand.CommandText = sql;
                this.sqlCommand.ExecuteNonQuery();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        /// <summary>
        /// 清空表数据
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public bool EmptyTable(string tableName) 
        {
            try
            {
                string sql = "delete from " + tableName;
                this.sqlCommand.CommandText = sql;
                this.sqlCommand.ExecuteNonQuery();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        /// <summary>
        /// 插入一条数据
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="coloumnNames"></param>
        /// <param name="rowData"></param>
        /// <returns></returns>
        public bool InsertData(string tableName, string columnNames, string rowData)
        {
            try
            {
                string sql = "insert into " + tableName + " (" + columnNames + ") values(" + rowData + ")";
                this.sqlCommand.CommandText = sql;
                this.sqlCommand.ExecuteNonQuery();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        /// <summary>
        /// 插入大批量数据
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="columnNames"></param>
        /// <param name="rowData"></param>
        /// <returns></returns>
        public bool InsertMassData(string tableName, string columnNames, List<string> rowData)
        {
            SQLiteTransaction sqlTrans = this.sqlConnection.BeginTransaction();
            try
            {
                string sql = "";
                for (int i = 0; i < rowData.Count; i++)
                {
                    sql = "insert into " + tableName + " (" + columnNames + ") values(" + rowData[i] + ")";
                    SQLiteCommand cmd = new SQLiteCommand(this.sqlConnection);
                    cmd.Transaction = sqlTrans;
                    cmd.CommandText = sql;
                    cmd.ExecuteNonQuery();
                    //this.sqlCommand.CommandText = sql;
                    //this.sqlCommand.ExecuteNonQuery();
                }
                sqlTrans.Commit();

                return true;
            }
            catch (Exception)
            {
                sqlTrans.Rollback();
                return false;
            }
        }
        /// <summary>
        /// 更新某个字段对应的数值
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="predicate"></param>
        /// <param name="updateData"></param>
        /// <returns></returns>
        public bool UpdateData(string tableName, string predicate, string updateData)
        {
            try
            {
                string sql = "update " + tableName + " set " + updateData + " where " + predicate;
                this.sqlCommand.CommandText = sql;
                this.sqlCommand.ExecuteNonQuery();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        /// <summary>
        /// 删除某条数据
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public bool DeleteData(string tableName, string predicate)
        {
            try
            {
                string sql = "delete from " + tableName + " where " + predicate;
                this.sqlCommand.CommandText = sql;
                this.sqlCommand.ExecuteNonQuery();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        /// <summary>
        /// 读取表内所有数据
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="columnNames"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool ReadData(string tableName, string columnNames, out List<string> data)
        {
            data = new List<string>();
            try
            {

                string sql = "select * from " + tableName;
                this.sqlCommand.CommandText = sql;
                SQLiteDataReader reader = this.sqlCommand.ExecuteReader();
                string[] colNames = columnNames.Split(',');

                while (reader.Read())
                {
                    string str = "";
                    for (int i = 0; i < colNames.Length; i++)
                    {
                        str += reader.GetString(i) + ",";
                    }
                    str = str.Remove(str.Length - 1);
                    data.Add(str);
                }

                reader.Dispose();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        /// <summary>
        /// 查询表是否存在
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public bool IsTableExist(string tableName)
        {
            try
            {
                string sql = "select count(*) as num from sqlite_master where type='table' and name='" + tableName + "'";
                this.sqlCommand.CommandText = sql;
                object obj = this.sqlCommand.ExecuteScalar();
                if (obj.ToString() == "0")
                    return false;

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        /// <summary>
        /// 查询表中某个字段是否存在
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public bool IsContentExist(string tableName, string predicate)
        {
            try
            {
                string sql = "select count(*) from " + tableName + " where " + predicate;
                this.sqlCommand.CommandText = sql;
                object obj = this.sqlCommand.ExecuteScalar();
                if (obj.ToString() == "0")
                    return false;

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
