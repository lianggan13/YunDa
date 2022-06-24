using MongoDB.Driver;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using Y.ASIS.Common.Models.Enums;
using Y.ASIS.Models.Enums;
using Y.ASIS.Server.Database;
using Y.ASIS.Server.Models;

namespace Y.ASIS.Server.Services
{
    public static class PositionService
    {
        public static List<int> RevokeOptCode = new List<int>()
        {
            (int)PLCOperateCode.断电流程完成清除操作权限,
            (int)PLCOperateCode.送电流程完成清除操作权限,
        };

        static PositionService()
        {
        }

        public static void AddPositionToUsers(int posId, IEnumerable<int> userNos)
        {
            IterateUser(userNos, (user) =>
            {
                if (!user.PositionIds.Contains(posId))
                {
                    user.PositionIds.Add(posId);
                }

                DataProvider.Instance.UpdateUserAllowUpdateState(user);
            });
        }

        public static void RemovePositionToUsers(int posId, IEnumerable<int> userNos)
        {
            IterateUser(userNos, (user) =>
            {
                if (user.PositionIds.Contains(posId))
                {
                    user.PositionIds.Remove(posId);
                }
                DataProvider.Instance.UpdateUserAllowUpdateState(user);
            });
        }

        public static void ClearPositionToUsers(IEnumerable<int> userNos)
        {
            IterateUser(userNos, (user) =>
            {
                user.PositionIds.Clear();
                DataProvider.Instance.UpdateUserAllowUpdateState(user);
            });
        }

        private static void IterateUser(IEnumerable<int> userNos, Action<User> action)
        {
            foreach (var no in userNos)
            {
                var user = DataProvider.Instance.UserList.FirstOrDefault(u => u.No == no);
                if (user != null)
                {
                    action?.Invoke(user);
                }
            }
        }


        public static string GetTrainNo(Position pos, int trainIndex)
        {
            List<VideoStream> videos = new List<VideoStream>();
            videos.AddRange(pos.Videos);
            videos.AddRange(pos.ExtraVideos);

            var vs = videos.FirstOrDefault(v =>
            {
                if (v.Type != VideoType.TrainNo)
                    return false;
                JObject jobj = JObject.Parse(v.Extension);
                if (jobj.TryGetValue("TrainIndex", out JToken jt2))
                {
                    int index = jt2.ToObject<int>();
                    bool s = index == trainIndex;
                    return s;
                }
                else
                    return false;
            });

            string no = "";

            if (vs != null)
                no = AlgorithmService.Instance.DetectTrainNo(pos.ID, vs.Channel);

            return no;
        }

    }
}
