using System;
using System.Threading.Tasks;

namespace Y.ASIS.App.Utility
{
    public static class TaskHelper
    {
        public static void LoopRun(Action action, TimeSpan delay, Action<Exception> exception = null)
        {
            Task.Run(async () =>
            {
                while (true)
                {
                    try
                    {
                        action?.Invoke();
                    }
                    catch (Exception ex)
                    {
                        exception?.Invoke(ex);
                    }
                    await Task.Delay(delay);
                }
            });
        }
    }
}
