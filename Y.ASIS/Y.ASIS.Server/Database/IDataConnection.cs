namespace Y.ASIS.Server.Database
{
    interface IDataConnection<T>
    {
        /// <summary>
        /// 创建数据库连接
        /// </summary>
        /// <param name="Ip">要连接数据库的IP</param>
        /// <param name="Port">要连接数据库的端口</param>
        /// <param name="DataName">要连接的库名称</param>
        /// <returns>是否链接成功</returns>
        bool Connect(string Ip, string Port, string DataName);

        /// <summary>
        /// 返回当前连接
        /// </summary>
        /// <returns>返回当前链接,如未连接成功返回NULL</returns>
        T GetConnect();

        /// <summary>
        /// 关闭当前连接
        /// </summary>
        /// <returns>是否关闭成功,不需要关闭返回NULL</returns>
        bool? CloseConnect();
    }
}
