using Newtonsoft.Json;
using System;

namespace Y.ASIS.App.Models
{
    /// <summary>
    /// 工人
    /// </summary>
    public class Worker : EnumerableObject
    {
        private int no;
        public int No
        {
            get { return no; }
            set { SetProperty(ref no, value); }
        }

        private string name;
        public string Name
        {
            get { return name; }
            set { SetProperty(ref name, value); }
        }

        private DateTime time;
        public DateTime Time
        {
            get { return time; }
            set { SetProperty(ref time, value); }
        }

        private int userGroupId;
        public int UserGroupId
        {
            get { return userGroupId; }
            set { SetProperty(ref userGroupId, value); }
        }

        private bool working;
        [JsonIgnore]
        public bool Working
        {
            get { return working; }
            set { SetProperty(ref working, value); }
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Worker worker))
            {
                return false;
            }
            return No == worker.No
                && Name == worker.Name
                && Time == worker.Time;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
