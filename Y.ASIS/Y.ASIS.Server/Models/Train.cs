using Y.ASIS.Models.Enums;

namespace Y.ASIS.Server.Models
{
    /// <summary>
    /// 车信息
    /// </summary>
    public class Train
    {
        /// <summary>
        /// 车号
        /// </summary>
        public string No { get; set; }

        /// <summary>
        /// 左受电弓状态
        /// </summary>
        public PantographState LeftPantograph { get; set; }

        /// <summary>
        /// 右受电弓状态
        /// </summary>
        public PantographState RightPantograph { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public string State { get; set; }

    }
}
