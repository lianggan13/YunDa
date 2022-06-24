using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Y.ASIS.App.Common;
using Y.ASIS.App.Communication;
using Y.ASIS.App.Ctls.Controls;
using Y.ASIS.App.Models;
using Y.ASIS.App.Services;
using Y.ASIS.App.Windows;
using Y.ASIS.Common.ExtensionMethod;
using Y.ASIS.Common.Models;
using Y.ASIS.Common.MVVMFoundation;

namespace Y.ASIS.App.ViewModels
{
    class AuthorityManagerViewModel : ViewModelBase
    {
        private IEnumerable<UserGroup> userGroups;
        private IEnumerable<User> users;
        private List<UserGroup<User>> containsUserGroups;
        private object locker = new object();

        private bool? isInspect = false;
        public bool? IsInspect
        {
            get { return isInspect; }
            set
            {
                isInspect = value;
                OnPropertyChanged(nameof(IsInspect));
            }
        }

        private Track currentTrack;
        public Track CurrentTrack
        {
            get { return currentTrack; }
            set { SetProperty(ref currentTrack, value); }
        }

        private Position currentPosition;
        public Position CurrentPosition
        {
            get { return currentPosition; }
            set
            {
                if (currentPosition != value)
                {
                    currentPosition = value;
                    OnPropertyChanged("CurrentPosition");

                    if (currentPosition != null)
                    {
                        RefreshOperatorsAsync(currentPosition);
                        RefreshWorkersAsync(currentPosition);
                    }
                }
            }
        }

        private ObservableCollection<Track> tracks;
        public ObservableCollection<Track> Tracks
        {
            get { return tracks; }
            set { SetProperty(ref tracks, value); }
        }

        private ObservableCollection<UserGroup<Worker>> issuedOperators;
        public ObservableCollection<UserGroup<Worker>> IssuedOperators
        {
            get { return issuedOperators; }
            set { SetProperty(ref issuedOperators, value); }
        }

        private ObservableCollection<UserGroup<Worker>> issuedWorkers;
        public ObservableCollection<UserGroup<Worker>> IssuedWorkers
        {
            get { return issuedWorkers; }
            set { SetProperty(ref issuedWorkers, value); }
        }

        private ObservableCollection<UserGroup<User>> unIssueOperators;
        public ObservableCollection<UserGroup<User>> UnIssueOperators
        {
            get { return unIssueOperators; }
            set { SetProperty(ref unIssueOperators, value); }
        }

        private ObservableCollection<UserGroup<User>> unIssueWorkers;
        public ObservableCollection<UserGroup<User>> UnIssueWorkers
        {
            get { return unIssueWorkers; }
            set { SetProperty(ref unIssueWorkers, value); }
        }

        public RelayCommand IssueUsersCommand { get; set; }

        public RelayCommand RevokeUsersCommand { get; set; }

        public RelayCommand InspectCommand => new RelayCommand(Inspect);

        public AuthorityManagerViewModel(IEnumerable<Track> tracks)
        {
            //IEnumerable<Track> clone = tracks.JsonDeepCopy();
            CurrentTrack = tracks.FirstOrDefault(i => i.IsSelected);
            Tracks = new ObservableCollection<Track>(tracks);
            IssueUsersCommand = new RelayCommand(IssueUsers, CanIssueUsers);
            RevokeUsersCommand = new RelayCommand(RevokeUsers, CanRevokeUsers);
            GetUsersAsync();
            GetUserGroupsAsync();
        }


        private async void IssueUsers()
        {
            GetCheckedUserNos(UnIssueOperators, UnIssueWorkers,
                out List<int> operatorNos, out List<int> workerNos);

            var issedOptNos = issuedOperators.SelectMany(u => u.Users).Select(u => u.No);
            var issedWorkerNos = issuedWorkers.SelectMany(u => u.Users).Select(u => u.No);
            var curWin = WindowManager.FindWindwByType(typeof(AuthorityManagerWindow));
            await LoadingWindow.Show(curWin, () =>
            {
                AuthorityService.RequestIssueUsers(currentPosition, operatorNos, workerNos, issedOptNos, issedWorkerNos, IsInspect, resp =>
                 {
                     if (resp != null && resp.Data)
                     {
                         AppDispatcherInvoker(() =>
                         {
                             MessageWindow.Show("权限下发成功");
                         });
                         RefreshOperatorsAsync(CurrentPosition);
                         RefreshWorkersAsync(CurrentPosition);
                     }
                     else
                     {
                         AppDispatcherInvoker(() =>
                         {
                             MessageWindow.Show("权限下发失败\r\n请检查通信状态或人员信息");
                         });
                     }
                 });
            });
        }


        private bool CanIssueUsers(object _)
        {
            bool con1 = UnIssueOperators?.Any(i => i.Users.Any(j => j.IsChecked)) == true;
            bool con2 = UnIssueWorkers?.Any(i => i.Users.Any(j => j.IsChecked)) == true;
            bool con = con1 || con2;
            return con;
        }


        private async void RevokeUsers()
        {
            GetCheckedWorkerNos(IssuedOperators, IssuedWorkers,
                out List<int> operatorNos, out List<int> workerNos);

            var curWin = WindowManager.FindWindwByType(typeof(AuthorityManagerWindow));
            await LoadingWindow.Show(curWin, () =>
            {
                AuthorityService.RequestRevokeUsers(CurrentPosition, operatorNos, workerNos, resp =>
                {
                    if (resp != null && resp.Data)
                    {
                        AppDispatcherInvoker(() =>
                        {
                            MessageWindow.Show("权限撤销成功");
                        });
                        RefreshOperatorsAsync(currentPosition);
                        RefreshWorkersAsync(currentPosition);
                    }
                    else
                    {
                        AppDispatcherInvoker(() =>
                        {
                            MessageWindow.Show("权限撤销失败\r\n请检查通信状态或人员信息");
                        });
                    }
                });
            });
        }

        private bool CanRevokeUsers(object _)
        {
            bool con1 = IssuedOperators?.Any(i => i.Users.Any(j => j.IsChecked)) == true;
            bool con2 = IssuedWorkers?.Any(i => i.Users.Any(j => j.IsChecked)) == true;
            bool con = con1 || con2;
            return con;
        }

        private void Inspect(object param = null)
        {
            if (isInspect == true)
            {
                ResetIsChecked(issuedWorkers);
                ResetIsChecked(unIssueWorkers);
            }
        }

        private void ResetIsChecked<T>(IEnumerable<UserGroup<T>> issuedWorkers)
        {
            if (issuedWorkers?.Count() > 0)
            {
                foreach (UserGroup<T> w in issuedWorkers)
                {
                    w.IsChecked = false;
                    if (w.Users == null)
                        continue;
                    foreach (var u in w.Users)
                    {
                        (u as EnumerableObject).IsChecked = false;
                    }
                }
            }
        }

        private void GetCheckedUserNos(IEnumerable<UserGroup<User>> operators, IEnumerable<UserGroup<User>> workers,
                                       out List<int> operatorNos, out List<int> workerNos)
        {
            operatorNos = operators.SelectMany(g => g.Users)
                                 .Where(u => u.IsChecked)
                                 .Select(u => u.No).ToList();

            workerNos = workers.SelectMany(g => g.Users)
                                  .Where(u => u.IsChecked)
                                  .Select(u => u.No).ToList();

        }

        private void GetCheckedWorkerNos(IEnumerable<UserGroup<Worker>> operators, IEnumerable<UserGroup<Worker>> workers,
                                      out List<int> operatorNos, out List<int> workerNos)
        {
            operatorNos = operators.SelectMany(g => g.Users)
                                 .Where(u => u.IsChecked)
                                 .Select(u => u.No).ToList();

            workerNos = workers.SelectMany(g => g.Users)
                                  .Where(u => u.IsChecked)
                                  .Select(u => u.No).ToList();

        }

        private void GetUsersAsync()
        {
            UserListRequest request = new UserListRequest();
            request.RequestAsync<ResponseData<IEnumerable<User>>>(resp =>
            {
                if (resp != null && resp.Data != null)
                {
                    users = resp.Data;
                }
            });
        }

        private void GetUserGroupsAsync()
        {
            UserGroupListRequest request = new UserGroupListRequest();
            request.RequestAsync<ResponseData<IEnumerable<UserGroup>>>(resp =>
            {
                if (resp != null && resp.Data != null)
                {
                    userGroups = resp.Data;
                }
            });
        }

        private void RefreshOperatorsAsync(Position position)
        {
            PositionIssuedOperatorsRequest request = new PositionIssuedOperatorsRequest(position.Id);
            request.RequestAsync<ResponseData<IEnumerable<Worker>>>(resp =>
            {
                if (resp != null && resp.Data != null)
                {
                    List<UserGroup<Worker>> list = new List<UserGroup<Worker>>();
                    List<Worker> added = new List<Worker>(resp.Data.Count());
                    foreach (Worker worker in resp.Data)
                    {
                        if (!list.Any(i => i.Id == worker.UserGroupId)
                            && userGroups.FirstOrDefault(i => i.Id == worker.UserGroupId) is UserGroup group)
                        {
                            UserGroup<Worker> g = group.ToUserGroup<Worker>();
                            g.Users.Clear();
                            list.Add(g);
                        }
                        UserGroup<Worker> get = list.FirstOrDefault(i => i.Id == worker.UserGroupId);
                        if (get != null)
                        {
                            get.Users.Add(worker);
                            added.Add(worker);
                        }
                    }
                    IEnumerable<Worker> remain = resp.Data.Where(j => !added.Contains(j));
                    if (remain.Any())
                    {
                        UserGroup<Worker> unGrouped = new UserGroup<Worker>()
                        {
                            Name = "未分组"
                        };
                        unGrouped.Users = new ObservableCollection<Worker>(remain);
                        list.Add(unGrouped);
                    }
                    list.ForEach(i => i.IsChecked = true);
                    IssuedOperators = new ObservableCollection<UserGroup<Worker>>(list);

                    UnIssueOperators = new ObservableCollection<UserGroup<User>>(GetUnIssueUsers(resp.Data));
                }
            });
        }

        private void RefreshWorkersAsync(Position position)
        {
            PositionIssuedWorkersRequest request = new PositionIssuedWorkersRequest(position.Id);
            request.RequestAsync<ResponseData<IEnumerable<Worker>>>(resp =>
            {
                if (resp != null && resp.Data != null)
                {
                    List<UserGroup<Worker>> list = new List<UserGroup<Worker>>();
                    List<Worker> added = new List<Worker>(resp.Data.Count());
                    foreach (Worker worker in resp.Data)
                    {
                        if (!list.Any(i => i.Id == worker.UserGroupId)
                            && userGroups.FirstOrDefault(i => i.Id == worker.UserGroupId) is UserGroup group)
                        {
                            UserGroup<Worker> g = group.ToUserGroup<Worker>();
                            g.Users.Clear();
                            list.Add(g);
                        }
                        UserGroup<Worker> get = list.FirstOrDefault(i => i.Id == worker.UserGroupId);
                        if (get != null)
                        {
                            get.Users.Add(worker);
                            added.Add(worker);
                        }
                    }
                    IEnumerable<Worker> remain = resp.Data.Where(j => !added.Contains(j));
                    if (remain.Any())
                    {
                        UserGroup<Worker> unGrouped = new UserGroup<Worker>()
                        {
                            Name = "未分组"
                        };
                        unGrouped.Users = new ObservableCollection<Worker>(remain);
                        list.Add(unGrouped);
                    }
                    list.ForEach(i => i.IsChecked = true);
                    IssuedWorkers = new ObservableCollection<UserGroup<Worker>>(list);

                    UnIssueWorkers = new ObservableCollection<UserGroup<User>>(GetUnIssueUsers(resp.Data));
                }
            });
        }

        private IEnumerable<UserGroup<User>> GetUnIssueUsers(IEnumerable<Worker> issued)
        {
            List<UserGroup<User>> groups = new List<UserGroup<User>>();
            lock (locker)
            {
                if (containsUserGroups == null)
                {
                    containsUserGroups = new List<UserGroup<User>>();
                    List<User> added = new List<User>(users.Count());
                    foreach (User user in users)
                    {
                        if (!containsUserGroups.Any(i => i.Id == user.UserGroupId)
                            && userGroups.FirstOrDefault(i => i.Id == user.UserGroupId) is UserGroup group)
                        {
                            UserGroup<User> g = group.ToUserGroup<User>();
                            g.Users.Clear();
                            containsUserGroups.Add(g);
                        }
                        UserGroup<User> get = containsUserGroups.FirstOrDefault(i => i.Id == user.UserGroupId);
                        if (get != null)
                        {
                            get.Users.Add(user);
                            added.Add(user);
                        }
                    }
                    IEnumerable<User> remain = users.Where(j => !added.Contains(j));
                    if (remain.Any())
                    {
                        UserGroup<User> unGrouped = new UserGroup<User>()
                        {
                            Name = "未分组"
                        };
                        unGrouped.Users = new ObservableCollection<User>(remain);
                        containsUserGroups.Add(unGrouped);
                    }
                }
            }
            groups = containsUserGroups.JsonDeepCopy();
            groups.ForEach(i =>
            {
                i.Users.ForEach(j =>
                {
                    j.Enable = !issued.Any(k => k.No == j.No);
                });
            });
            return groups;
        }
    }
}
