using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Y.ASIS.App.Models
{
    public class Title : EnumerableObject
    {
        private int id;
        public int Id
        {
            get { return id; }
            set { SetProperty(ref id, value); }
        }

        private string resourceKey;
        public string ResourceKey
        {
            get { return resourceKey; }
            set { SetProperty(ref resourceKey, value); }
        }

        private string content;
        public string Content
        {
            get { return content; }
            set { SetProperty(ref content, value); }
        }

        private string describe;
        public string Describe
        {
            get { return describe; }
            set { SetProperty(ref describe, value); }
        }
    }
}
