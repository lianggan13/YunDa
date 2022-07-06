using RestSharp;
using System;
using System.Threading.Tasks;
using System.Windows;
using Y.ASIS.App.Common;
using Y.ASIS.App.Communication.Algorithm;
using Y.ASIS.App.Models;
using Y.ASIS.App.Services;
using Y.ASIS.App.Services.CameraService;
using Y.ASIS.App.Windows;
using Y.ASIS.Common.Utils;
using Y.HIKNVR.SDK;


public static class Tester
{
    public static void TestSafeConfirm()
    {
        var vm = AppGlobal.Instance.MainVM;


        int no = 1;
        var sc = SafeConfirmManager.Instance.GetSafeConfirm(no); // data comes from SafeConfirmConfig.json
        if (sc != null)
        {
            vm.CurrentPosition.SafeConfirm = sc;

            PositionService.ApplyVideoCondition(vm.CurrentPosition, sc);
        }


        return;
        if (vm.CurrentPosition != null)
        {
            SafeConfirmWindow window = new SafeConfirmWindow(vm.CurrentPosition, vm.CurrentUser)
            {
                Owner = Application.Current.MainWindow,
            };
            window.ShowDialog();
        }

        return;

        var ss = AlgorithmService.DetectSafety(22, "");

        return;
        while (true)
        {
            var url = "rtsp://admin:yunda123@10.6.1.253:554/cam/realmonitor?channel=1&subtype=1";
            //DetectResult result = SafeConfirmManager.Instance.AlgorithmDetect("", 29);
            DetectResult result2 = AlgorithmService.DetectTrain(url, 13); //  29 -- 自测
        }
    }

    public static void TestCamera()
    {
        return;
        //var track6videos = new int[] { 1, 2, 27, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 };
        var track6videos = new int[] { 1, };
        HIKNVRService.Switch(CameraLayout.XVI, track6videos);
        HIKNVRService.SwitchAll();
    }

    public static void TestTime()
    {
        var dt1 = TimeUtil.TimeStamp();
        var dt2 = TimeUtil.TimeStamp();
    }




    public static void TestGetTrackStates()
    {
        Task.Run(async () =>
        {
            while (true)
            {
                string Host = "http://192.168.1.87:8080/";
                string AuthKey = "5555555555555555";

                var client = new RestClient(Host);
                client.AddDefaultHeader("AuthKey", AuthKey);

                var apiUrl = "api/trackstate";
                var request = new RestRequest(apiUrl, Method.POST);
                request.Timeout = 3000;
                var response = client.Execute(request);
                var values = response.Content;
                if (string.IsNullOrEmpty(values))
                {

                }

                await Task.Delay(300);
            }
        });

        Console.Read();
    }

    static Random rand = new Random();


    public static void TestPlatform(Position position, PositionStateNet value)
    {
        //if (id == 2)
        //{
        //    value.Platforms[0].Doors = new ObservableCollection<int>() { rand.Next(0, 5) };
        //}
        //if (rand.Next(100) > 50)
        //{
        //    if (value.Platforms.Count > 1)
        //        value.Platforms[0].Doors[0] = rand.Next(1, 5);
        //    //value.Platforms[0].Doors[0] = rand.Next(1, 5);
        //}
    }


    public static void TestTrain(Position position, Y.ASIS.App.Models.PositionStateNet value)
    {

        if (value.Trains.Count == 0)
        {
            var train1 = new Train()
            {
                No = $"{rand.Next(1234, 9999)}",
                //State = rand.Next(1, 4).ToString(),
            };

            var train2 = new Train()
            {
                No = $"{rand.Next(1234, 9999)}",
                //State = rand.Next(1, 4).ToString(),
            };

            value.Trains.Add(train1);
            value.Trains.Add(train2);
        }

        if (rand.Next(1000) > 800)
        {
            //value.Trains[1] = null;
        }
    }


}
