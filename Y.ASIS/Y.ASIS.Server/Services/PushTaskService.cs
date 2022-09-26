using RestSharp;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using Y.ASIS.Common.ExtensionMethod;
using Y.ASIS.Server.Database;
using Y.ASIS.Server.Utility;

namespace Y.ASIS.Server.Services
{
    public class PushTaskService
    {
        private static PushTaskService instance;

        public static PushTaskService Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new PushTaskService();
                }
                return instance;
            }
        }

        private const int ThreadNumber = 1;//4;

        private IList<Thread> workers;
        private readonly ConcurrentQueue<PushTask> queue;
        //private bool running;
        public bool running { get; set; }
        private bool disposed;

        private PushTaskService()
        {
            queue = new ConcurrentQueue<PushTask>();
            Start();
        }

        /// <summary>
        /// 执行
        /// </summary>
        private void DoWork()
        {
            while (!disposed)
            {
                if (running)
                {
                    try
                    {
                        if (queue.TryDequeue(out PushTask task) && task.CanExceture)
                        {
                            ExecuteTask(task);
                            if (task.Status == PushTaskStatus.Failed)
                            {
                                if (task.MaxRetry == -1)
                                {
                                    task.Status = PushTaskStatus.Waiting;
                                    queue.Enqueue(task);
                                }
                                else
                                {
                                    if (task.RetryTimes < task.MaxRetry)
                                    {
                                        task.Status = PushTaskStatus.Waiting;
                                        task.RetryTimes++;
                                        queue.Enqueue(task);
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (queue.IsEmpty)
                            {
                                Thread.Sleep(500);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        LogHelper.Error(e.Message);
                    }
                }
            }
        }

        private async void ExecuteTask(PushTask task)
        {
            try
            {
                task.Status = PushTaskStatus.Running;
                RestRequest request = RestSharpHelper.Instance.GetRestRequest(task.Url, out RestClient client);
                request.Method = Method.POST;
                //request.Timeout = 500;
                request.Timeout = 300;
                request.AddParameter("application/json", task.Obj.JsonSerialize(), ParameterType.RequestBody);
                task.Headers.ForEach(i => request.AddHeader(i.Key, i.Value));
                task.Cookies.ForEach(i => request.AddCookie(i.Name, i.Value));
                IRestResponse resp = await client.ExecuteAsync(request);
                task.Status = resp != null && resp.Content.ToUpper() == "OK" ? PushTaskStatus.Success : PushTaskStatus.Failed;
            }
            catch (Exception ex)
            {
                task.Error = ex;
                task.Status = PushTaskStatus.Failed;
                LogHelper.Warn(ex.Message, ex);
            }
        }

        /// <summary>
        /// 按照优先级重新排列任务队列
        /// </summary>
        /// <param name="priority">优先级值，如果大于-1将重新设置队列中任务的优先级</param>
        private void Arrange(int priority = -1)
        {
            if (queue.IsEmpty || priority <= -1) return;
            running = false;
            List<PushTask> list = new List<PushTask>();
            while (!queue.IsEmpty)
            {
                if (queue.TryDequeue(out PushTask task))
                {
                    list.Add(task);
                }
            }
            list.Sort((x, y) => x.Priority - y.Priority);
            list.ForEach(i =>
            {
                queue.Enqueue(i);
            });
            running = true;
        }

        /// <summary>
        /// 设置当前推送管理器的优先级
        /// </summary>
        /// <param name="priority"></param>
        public void SetPriority(int priority)
        {
            if (priority < 0) return;
            Arrange(priority);
        }

        /// <summary>
        /// 清除全部任务
        /// </summary>
        public void Clear()
        {
            while (!queue.IsEmpty)
            {
                queue.TryDequeue(out _);
            }
        }

        /// <summary>
        /// 启动
        /// </summary>
        public void Start()
        {
            if (running) return;
            disposed = false;
            running = true;
            if (workers != null)
            {
                throw new Exception("you must stop workers before start.");
            }
            ThreadStart threadStart = new ThreadStart(DoWork);
            workers = new List<Thread>(ThreadNumber);
            for (int i = 0; i < ThreadNumber; i++)
            {
                Thread thread = new Thread(threadStart)
                {
                    Name = "PushWorkerThread-" + i
                };
                workers.Add(thread);
                thread.Start();
            }
        }

        /// <summary>
        /// 停止
        /// </summary>
        public void Close()
        {
            if (disposed) return;
            disposed = true;
            running = false;
            if (workers != null)
            {
                foreach (Thread thread in workers)
                {
                    thread.Abort();
                }
            }
            workers = null;
        }


        /// <summary>
        /// 添加推送任务至队列
        /// </summary>
        /// <param name="task"></param>
        /// <param name="privilege"></param>
        public void AddQueue(PushTask task, bool privilege = false)
        {
            if (task == null) return;
            if (privilege)
            {
                int priority = task.Priority;
                priority = priority < 1 ? 0 : priority - 1;
                SetPriority(priority);
            }
            if (task.CanExceture)
            {
                queue.Enqueue(task);
            }
            if (!privilege)
            {
                Arrange();
            }
        }

        /// <summary>
        /// 添加推送任务至队列
        /// </summary>
        /// <param name="tasks"></param>
        /// <param name="privilege"></param>
        public void AddQueue(IEnumerable<PushTask> tasks, bool privilege = false)
        {
            if (!tasks.Any()) return;
            if (privilege)
            {
                int priority = tasks.Min(x => x.Priority);
                priority = priority < 1 ? 0 : priority - 1;
                SetPriority(priority);
            }
            tasks.ForEach(task =>
            {
                if (task.CanExceture)
                {
                    queue.Enqueue(task);
                }
            });
            if (!privilege)
            {
                Arrange();
            }
        }

        public void Push(object obj, int retry = -1)
        {
            IEnumerable<ExternalSystem> es = DataProvider.Instance.ExternalSystemList.Where(i => i.Connected);
            foreach (var i in es)
            {
                PushTask task = new PushTask(i.PushAddress, obj)
                {
                    MaxRetry = retry
                };
                AddQueue(task);
            }
        }

        //public void PushState(object obj)
        //{
        //    IEnumerable<ExternalSystem> es = DataProvider.Instance.ExternalSystemList.Where(i => i.Connected);
        //    if (es.Any())
        //    {
        //        DateTime expire = DateTime.Now.AddMilliseconds(1000);
        //        es.ForEach(i =>
        //        {
        //            PushTask task = new PushTask(i.PushAddress, obj)
        //            {
        //                MaxRetry = 1,
        //                ExpireTime = expire
        //            };
        //            AddQueue(task);
        //        });
        //    }
        //}
    }

    /// <summary>
    /// 推送任务状态
    /// </summary>
    public enum PushTaskStatus
    {
        Waiting,
        Running,
        Paused,
        Success = 200,
        Failed = 300
    }

    /// <summary>
    /// 描述一个信息推送任务
    /// </summary>
    public class PushTask
    {
        public PushTask(string url, object obj, int priority = 0)
        {
            Url = url;
            Obj = obj;
            Priority = priority;
        }

        /// <summary>
        /// 获取或设置任务的编号
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 推送地址
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 推送的对象
        /// </summary>
        public object Obj { get; set; }

        /// <summary>
        /// 推送请求头信息
        /// </summary>
        public IDictionary<string, string> Headers { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// 推送的Cookies信息
        /// </summary>
        public IEnumerable<Cookie> Cookies { get; set; } = new List<Cookie>();

        /// <summary>
        /// 获取或设置任务的优先级
        /// </summary>
        public int Priority { get; set; }

        /// <summary>
        /// 获取或设置任务的异常信息对象
        /// </summary>
        public Exception Error { get; set; }

        /// <summary>
        /// 获取或设置重试次数
        /// </summary>
        public int RetryTimes { get; set; }

        /// <summary>
        /// 获取或设置最大重试次数 默认为3次
        /// </summary>
        public int MaxRetry { get; set; } = 3;

        /// <summary>
        /// 获取或设置任务的状态
        /// </summary>
        public PushTaskStatus Status { get; set; }

        public DateTime ExpireTime { get; set; } = DateTime.MaxValue;

        /// <summary>
        /// 获取任务是否可以执行
        /// </summary>
        public bool CanExceture
        {
            get
            {
                return (int)Status < 200
                    && Status != PushTaskStatus.Running
                    && !string.IsNullOrEmpty(Url)
                    && ExpireTime > DateTime.Now;
            }
        }
    }
}
