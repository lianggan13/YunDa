namespace Y.ASIS.Common.Models.Enums
{
    /// <summary>
    /// 项目类型
    /// </summary>
    public enum ProjectType
    {
        /// <summary>
        /// 国铁
        /// </summary>
        NationalRailway = 0,

        /// <summary>
        /// 国铁白色(高度定制化,版本太多了,后期需要版本管理)
        /// </summary>
        NationalRailway_BaiSe,

        /// <summary>
        /// 城轨1(无工具柜|含验电)
        /// </summary>
        CityRailway_1,
        /// <summary>
        /// 城轨2(无工具柜|无验电)
        /// </summary>
        CityRailway_2,
        /// <summary>
        /// 深12(无安全确认按钮,自动安全确认)
        /// </summary>
        Shenzhen12,
    }
}
