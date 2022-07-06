using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Y.ASIS.Server.Database;
using Y.ASIS.Server.Device.Speaker;
using Y.ASIS.Server.Models;

namespace Y.ASIS.Server.Services.IPSpeaker
{
    public static class IPSpeakerService
    {
        public static bool IsDoorOpen(int s)
        {
            bool state = false;
            if (s == 1 || s == 3)
                state = true;
            else if (s == 2 || s == 4)
                state = false;
            return state;
        }

        //public static List<DoorStateTask> DoorStateTasks;
        public static List<DoorStateTask> DoorStateTasks;
        private static SemaphoreSlim semaphore;

        static IPSpeakerService()
        {
            DoorStateTasks = new List<DoorStateTask>();
            semaphore = new SemaphoreSlim(1, 3);
        }

        public static void MonitorDoorState(Position pos, int doorIndex, List<int> doorStates)
        {
            int state = doorStates[0];

            try
            {
                semaphore.Wait();

                var dst = DoorStateTasks.FirstOrDefault(d => d.PositionId == pos.ID && d.DoorIndex == doorIndex);
                if (IsDoorOpen(state))
                {
                    if (dst == null)
                    {
                        var ndst = new DoorStateTask(pos.ID, doorIndex);
                        DoorStateTasks.Add(ndst);
                        ndst.Run();
                    }
                    else
                    {
                        dst.Cancel();
                        DoorStateTasks.Remove(dst);
                    }
                }
                else
                {
                    if (dst == null)
                    { }
                    else
                    {
                        dst.Cancel();
                        DoorStateTasks.Remove(dst);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message, ex);
            }
            finally
            {
                semaphore.Release();
            }
        }
    }


    public class DoorStateTask
    {
        public int PositionId;

        public int DoorIndex;

        public DoorStateTask(int positionId, int doorIndex)
        {
            PositionId = positionId;
            DoorIndex = doorIndex;
        }

        public Task task { get; set; }

        private CancellationTokenSource cts = new CancellationTokenSource();

        public void Run()
        {
            task = new Task(async () =>
            {
                DateTime newdt = DateTime.Now;

                while (true)
                {
                    if (cts.IsCancellationRequested)
                    {
                        LogHelper.Info($"Pos:{PositionId}--Door:{DoorIndex} Closed.");
                        break;
                    }

                    if (DateTime.Now - newdt > TimeSpan.FromSeconds(10d))
                    {
                        newdt = DateTime.Now;
                        var track = DataProvider.Instance.GetTrackByPosId(PositionId);
                        var pos = DataProvider.Instance.GetPosition(PositionId);
                        string txt = $"，警告，{track.No}股道{DoorIndex + 1}号门超时未关。";

                        //SpeakerManager.Instance.RealPlay(txt, times: 1, 1);
                        SpeakerManager.Instance.RealPlay(pos.SpeakerIds, txt, times: 2);

                        LogHelper.Info(txt);
                    }

                    await Task.Delay(TimeSpan.FromSeconds(1));
                }
            }, cts.Token);

            task.Start();
        }

        public void Cancel()
        {
            cts.Cancel();
        }


    }
}
