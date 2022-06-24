using Newtonsoft.Json;
using Y.ASIS.Common.MVVMFoundation;

namespace Y.ASIS.App.Models
{
    public class EnumerableObject : NotifyObjectBase
    {
        private int viewIndex;
        public int ViewIndex
        {
            get { return viewIndex; }
            set { SetProperty(ref viewIndex, value); }
        }

        private bool isChecked;
        [JsonIgnore]
        public bool IsChecked
        {
            get { return isChecked; }
            set { SetProperty(ref isChecked, value); }
        }

        private bool isSelected;
        [JsonIgnore]
        public bool IsSelected
        {
            get { return isSelected; }
            set { SetProperty(ref isSelected, value); }
        }
    }
}
