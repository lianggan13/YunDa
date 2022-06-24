using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace Y.ASIS.App.Database
{
    public sealed class SqliteConn
    {
        private bool _disposed;
        private static readonly Dictionary<string, SQLiteConnection> _connPool = new Dictionary<string, SQLiteConnection>();
        private static readonly Dictionary<string, ReaderWriterLock> _rwLock = new Dictionary<string, ReaderWriterLock>();
        private static SqliteConn _instance;

        private SqliteConn(string[] dbNames)
        {
            foreach (string db in dbNames)
            {
                AppendConnection(db);
            }
            Debug.WriteLine("Init finished.");
        }

        private static SQLiteConnection CreateConn(string path)
        {
            SQLiteConnection conn = new SQLiteConnection();
            try
            {
                SQLiteConnectionStringBuilder connStr = new SQLiteConnectionStringBuilder
                {
                    DataSource = path,
                };
                conn.ConnectionString = connStr.ToString();
                conn.Open();
                return conn;
            }
            catch (Exception e)
            {
                Debug.WriteLine("Connection create error:{0}", e.Message);
                return null;
            }
        }

        public void Dispose()
        {
            Dispose();
            GC.SuppressFinalize(this);
        }

        public void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    Debug.WriteLine("Closing local db connection...");
                    CloseConn();
                }
                _disposed = true;
            }
        }

        ~SqliteConn()
        {
            Dispose(false);
        }

        public void CloseConn()
        {
            foreach(KeyValuePair<string, SQLiteConnection> pair in _connPool)
            {
                string connName = pair.Key;
                SQLiteConnection conn = pair.Value;
                if(conn != null && conn.State != ConnectionState.Closed)
                {
                    try
                    {
                        conn.Close();
                        conn.Dispose();
                        conn = null;
                        Debug.WriteLine(string.Format("Connection {0} closed.", connName));
                    }catch(Exception e)
                    {
                        Debug.WriteLine(
                            string.Format("Serious error, cannot close db {0} connection:{1}", connName, e.Message));
                    }
                    finally
                    {
                        conn = null;
                    }
                }
            }
        }

        public static SqliteConn GetInstance(params string[] dbNames)
        {
            if(dbNames != null && _instance == null)
            {
                _instance = new SqliteConn(dbNames);
            }
            return _instance;
        }

        public SQLiteConnection GetConnection(string name)
        {
            SQLiteConnection conn = _connPool[name];
            try
            {
                if(conn != null)
                {
                    Debug.WriteLine("Try get lock.");
                    // 加锁
                    _rwLock[name].AcquireReaderLock(3000);
                    Debug.WriteLine("Get lock.");
                    return conn;
                }
            }
            catch(Exception e)
            {
                Debug.WriteLine(string.Format("Get connection error:{0}", e.Message));
            }
            return null;
        }

        public void ReleaseConn(string name)
        {
            try
            {
                // 释放锁
                Debug.WriteLine("Release lock.");
                _rwLock[name].ReleaseLock();
            }
            catch(Exception e)
            {
                Debug.WriteLine(string.Format("Release lock error:", e.Message));
            }
        }

        public void AppendConnection(string db)
        {
            string key = Path.GetFileNameWithoutExtension(db);
            if(!_disposed && !_connPool.ContainsKey(key))
            {
                string path = Path.GetFullPath(db);
                _rwLock.Add(key, new ReaderWriterLock());
                SQLiteConnection conn = CreateConn(path);
                _connPool.Add(key, conn);
            }
        }
    }
}
