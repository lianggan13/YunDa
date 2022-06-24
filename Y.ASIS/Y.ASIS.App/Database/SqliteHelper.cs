using System.Data;
using System.Data.SQLite;

namespace Y.ASIS.App.Database
{
    public sealed class SqliteHelper
    {
        /// <summary>
        /// 执行带参数的SQL语句
        /// </summary>
        /// <param name="command">SQL语句</param>
        /// <param name="conn">数据库连接对象</param>
        /// <param name="param">SQL参数</param>
        /// <returns>返回受影响的行数</returns>
        public static int ExecuteNonQuery(string command, SQLiteConnection conn, SQLiteParameter[] param = null)
        {
            int rows = 0;
            using(SQLiteCommand cmd = new SQLiteCommand(command, conn))
            {
                if (param != null) cmd.Parameters.AddRange(param);
                rows = cmd.ExecuteNonQuery();
            }
            return rows;
        }

        /// <summary>
        /// 执行SQL查询语句，并返回结果的第一行第一列数据
        /// </summary>
        /// <param name="command">SQL语句</param>
        /// <param name="conn">数据库连接对象</param>
        /// <param name="param">SQL参数</param>
        /// <returns>返回结果的第一行第一列数据</returns>
        public static object ExecuteScalar(string command, SQLiteConnection conn, SQLiteParameter[] param = null)
        {
            object result = null;
            using(SQLiteCommand cmd = new SQLiteCommand(command, conn))
            {
                if (param != null) cmd.Parameters.AddRange(param);
                result = cmd.ExecuteScalar();
            }
            return result;
        }

        /// <summary>
        /// 执行一个查询SQL语句，返回一个DataTable
        /// </summary>
        /// <param name="command">SQL语句</param>
        /// <param name="conn">数据库连接对象</param>
        /// <param name="param">SQL参数</param>
        /// <returns>返回查询结果表</returns>
        public static DataTable ExecuteQuery(string command, SQLiteConnection conn, SQLiteParameter[] param = null)
        {
            DataTable table = new DataTable();
            using(SQLiteCommand cmd = new SQLiteCommand(command, conn))
            {
                if (param != null) cmd.Parameters.AddRange(param);
                using(SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd))
                {
                    adapter.Fill(table);
                }
            }
            return table;
        }

        /// <summary>
        /// 分页查询，并返回一个DataSet
        /// </summary>
        /// <param name="pageCount">返回查询结果总数</param>
        /// <param name="pageIndex">要获取的页的索引</param>
        /// <param name="pageSize">该页大小</param>
        /// <param name="command">SQL查询语句</param>
        /// <param name="countCommand">查询满足条件的数据条数的SQL语句</param>
        /// <param name="conn">连接对象</param>
        /// <param name="param">SQL参数</param>
        /// <returns>返回包含该页数据的DataSet</returns>
        public static DataSet ExecutePager(ref int pageCount, int pageIndex, int pageSize,
            string command, string countCommand, SQLiteConnection conn, SQLiteParameter[] param = null)
        {
            if(pageCount < 0)
            {
                pageCount = int.Parse(ExecuteScalar(countCommand, conn, param).ToString());
            }
            DataSet set = new DataSet();
            using (SQLiteCommand cmd = new SQLiteCommand(command, conn))
            {
                if (param != null) cmd.Parameters.AddRange(param);
                using(SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd))
                {
                    adapter.Fill(set, (pageIndex - 1) * pageSize, pageSize, "result"); ;
                }
            }
            return set;
        }

        /// <summary>
        /// 执行一个查询语句，返回一个SQLiteDataReader实例
        /// </summary>
        /// <param name="command">SQL查询语句</param>
        /// <param name="conn">连接对象</param>
        /// <param name="param">SQL参数</param>
        /// <returns>查询的SQLiteDataReader实例</returns>
        public static SQLiteDataReader ExecuteReader(string command, SQLiteConnection conn, SQLiteParameter[] param = null)
        {
            SQLiteDataReader reader = null;
            using (SQLiteCommand cmd = new SQLiteCommand(command, conn))
            {
                if (param != null) cmd.Parameters.AddRange(param);
                reader = cmd.ExecuteReader();
            }
            return reader;
        }

        /// <summary>
        /// 查询数据库中的所有数据类型信息
        /// </summary>
        /// <param name="conn">连接对象</param>
        /// <returns>返回包含数据库类型的DataTable对象</returns>
        public static DataTable GetSchema(SQLiteConnection conn)
        {
            DataTable data = conn.GetSchema("TABLES");
            return data;
        }

        /// <summary>
        /// 创建事务命令
        /// </summary>
        /// <param name="commandText">要添加的命令</param>
        /// <param name="conn">连接对象</param>
        /// <param name="trans">事务对象</param>
        /// <param name="param">SQL参数</param>
        /// <returns>一个SQLiteCommand对象</returns>
        public static SQLiteCommand CreateCommand(string commandText, SQLiteConnection conn, SQLiteTransaction trans, SQLiteParameter[] param = null)
        {
            SQLiteCommand cmd = new SQLiteCommand(commandText, conn);
            if (param != null && param.Length > 0)
            {
                cmd.Parameters.AddRange(param);
            }
            cmd.Transaction = trans;
            return cmd;
        }
    }
}
