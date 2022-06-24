using NetSDKCS;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Y.DaHua.NVR.Test;

namespace Y.DaHua.NVR
{
    public class Program
    {
        static Thread change;
        static Random random = new Random();
        static void Main(string[] args)
        {
            Console.WriteLine("Start test....");
            List<Task> tasks = new List<Task>();

            var ipc = new
            {
                Ip = "192.168.1.108",
                Port = (ushort)37777,
                UserName = "admin",
                Password = "admin123",
            };
            NET_DEVICEINFO_Ex deviceInfo = new NET_DEVICEINFO_Ex();
            var LoginId = NETClient.Login(ipc.Ip, ipc.Port, ipc.UserName, ipc.Password, EM_LOGIN_SPAC_CAP_TYPE.TCP, IntPtr.Zero, ref deviceInfo);

            for (int i = 0; i < 3; i++)
            {
                Task task = new Task(() =>
                {
                    while (true)
                    {
                        bool open = random.Next(1000) > 500;
                        if (open)
                        {
                            DaHuaNVRTester.OpenPreview(NetSDKCS.EM_RealPlayType.Multiplay_1, 0);
                        }
                        else
                        {
                            DaHuaNVRTester.Reset();
                        }
                        Thread.Sleep(1500);
                    }
                });

                tasks.Add(task);
            }

            Task.WhenAll(tasks);

            //realplay realplay1 = new realplay();
            //realplay1.ChangeRealplay(NetSDKCS.EM_RealPlayType.Multiplay_16, 0);
            //change = new Thread(new ThreadStart(change_show));
            //change.IsBackground = true;
            //change.Start();

            Console.WriteLine("end.");
            Console.ReadLine();


        }

        static void change_show()
        {
            int i = 0;
            NVR_Preview realplay1 = new NVR_Preview();
            //realplay1.NVR_Load();

            realplay1.NVR_Load("192.168.1.117", 37777, "admin", "yunda123");


            while (true)
            {

                if (i % 4 == 0)
                {
                    realplay1.PreviewTimeout(NetSDKCS.EM_RealPlayType.Multiplay_4, 0, 4000);
                }
                else if (i % 4 == 1)
                {
                    realplay1.Preview(NetSDKCS.EM_RealPlayType.Multiplay_9, 0);
                }
                else if (i % 4 == 2)
                {
                    realplay1.Preview(NetSDKCS.EM_RealPlayType.Multiplay_1, 0);
                }
                else if (i % 4 == 3)
                {
                    realplay1.Preview(NetSDKCS.EM_RealPlayType.Multiplay_16, 0);
                }
                i++;
                if (i > 1000)
                {
                    i = 0;
                }
                Thread.Sleep(1000);
                realplay1.StopPreview();
                Thread.Sleep(1000);
            }

        }



    }
}
