using System;

namespace Y.ASIS.App.Models
{
    public class FaultMessages : EnumerableObject
    {
        private int id;
        public int Id
        {
            get { return id; }
            set { SetProperty(ref id, value); }
        }

        private string track;
        public string Track
        {
            get { return track; }
            set { SetProperty(ref track, value); }
        }

        private int faultCode;
        public int FaultCode
        {
            get { return faultCode; }
            set { SetProperty(ref faultCode, value); }
        }

        private DateTime time;
        public DateTime Time
        {
            get { return time; }
            set { SetProperty(ref time, value); }
        }

        private string handledBy;
        public string HandledBy
        {
            get { return handledBy; }
            set { SetProperty(ref handledBy, value); }
        }

        private string handledUser;
        public string HandledUser
        {
            get { return handledUser; }
            set { SetProperty(ref handledUser, value); }
        }

        private DateTime? handledTime;
        public DateTime? HandledTime
        {
            get { return handledTime; }
            set { SetProperty(ref handledTime, value); }
        }

        private string remarks;
        public string Remarks
        {
            get { return remarks; }
            set { SetProperty(ref remarks, value); }
        }
    }
}
