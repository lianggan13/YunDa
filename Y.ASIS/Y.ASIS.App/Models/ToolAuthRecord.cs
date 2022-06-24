using System;
using System.Collections.ObjectModel;

namespace Y.ASIS.App.Models
{
    class ToolAuthRecord : EnumerableObject
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

        private ObservableCollection<string> tools;
        public ObservableCollection<string> Tools
        {
            get { return tools; }
            set { SetProperty(ref tools, value); }
        }

        private SimpleUser user;
        public SimpleUser User
        {
            get { return user; }
            set { SetProperty(ref user, value); }
        }

        private DateTime issueTime;
        public DateTime IssueTime
        {
            get { return issueTime; }
            set { SetProperty(ref issueTime, value); }
        }

        private DateTime? revokeTime;
        public DateTime? RevokeTime
        {
            get { return revokeTime; }
            set { SetProperty(ref revokeTime, value); }
        }

        private DateTime? returnTime;
        public DateTime? ReturnTime
        {
            get { return returnTime; }
            set { SetProperty(ref returnTime, value); }
        }
    }
}
