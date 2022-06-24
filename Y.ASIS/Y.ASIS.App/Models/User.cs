using Newtonsoft.Json;
using System.Collections.ObjectModel;
using Y.ASIS.Common.MVVMFoundation;
namespace Y.ASIS.App.Models
{
    public class User : EnumerableObject
    {
        private int id;
        public int Id
        {
            get { return id; }
            set { SetProperty(ref id, value); }
        }

        private int no;
        public int No
        {
            get { return no; }
            set { SetProperty(ref no, value); }
        }

        private int cardNo;
        public int CardNo
        {
            get { return cardNo; }
            set { SetProperty(ref cardNo, value); }
        }

        private string name;
        public string Name
        {
            get { return name; }
            set { SetProperty(ref name, value); }
        }

        public string Password { get; set; }

        private string oldPassword;
        public string OldPassword
        {
            get { return oldPassword; }
            set { SetProperty(ref oldPassword, value); }
        }

        private string newPassword;
        public string NewPassword
        {
            get { return newPassword; }
            set { SetProperty(ref newPassword, value); }
        }

        private string confirmPassword;
        [JsonIgnore]
        public string ConfirmPassword
        {
            get { return confirmPassword; }
            set { SetProperty(ref confirmPassword, value); }
        }

        private string photoUrl;
        public string PhotoUrl
        {
            get { return photoUrl; }
            set { SetProperty(ref photoUrl, value); }
        }

        private int roleId;
        public int RoleId
        {
            get { return roleId; }
            set { SetProperty(ref roleId, value); }
        }

        private int userGroupId;
        public int UserGroupId
        {
            get { return userGroupId; }
            set { SetProperty(ref userGroupId, value); }
        }

        private bool allowUpdate;
        public bool AllowUpdate
        {
            get { return allowUpdate; }
            set { SetProperty(ref allowUpdate, value); }
        }

        private ObservableCollection<int> functions;
        [JsonIgnore]
        public ObservableCollection<int> Functions
        {
            get { return functions; }
            set { SetProperty(ref functions, value); }
        }

        private bool enable;
        [JsonIgnore]
        public bool Enable
        {
            get { return enable; }
            set { SetProperty(ref enable, value); }
        }
    }

    public class SimpleUser : NotifyObjectBase
    {
        private int id;
        public int Id
        {
            get { return id; }
            set { SetProperty(ref id, value); }
        }

        private int no;
        public int No
        {
            get { return no; }
            set { SetProperty(ref no, value); }
        }

        private int cardNo;
        public int CardNo
        {
            get { return cardNo; }
            set { SetProperty(ref cardNo, value); }
        }

        private string name;
        public string Name
        {
            get { return name; }
            set { SetProperty(ref name, value); }
        }

        private int userGroupId;
        public int UserGroupId
        {
            get { return userGroupId; }
            set { SetProperty(ref userGroupId, value); }
        }

        private string userGroup;
        public string UserGroup
        {
            get { return userGroup; }
            set { SetProperty(ref userGroup, value); }
        }
    }
}
