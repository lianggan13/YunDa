using System.Collections.Generic;

namespace Y.ASIS.Server.Models
{
    /// <summary>
    /// 平台信息
    /// </summary>
    public class Platform
    {
        /// <summary>
        /// 门状态 00=未知、通信失败等异常状态 01=使能状态下开 02=使能状态下关 03=未使能状态下开 04=未使能状态下关
        /// </summary>
        public List<int> Doors { get; set; } = new List<int>();

        /// <summary>
        /// 渡板状态
        /// </summary>
        public int Gangway { get; set; }

        /// <summary>
        /// 登顶人员计数
        /// </summary>
        public string PassCount { get; set; }

        /// <summary>
        /// 登顶人员信息
        /// </summary>
        public IReadOnlyList<Worker> Workers = new List<Worker>();
    }
}
