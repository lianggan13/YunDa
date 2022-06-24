using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Linq;
using Y.ASIS.Common.MVVMFoundation;
namespace Y.ASIS.App.Models
{
    /// <summary>
    /// 平台
    /// </summary>
    public class Platform : NotifyObjectBase
    {
        private int index;
        [JsonIgnore]
        public int Index
        {
            get { return index; }
            set { SetProperty(ref index, value); }
        }

        private ObservableCollection<int> doors;
        public ObservableCollection<int> Doors
        {
            get { return doors; }
            set
            {
                if (doors == null || value == null || !doors.SequenceEqual(value))
                {
                    doors = value;
                    OnPropertyChanged(nameof(Doors));
                }
            }
        }

        private int gangway;
        public int Gangway
        {
            get { return gangway; }
            set { SetProperty(ref gangway, value); }
        }

        private string passCount;
        /// <summary>
        /// 门禁客流计统计人数	::AsGlobalPV:State.Passcount[n]
        /// 如“1”、“-2”、“Error”、“Null”，负号代表出门人数大于进门人数，“Error”代表设备错误。
        /// </summary>
        public string PassCount
        {
            get { return passCount; }
            set { SetProperty(ref passCount, value); }
        }

        private ObservableCollection<Worker> workers;
        public ObservableCollection<Worker> Workers
        {
            get { return workers; }
            set
            {
                if (workers == null || value == null)
                {
                    workers = value;
                    OnPropertyChanged("Workers");
                }
                else if (!workers.SequenceEqual(value))
                {
                    for (int i = workers.Count - 1; i >= 0; i--)
                    {
                        if (!value.Contains(workers[i]))
                        {
                            workers.RemoveAt(i);
                        }
                    }
                    foreach (Worker worker in value)
                    {
                        if (!workers.Contains(worker))
                        {
                            workers.Add(worker);
                        }
                    }
                }
            }
        }
    }
}
