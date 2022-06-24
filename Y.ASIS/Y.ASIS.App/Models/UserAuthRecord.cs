using System;
using System.Collections.ObjectModel;
using Y.ASIS.Models.Enums;

namespace Y.ASIS.App.Models
{
    public class UserAuthRecord : EnumerableObject
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

        private ObservableCollection<string> positions;
        public ObservableCollection<string> Positions
        {
            get { return positions; }
            set { SetProperty(ref positions, value); }
        }

        private SimpleUser user;
        public SimpleUser User
        {
            get { return user; }
            set { SetProperty(ref user, value); }
        }

        private IssueType issueType;
        public IssueType IssueType
        {
            get { return issueType; }
            set { SetProperty(ref issueType, value); }
        }

        private DateTime issueTime;
        public DateTime IssueTime
        {
            get { return issueTime; }
            set { SetProperty(ref issueTime, value); }
        }

        private DateTime? workTime;
        public DateTime? WorkTime
        {
            get { return workTime; }
            set { SetProperty(ref workTime, value); }
        }

        private DateTime? revokeTime;
        public DateTime? RevokeTime
        {
            get { return revokeTime; }
            set { SetProperty(ref revokeTime, value); }
        }
    }
}
