using System;

namespace Y.ASIS.App.Models
{
    public class Record
    {
        /// <summary>
        /// 列位 Index
        /// </summary>
        public int Index { get; set; }

        public int TrackNo { get; set; }

        public DateTime Time { get; set; }
    }

    public class FaultRecord : Record
    {

        public string FaultCode { get; set; }
    }

}
