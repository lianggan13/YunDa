using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Y.ASIS.App.Communication;
using Y.ASIS.App.Models;
using Y.ASIS.App.Windows;
using Y.ASIS.Common.Models;
using Y.ASIS.Common.Models.Enums;

namespace Y.ASIS.App.Services
{
    public class AuthorityService
    {
        public static void RequestIssueUsers(Position pos, List<int> operatorNos, List<int> workerNos, IEnumerable<int> issWorkerNos, bool? isInspect,
            Action<ResponseData<bool>> callback
            )
        {
            if (AppGlobal.Instance.Project == ProjectType.NationalRailway_BaiSe)
            {
                int max = 5;
                if (workerNos.Count + issWorkerNos?.Count() > max)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        MessageWindow.Show($"权限下发失败\r\n作业人员数量超过{max}个"); // NationalRailway_BaiSe
                    });
                    return;
                }

                isInspect = false;
                if (operatorNos.Count > 0 && !PositionService.NoElec(pos))
                {
                    workerNos.AddRange(operatorNos);  // elec of authority append operator of authority
                    workerNos = workerNos.Distinct().ToList();
                }

                PositionIssueUsersRequest request = new PositionIssueUsersRequest(pos.Id, operatorNos, workerNos, isInspect);
                request.RequestAsync<ResponseData<bool>>(resp =>
                {
                    callback?.Invoke(resp);
                });
            }
        }


        public static void RequestRevokeUsers(Position pos, List<int> operatorNos, List<int> workerNos,
          Action<ResponseData<bool>> callback
          )
        {
            PositionRevokeUsersRequest request = new PositionRevokeUsersRequest(pos.Id, operatorNos, workerNos);
            request.RequestAsync<ResponseData<bool>>(resp =>
            {
                callback?.Invoke(resp);
            });
        }

    }
}
