using System;
using System.Data;
using System.Data.SQLite;
using System.IO;

namespace Y.ASIS.App.Database
{
    public abstract class DbBase
    {
        private readonly SqliteConn _conn;

        public string DatabaseName { get; private set; }

        public DbBase(string dbName)
            : this(dbName, AppDomain.CurrentDomain.BaseDirectory)
        {

        }

        public DbBase(string dbName, string directory)
        {
            DatabaseName = dbName;
            string path = Path.Combine(directory, dbName);
            _conn = SqliteConn.GetInstance();
            _conn.AppendConnection(path);
            CreateTables();
            CheckFields();
            CheckData();
        }

        protected abstract void CreateTables();

        protected abstract void CheckFields();

        protected abstract void CheckData();

        /// <summary>
        /// 获取连接
        /// </summary>
        /// <returns>SQL连接对象</returns>
        protected SQLiteConnection GetConnection()
        {
            string key = Path.GetFileNameWithoutExtension(DatabaseName);
            return _conn.GetConnection(key);
        }

        protected void ReleaseConnection()
        {
            string key = Path.GetFileNameWithoutExtension(DatabaseName);
            _conn.ReleaseConn(key);
        }

        /// <summary>
        /// 检测某个表中是否存在指定的字段
        /// </summary>
        /// <param name="field">字段</param>
        /// <param name="table">表名</param>
        /// <returns>存在则返回true 否则返回false</returns>
        protected bool IsFieldExist(string field, string table)
        {
            string cmd = string.Format("select count(1) from SQLITE_MASTER where TBL_NAME='{0}' and SQL like '%{1}%'",
                table, field);
            SQLiteConnection conn = GetConnection();
            object result = SqliteHelper.ExecuteScalar(cmd, conn);
            ReleaseConnection();
            return Convert.ToBoolean(result);
        }

        /// <summary>
        /// 尝试向表添加字段
        /// </summary>
        /// <param name="field">字段名</param>
        /// <param name="type">字段类型</param>
        /// <param name="table">表名</param>
        protected void TryAddField(string field, string type, string table, string defaultValue = null)
        {
            if (!IsFieldExist(field, table))
            {
                string cmd = string.Format("alter table '{0}' add '{1}' {2}",
                                table, field, type);
                if (defaultValue != null)
                {
                    cmd += string.Format(" default({0})", defaultValue);
                }
                SQLiteConnection conn = GetConnection();
                SqliteHelper.ExecuteNonQuery(cmd, conn);
                ReleaseConnection();
            }
        }

        /// <summary>
        /// 执行一个没有返回值的指令
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="param"></param>
        public void ExecuteNonQueryCommand(string cmd, SQLiteParameter[] param = null)
        {
            SQLiteConnection conn = GetConnection();
            try
            {
                SqliteHelper.ExecuteNonQuery(cmd, conn, param);
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                ReleaseConnection();
            }
        }

        /// <summary>
        /// 创建一个命令并执行 不返回结果
        /// </summary>
        /// <param name="cmd">要执行的命令</param>
        /// <param name="trans">事务对象</param>
        /// <param name="param">SQL参数</param>
        public void CreateCmdAndExecuteNonQuery(string cmd, SQLiteTransaction trans, SQLiteParameter[] param = null)
        {
            SQLiteConnection conn = GetConnection();
            SQLiteCommand sqliteCmd = SqliteHelper.CreateCommand(cmd, conn, trans, param);
            sqliteCmd.ExecuteNonQuery();
        }

        /// <summary>
        /// 执行SQL查询语句 并返回结果的第一行第一列数据
        /// </summary>
        /// <param name="cmd">SQL语句</param>
        /// <param name="param">SQL参数</param>
        /// <returns>返回结果的第一行第一列数据</returns>
        public object ExecuteScalarCommand(string cmd, SQLiteParameter[] param = null)
        {
            SQLiteConnection conn = GetConnection();
            object result = null;
            try
            {
                result = SqliteHelper.ExecuteScalar(cmd, conn, param);
            }
            catch (Exception e)
            {
                result = null;
                throw e;
            }
            finally
            {
                ReleaseConnection();
            }
            return result;
        }

        /// <summary>
        /// 执行一个查询语句
        /// </summary>
        /// <param name="cmd">SQL语句</param>
        /// <param name="param">SQL参数</param>
        /// <returns>查询结果的DataTable对象</returns>
        public DataTable ExecuteQueryCommand(string cmd, SQLiteParameter[] param = null)
        {
            SQLiteConnection conn = GetConnection();
            DataTable table = null;
            try
            {
                table = SqliteHelper.ExecuteQuery(cmd, conn, param);
            }
            catch (Exception e)
            {
                table = null;
                throw e;
            }
            finally
            {
                ReleaseConnection();
            }
            return table;
        }

        public void Close()
        {
            ReleaseConnection();
        }
    }
}
