
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Y.ASIS.Models.Enums;

namespace Y.ASIS.App.Models
{
    /// <summary>
    /// 股道
    /// </summary>
    public class Track : EnumerableObject
    {
        private int id;
        /// <summary>
        /// 获取或设置编号
        /// </summary>
        public int Id
        {
            get { return id; }
            set { SetProperty(ref id, value); }
        }

        private int no;
        /// <summary>
        /// 获取或设置股道号
        /// </summary>
        public int No
        {
            get { return no; }
            set { SetProperty(ref no, value); }
        }

        private string name;
        /// <summary>
        /// 获取或设置股道名称
        /// </summary>
        public string Name
        {
            get { return name; }
            set { SetProperty(ref name, value); }
        }

        private TrackType type;
        /// <summary>
        /// 获取或设置股道类型
        /// </summary>
        public TrackType Type
        {
            get { return type; }
            set { SetProperty(ref type, value); }
        }

        private ObservableCollection<Position> positions;
        public ObservableCollection<Position> Positions
        {
            get { return positions; }
            set { SetProperty(ref positions, value); }
        }

        private bool isLinked;
        /// <summary>
        /// 获取或设置是否重联
        /// </summary>
        [JsonIgnore]
        public bool IsLinked
        {
            get { return isLinked; }
            set { SetProperty(ref isLinked, value); }
        }
    }

    /// <summary>
    /// 股道状态
    /// </summary>
    public class TrackState
    {
        public int TrackId { get; set; }

        //public IEnumerable<PositionState> PositionStates { get; set; }
        public IEnumerable<PositionStateNet> PositionStates { get; set; }
    }




}
