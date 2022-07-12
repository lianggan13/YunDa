using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Y.ASIS.Common.Models.Enums;
using Y.ASIS.Server.Database;
using Y.ASIS.Server.Device;
using Y.ASIS.Server.Device.Speaker;
using Y.ASIS.Server.Services;
using Y.ASIS.Server.Services.CameraService;
using Y.ASIS.Server.Services.IPSpeaker;

public static class Tester
{
    public static void TestAlgorithm()
    {
        var pos1 = DataProvider.Instance.TrackList.SelectMany(t => t.Positions).ElementAt(0);
        string no1 = PositionService.GetTrainNo(pos1, 0);
        string no2 = PositionService.GetTrainNo(pos1, 1);
        Console.ReadKey();
    }


    public static void TestLinkVideo()
    {
        //HIKNVRService.Switch(2);
        //HIKNVRService.Switch(HIKNVR.SDK.CameraLayout.IX, new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 });
        //HIKNVRService.SwitchAll();

        var pos1 = DataProvider.Instance.TrackList.SelectMany(t => t.Positions).ElementAt(0);
        HIKNVRService.LinkDoorVideo(pos1, 0, null);
    }


    public static void TestUser()
    {
        int posId = 2;
        var users = DataProvider.Instance.UserList.Where(u => u.PositionIds.Contains(posId));
        PositionService.RemovePositionToUsers(posId, users?.Select(u => u.No));

        var user = DataProvider.Instance.UserList.FirstOrDefault(u => u.No == 290017);
        // position issue
        user.PositionIds.Add(1);
        user.PositionIds.Add(2);
        user.PositionIds.Add(3);

        // position revoke

        //if (user.PositionIds.Count > 0)
        //{
        //    DataProvider.Instance.UpdateUserAllowUpdateState(new List<int>() { user.No }, false);
        //}
        //else
        //{
        //    DataProvider.Instance.UpdateUserAllowUpdateState(new List<int>() { user.No }, true);
        //}

    }

    public static void TestIssue()
    {
        int posId = 1;
        int no = 290017;
        DataProvider provider = DataProvider.Instance;
        User user = provider.UserList.FirstOrDefault(u => u.No == no);
        PLCManager.Instance.GetPlcByPositionId(posId, out PLC plc);

        ISet<User> workersSet = new HashSet<User>() { user };
        ISet<User> operatorsSet = new HashSet<User>() { user };

        bool success1 = plc.IssueUsers(workersSet, operatorsSet, false);


    }



    public static void TestIPVoice()
    {
        //SpeakerManager.Instance.Start(new List<int>() { 5 }, "，，，张亮 请打开，，，");
        SpeakerManager.Instance.Start(new List<int>() { 21, 22 }, "杨勇听到17股道音柱播报请回复");

        Thread.Sleep(20 * 1000);

        //var positionId = 1;
        //SpeakerManager.Instance.SwitchOff(DataProvider.Instance.GetPosition(positionId).SpeakerIds, positionId.ToString());


        DataProvider.Instance.DeviceInfos.ForEach(d =>
        {
            DeviceService.BuildDevice(d);
        });

        var poss = DataProvider.Instance.TrackList.SelectMany(t => t.Positions);
        foreach (var pos in poss)
        {
            var speakers = SpeakerManager.Instance.Devices.Values?.Where(s => pos.DeviceIds.Contains(s.Info.ID));
            pos.SpeakerIds = speakers?.Select(s => s.TerminalId)?.ToList();
        }

        Task.Run(async () =>
        {
            Random rand = new Random();

            while (true)
            {
                int doorstate = -1;
                if (rand.Next(1000) > 500)
                {
                    doorstate = 1;
                }
                else
                {
                    doorstate = 2;
                }

                IPSpeakerService.MonitorDoorState(poss.ElementAt(0), 0, new List<int>() { doorstate });
                await Task.Delay(TimeSpan.FromSeconds(rand.Next(4, 10)));
            }
        });

        Task.Run(async () =>
        {
            Random rand = new Random();

            while (true)
            {
                int doorstate = -1;
                if (rand.Next(1000) > 500)
                {
                    doorstate = 1;
                }
                else
                {
                    doorstate = 2;
                }

                IPSpeakerService.MonitorDoorState(poss.ElementAt(0), 1, new List<int>() { doorstate });
                await Task.Delay(TimeSpan.FromSeconds(rand.Next(4, 10)));
            }
        });



        Task.Run(async () =>
        {
            Random rand = new Random();

            while (true)
            {
                int doorstate = -1;
                if (rand.Next(1000) < 500)
                {
                    doorstate = 1;
                }
                else
                {
                    doorstate = 2;
                }

                IPSpeakerService.MonitorDoorState(poss.ElementAt(1), 1, new List<int>() { doorstate });
                await Task.Delay(TimeSpan.FromSeconds(rand.Next(4, 10)));
            }
        });

        Console.ReadKey();
    }

    public static void TestRecord()
    {
        var pos = DataProvider.Instance.TrackList.SelectMany(t => t.Positions).ElementAt(0);
        OperationRecord record = new OperationRecord()
        {
            Index = pos.Index,
            Time = DateTime.Now,
            TrackNo = DataProvider.Instance.GetTrackByPosId(pos.ID).No,
            WorkerNo = "290017",
            OperationCode = $"{(int)PLCOperateCode.手动消除权限}",
        };
        DataProvider.Instance.AddOrUpdateOperationRecord(record);
    }

    public static void TestPing()
    {
        Task.Run(async () =>
        {
            while (true)
            {
                string host = "10.6.1.1";
                Ping p1 = new Ping();
                PingReply reply = await p1.SendPingAsync(host); //发送主机名或Ip地址

                StringBuilder sbuilder;
                if (reply.Status == IPStatus.Success)
                {
                    sbuilder = new StringBuilder();
                    sbuilder.Append(string.Format("Address: {0} ", reply.Address.ToString()));
                    sbuilder.Append(string.Format("RoundTrip time: {0} ", reply.RoundtripTime));
                    sbuilder.Append(string.Format("Time to live: {0} ", reply.Options.Ttl));
                    sbuilder.Append(string.Format("Don't fragment: {0} ", reply.Options.DontFragment));
                    sbuilder.Append(string.Format("Buffer size: {0} ", reply.Buffer.Length));
                    Debug.WriteLine(sbuilder.ToString());
                    await Task.Delay(1000);
                }
                else if (reply.Status == IPStatus.TimedOut)
                {
                    Debug.WriteLine("超时");
                }
                else
                {
                    Debug.WriteLine("失败");
                }

            }
        });


        Console.ReadKey();

    }

}
