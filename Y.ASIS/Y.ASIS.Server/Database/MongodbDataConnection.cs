using MongoDB.Driver;
using System;

namespace Y.ASIS.Server.Database
{
    public class MongodbDataConnection : IDataConnection<IMongoDatabase>
    {
        private IMongoDatabase Database;
        private MongoClient client;

        public bool? CloseConnect()
        {
            return null;
        }

        public bool Connect(string Ip, string Port, string DataName)
        {
            try
            {
                client = new MongoClient($"mongodb://{Ip}:{Port}");
                Database = client.GetDatabase(DataName);
                return true;
            }
            catch (Exception e)
            {
                LogHelper.Error(e.Message + ":" + e.InnerException);
                return false;
            }
        }

        public IMongoDatabase GetConnect()
        {
            if (Database == null)
            {
                LogHelper.Info("返回的连接为NULL!");
                return null;
            }
            return Database;
        }
    }
}
