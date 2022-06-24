using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using Y.ASIS.App.Models;

namespace Y.ASIS.App.Database
{
    class AppDatabase : DbBase
    {
        private static AppDatabase instance;
        private readonly static object locker = new object();

        private const string DefaultDatabaseName = "App.db";

        private const string SQL_CREATE_USER_TABLE = "CREATE TABLE IF NOT EXISTS Title(Id INTEGER PRIMARY KEY AUTOINCREMENT, ResourceKey VARCHAR(32), Content VARCHAR(256), Describe VARCHAR(256));";

        private const string SQL_User_TABLE_INIT_Data = "INSERT INTO Title(ResourceKey, Content, Describe) SELECT \'TitleText\', \'检修作业安全联锁系统\', \'主界面大标题\' WHERE NOT EXISTS (SELECT 1 FROM Title WHERE ResourceKey=\'TitleText\')";

        private const string SQL_SELECT_ALL_PREFIX = "SELECT * FROM ";
        private const string SQL_SELECT_COUNT_PREFIX = "SELECT COUNT(*) FROM ";

        public static AppDatabase Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (locker)
                    {
                        if (instance == null)
                        {
                            instance = new AppDatabase();
                        }
                    }
                }
                return instance;
            }
        }

        private AppDatabase()
            :base(DefaultDatabaseName)
        {

        }

        protected override void CreateTables()
        {
            ExecuteNonQueryCommand(SQL_CREATE_USER_TABLE);
        }

        protected override void CheckFields()
        {
            
        }

        protected override void CheckData()
        {
            ExecuteNonQueryCommand(SQL_User_TABLE_INIT_Data);
        }

        public List<Title> GetTitles()
        {
            List<Title> titles = new List<Title>();
            string cmd = SQL_SELECT_ALL_PREFIX + "Title";
            DataTable dt = ExecuteQueryCommand(cmd);
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    Title title = new Title()
                    {
                        Id = Convert.ToInt32(row["Id"]),
                        ResourceKey = Convert.ToString(row["ResourceKey"]),
                        Content = Convert.ToString(row["Content"]),
                        Describe = Convert.ToString(row["Describe"])
                    };
                    titles.Add(title);
                }
            }
            return titles;
        }

        public void UpdateTitle(Title title)
        {
            string cmd = "UPDATE Title SET Content=@cont,Describe=@desc WHERE Id=@id";
            SQLiteParameter[] parameters = new SQLiteParameter[]
            {
                    new SQLiteParameter("@id", title.Id),
                    new SQLiteParameter("@cont", title.Content),
                    new SQLiteParameter("@desc", title.Describe)
            };
            ExecuteNonQueryCommand(cmd, parameters);
        }
    }
}
