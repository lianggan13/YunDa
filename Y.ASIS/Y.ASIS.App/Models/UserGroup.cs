using System.Collections.ObjectModel;

namespace Y.ASIS.App.Models
{
    public class UserGroup : EnumerableObject
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

        public UserGroup<T> ToUserGroup<T>()
        {
            return new UserGroup<T>()
            {
                Id = Id,
                Name = Name
            };
        }
    }

    public class UserGroup<T> : UserGroup
    {
        private ObservableCollection<T> users = new ObservableCollection<T>();
        public ObservableCollection<T> Users
        {
            get { return users; }
            set { SetProperty(ref users, value); }
        }

        public UserGroup()
        {
            PropertyChanged += UserGroup_PropertyChanged;
        }

        private void UserGroup_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(IsChecked))
            {
                foreach (T u in Users)
                {
                    var obj = u as EnumerableObject;
                    if (obj != null)
                    {
                        obj.IsChecked = IsChecked;
                    }
                }
            }
        }
    }
}
