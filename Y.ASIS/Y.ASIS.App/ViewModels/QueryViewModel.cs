using System.Collections.Generic;
using System.Collections.ObjectModel;
using Y.ASIS.App.Models;
using Y.ASIS.Common.ExtensionMethod;
using Y.ASIS.Common.Models.Enums;

namespace Y.ASIS.App.ViewModels
{
    class QueryViewModel : ViewModelBase
    {
        private ObservableCollection<Track> tracks;
        public ObservableCollection<Track> Tracks
        {
            get { return tracks; }
            set { SetProperty(ref tracks, value); }
        }

        #region 故障查询部分
        private Dictionary<string, int> faultTypeCode;
        public Dictionary<string, int> FaultTypeCode
        {
            get { return faultTypeCode; }
            set { SetProperty(ref faultTypeCode, value); }
        }

        private Dictionary<string, int> operationTypeCode;
        public Dictionary<string, int> OperationTypeCode
        {
            get { return operationTypeCode; }
            set { SetProperty(ref operationTypeCode, value); }
        }
        #endregion

        public QueryViewModel(IEnumerable<Track> tracks)
        {
            Tracks = new ObservableCollection<Track>(tracks);
            FaultTypeCode = EnumExt.GetDic<PLCFaultCode>();
            OperationTypeCode = EnumExt.GetDic<PLCOperateCode>();
        }
    }
}
