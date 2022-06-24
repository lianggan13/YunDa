using Y.ASIS.App.Communication.Algorithm;
using Y.ASIS.App.Services;
using Y.ASIS.App.Services.CameraService;
using Y.HIKNVR.SDK;

namespace Y.ASIS.App.Test
{
    public static class Tester
    {
        public static void TestSafeConfirm()
        {
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
    }
}
