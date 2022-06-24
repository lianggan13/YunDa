using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Y.ASIS.App.Models
{
    public class ExternalSystem : EnumerableObject
    {
        private int id;
        public int Id
        {
            get { return id; }
            set { SetProperty(ref id, value); }
        }

        private string name;
        public string Name
        {
            get { return name; }
            set { SetProperty(ref name, value); }
        }

        private string authKey;
        public string AuthKey
        {
            get { return authKey; }
            set { SetProperty(ref authKey, value); }
        }

        private bool enable;
        public bool Enable
        {
            get { return enable; }
            set { SetProperty(ref enable, value); }
        }

        private bool connected;
        [JsonIgnore]
        public bool Connected
        {
            get { return connected; }
            set { SetProperty(ref connected, value); }
        }

        private string describe;
        public string Describe
        {
            get { return describe; }
            set { SetProperty(ref describe, value); }
        }

        private string pushAddress;
        public string PushAddress
        {
            get { return pushAddress; }
            set { SetProperty(ref pushAddress, value); }
        }
    }
}
