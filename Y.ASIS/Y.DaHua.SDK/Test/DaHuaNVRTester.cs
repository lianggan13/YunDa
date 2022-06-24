using NetSDKCS;
using System.Threading.Tasks;

namespace Y.DaHua.NVR.Test
{
    public class DaHuaNVRTester
    {
        private static NVR_Preview nvr;

        //private static object objLock = new object();

        static DaHuaNVRTester()
        {
            nvr = new NVR_Preview();
            nvr.NVR_Load();
        }

        public static void OpenPreview(EM_RealPlayType playType, int channel)//切换预览
        {
            if (nvr.IsPreviewing())
                return;

            nvr.Preview(playType, channel);
        }

        public static async void PreviewwithTime(EM_RealPlayType playType, int channel, int millisecondsDelay = 3000)//切换预览
        {
            if (nvr.IsPreviewing())
                return;

            nvr.Preview(playType, channel);
            await Task.Delay(millisecondsDelay);
            nvr.StopPreview();

            Reset();
        }

        public static async void Reset()
        {
            if (nvr.IsPreviewing())
            {
                // 确保已关闭 预览
                nvr.StopPreview();
                await Task.Delay(1000);
            }

            if (nvr.CurrentPlayType != EM_RealPlayType.Multiplay_16)
            {
                nvr.Preview(EM_RealPlayType.Multiplay_16, 0);
                await Task.Delay(1000);
                nvr.StopPreview();
            }
        }

    }
}
