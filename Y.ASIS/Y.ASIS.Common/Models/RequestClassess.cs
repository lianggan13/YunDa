using System.Collections.Generic;

namespace Y.ASIS.Common.Models
{
    public class PositionIssueUsersParams
    {
        public int PositionId { get; set; }

        public IEnumerable<int> WorkerNos { get; set; }

        public IEnumerable<int> OperatorNos { get; set; }

        /// <summary>
        /// 巡检特权
        /// </summary>
        public bool? IsInspect { get; set; }

    }

    public class UpdateUserPhotoParams
    {
        public int UserId { get; set; }

        public string PhotoString { get; set; }
    }

    public class HandleWarningParams
    {
        public int WarningId { get; set; }

        public int UserId { get; set; }

        public string Remarks { get; set; }
    }
}
