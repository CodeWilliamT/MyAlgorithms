/****************************************************************************/
/*  File Name   :   UserManagement.cs                                       */
/*  Brief       :   User management functions                               */
/*  Date        :   2017/7/13                                               */
/*  Author      :   Tongqing CHEN	                                        */
/****************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Data;
using HT_Lib;

namespace LeadframeAOI
{
    class UserManagement
    {
        public String passWord;
        public String workNumber;
        public int userLevel;
        private String paraFile;
        private String paraTable;
        private SQLiteConnection conn;
        private SQLiteDataReader reader;
        /// <summary>
        ///通过构造函数，数据库路径，数据表、数据库连接的初始化，by LYH
        /// </summary>
        /// <param name="para_file"></param>
        /// <param name="para_table"></param>
        public UserManagement(String para_file, String para_table)
        {
            paraFile = para_file;
            paraTable = para_table;
        }
        public void BuildConnection()
        {
            try
            {
                conn = new SQLiteConnection(@"data source=" + paraFile + @";Version=3");
            }
            catch (Exception EXP)
            {
                HTLog.Error(EXP.Message);
            }
        }
        /// <summary>
        /// 向数据表格中插入一行数据 by LYH
        /// </summary>
        /// <returns></returns>
        public Boolean InsertData()
        {
            Boolean ret = false;
            string sql = String.Format("INSERT INTO {0} (Password,Worknumber,Userlevel) VALUES({1},{2},{3})", paraTable, passWord, workNumber, userLevel);
            try
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand(sql, conn);
                if (cmd.ExecuteNonQuery() > 0)
                {
                    ret = true;
                    goto _end;
                }
                HTLog.Error("InsertData failed!");
            }
            catch (Exception exp)
            {
                HTLog.Error("Insert data failed!" + exp);
            }
            _end:
            conn.Close();
            return ret;
        }
        /// 登录校验方法 查询用户名和密码 by LYH
        /// </summary>
        /// <returns></returns>
        public Boolean Login()
        {
            Boolean ret = false;
            string query = String.Format("SELECT * FROM {0} WHERE Worknumber = {1} and Password = {2}", paraTable, workNumber, passWord);
            try
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand(query, conn);
                reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    ret = true;
                    goto _end;
                }
                HTLog.Error("Login failed!");
            }
            catch (Exception exp)
            {
                HTLog.Error("Login failed" + exp);
            }
            _end:
            reader.Close();
            conn.Close();
            return ret;
        }
        /// <summary>
        /// 新建数据库，及数据表，查询数据库中是否有此工号 为false 时工号不存在 by LYH
        /// </summary>
        /// <returns></returns>
        public Boolean Query()
        {
            Boolean ret = false;             //默认工号不存在
            string sql = String.Format("create table if not exists {0} (Password Text NOT NULL, Worknumber Text PRIMARY KEY NOT NULL, Userlevel INT NOT NULL DEFAULT '0')", paraTable);
            try
            {
                conn.Open();                        //此时自动创建数据库
                SQLiteCommand command = new SQLiteCommand(sql, conn);
                command.ExecuteNonQuery();
                sql = String.Format("SELECT * FROM {0} WHERE Worknumber = {1}", paraTable, workNumber);
                SQLiteCommand cmd = new SQLiteCommand(sql, conn);
                reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    passWord = reader["Password"].ToString();
                    userLevel = int.Parse(reader["Userlevel"].ToString());
                    HTLog.Debug("User exitence");
                    ret = true;
                    goto _end;
                }
            }
            catch (Exception exp)
            {
                HTLog.Error("Query user failed " + exp);
            }
            _end:
            reader.Close();
            conn.Close();
            return ret;
        }
        /// <summary>
        /// 更新数据库中的信息 by LYH
        /// </summary>
        /// <returns></returns>
        public Boolean UpdateData()
        {
            Boolean ret = false;
            string query = String.Format("UPDATE {0} set Password = {1} , Userlevel = {2} WHERE Worknumber = {3}", paraTable, passWord, userLevel, workNumber);
            if (conn.State == ConnectionState.Open)
                conn.Close();
            try
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand(query, conn);
                cmd.ExecuteNonQuery();
                if (cmd.ExecuteNonQuery() > 0)
                {
                    HTLog.Debug("Update data successed!");
                    ret = true;
                    goto _end;
                }
                HTLog.Error("Update data failed!");
            }
            catch (Exception exp)
            {
                HTLog.Error("Update data failed" + exp);
            }
            _end:
            conn.Close();
            return ret;
        }
        /// <summary>
        /// 删除数据库中的某条信息  by LYH
        /// </summary>
        /// <param name="workNumber"></param>
        /// <returns></returns>
        public Boolean DeleteData(string workNumber)
        {
            Boolean ret = false;
            string sql = String.Format("DELETE FROM {0} WHERE Worknumber = {1}", paraTable, workNumber);
            try
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand(sql, conn);
                if (cmd.ExecuteNonQuery() > 0)  //只有大于零表示成功
                {
                    ret = true;
                    goto _end;
                }
                HTLog.Error("Delete data failed!");
            }
            catch (Exception exp)
            {
                HTLog.Error("Delete data failed" + exp);
            }
            _end:
            conn.Close();
            return ret;
        }
        /// <summary>
        /// 读取数据库中的信息到List数组中 by LYH
        /// </summary>
        /// <returns></returns>
        public List<String> DataToList()
        {
            List<String> workNumList = new List<String>();
            string query = @"SELECT* FROM '" + paraTable + "'";
            try
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand(query, conn);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    workNumList.Add(reader["Worknumber"].ToString());
                }
                reader.Close();        // 关闭数据集 
            }
            catch (Exception EXP)
            {
                HTLog.Info(EXP.Message);
            }
            conn.Close();
            return workNumList;
        }
    }
}



