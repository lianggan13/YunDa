using System;

namespace Y.ASIS.Server.Models
{
    /// <summary>
    /// 登顶人员
    /// </summary>
    public class Worker
    {
        /// <summary>
        /// 人员工号
        /// </summary>
        public int No { get; set; }

        /// <summary>
        /// 人员姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 班组编号
        /// </summary>
        public int UserGroupId { get; set; }

        /// <summary>
        /// 登顶时间
        /// </summary>
        public DateTime Time { get; set; }
    }
}
