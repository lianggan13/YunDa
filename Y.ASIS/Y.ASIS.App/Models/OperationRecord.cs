using System;

namespace Y.ASIS.App.Models
{
    class OperationRecord : EnumerableObject
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

        private SimpleUser user;
        public SimpleUser User
        {
            get { return user; }
            set { SetProperty(ref user, value); }
        }

        private int operationCode;
        public int OperationCode
        {
            get { return operationCode; }
            set { SetProperty(ref operationCode, value); }
        }

        private DateTime time;
        public DateTime Time
        {
            get { return time; }
            set { SetProperty(ref time, value); }
        }
    }
}
