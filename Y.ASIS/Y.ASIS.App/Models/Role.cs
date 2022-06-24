using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Y.ASIS.App.Models
{
    public class Role : EnumerableObject
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

        private ObservableCollection<int> functions;
        public ObservableCollection<int> Functions
        {
            get { return functions; }
            set { SetProperty(ref functions, value); }
        }

        private string describe;
        public string Describe
        {
            get { return describe; }
            set { SetProperty(ref describe, value); }
        }
    }
}
