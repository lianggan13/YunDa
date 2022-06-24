using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using Y.ASIS.App.Common;
using Y.ASIS.App.Communication;
using Y.ASIS.App.Database;
using Y.ASIS.App.Models;
using Y.ASIS.Common.Models;

namespace Y.ASIS.App.ViewModels
{
    class ConfigViewModel : ViewModelBase
    {

        private ObservableCollection<Title> titles;
        public ObservableCollection<Title> Titles
        {
            get { return titles; }
            set { SetProperty(ref titles, value); }
        }

        private ObservableCollection<UserGroup> groups;
        public ObservableCollection<UserGroup> Groups
        {
            get { return groups; }
            set { SetProperty(ref groups, value); }
        }

        private ObservableCollection<Function> functions;
        public ObservableCollection<Function> Functions
        {
            get { return functions; }
            set { SetProperty(ref functions, value); }
        }

        private ObservableCollection<Role> roles;
        public ObservableCollection<Role> Roles
        {
            get { return roles; }
            set { SetProperty(ref roles, value); }
        }

        private ObservableCollection<User> users;
        public ObservableCollection<User> Users
        {
            get { return users; }
            set { SetProperty(ref users, value); }
        }

        public ConfigViewModel()
        {
            RefreshTitles();
            RefreshFunctions();
            RefreshUserGroups();
        }

        public void RefreshTitles()
        {
            try
            {
                IEnumerable<Title> titles = AppDatabase.Instance.GetTitles();
                Titles = new ObservableCollection<Title>(titles);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message, ex);
            }
        }

        public void RefreshUserGroups(bool async = false)
        {
            UserGroupListRequest request = new UserGroupListRequest();
            void callback(ResponseData<IEnumerable<UserGroup>> resp)
            {
                if (resp != null && resp.IsSuccess)
                {
                    Groups = new ObservableCollection<UserGroup>(resp.Data);
                }
            };
            if (async)
            {
                request.RequestAsync<ResponseData<IEnumerable<UserGroup>>>(resp =>
                {
                    callback(resp);
                });
            }
            else
            {
                ResponseData<IEnumerable<UserGroup>> resp = request.Request<ResponseData<IEnumerable<UserGroup>>>();
                callback(resp);
            }
        }

        public void RefreshFunctions()
        {
            FunctionListRequest request = new FunctionListRequest();
            ResponseData<IEnumerable<Function>> resp = request.Request<ResponseData<IEnumerable<Function>>>();
            if (resp != null && resp.IsSuccess)
            {
                Functions = new ObservableCollection<Function>(resp.Data);
                AppGlobal.Instance.SetData("Functions", Functions);
            }
        }

        public void RefreshRoles(bool async = false)
        {
            RoleListRequest request = new RoleListRequest();

            void callback(ResponseData<IEnumerable<Role>> resp)
            {
                if (resp != null && resp.IsSuccess)
                {
                    Roles = new ObservableCollection<Role>(resp.Data);
                }
            }
            if (async)
            {
                request.RequestAsync<ResponseData<IEnumerable<Role>>>(resp =>
                {
                    callback(resp);
                });
            }
            else
            {
                ResponseData<IEnumerable<Role>> resp = request.Request<ResponseData<IEnumerable<Role>>>();
                callback(resp);
            }
        }

        public void RefreshUsers(bool async = false)
        {
            UserListRequest request = new UserListRequest();

            void callback(ResponseData<IEnumerable<User>> resp)
            {
                if (resp != null && resp.IsSuccess)
                {
                    Users = new ObservableCollection<User>(resp.Data);
                }
            }

            if (async)
            {
                request.RequestAsync<ResponseData<IEnumerable<User>>>(resp =>
                {
                    callback(resp);
                });
            }
            else
            {
                ResponseData<IEnumerable<User>> resp = request.Request<ResponseData<IEnumerable<User>>>();
                callback(resp);
            }
        }

        public void InvokeMethod(string methodName)
        {
            if (methodName == null)
            {
                return;
            }
            Type type = typeof(ConfigViewModel);
            MethodInfo method = type.GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            if (method != null)
            {
                ParameterInfo[] parameters = method.GetParameters();
                if (parameters.Count() == 1 && parameters.First().Name == "async")
                {
                    method.Invoke(this, new object[] { true });
                }
                else
                {
                    method.Invoke(this, null);
                }
            }
        }
    }
}
