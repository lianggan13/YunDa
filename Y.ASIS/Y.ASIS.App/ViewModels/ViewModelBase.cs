using System;
using System.Windows;
using System.Windows.Threading;
using Y.ASIS.Common.MVVMFoundation;
namespace Y.ASIS.App.ViewModels
{
    public class ViewModelBase : NotifyObjectBase, IDisposable
    {
        protected bool _disposed;

        public delegate object NotifyViewHandler(ViewModelMessage type, params object[] args);

        ~ViewModelBase()
        {
            Dispose(false);
        }

        public event NotifyViewHandler NotifyView;

        /// <summary>
        /// 通知界面事件
        /// </summary>
        /// <param name="msgType">消息类型</param>
        /// <param name="arg">相关参数</param>
        /// <returns>界面返回的处理结果</returns>
        protected virtual object OnNotifyView(ViewModelMessage msgType, params object[] arg)
        {
            object result = null;
            if (NotifyView != null)
            {
                AppDispatcherInvoker(() =>
                {
                    result = NotifyView(msgType, arg);
                });
            }
            return result;
        }

        /// <summary>
        /// 系统主线程执行函数
        /// </summary>
        /// <param name="action">要执行的函数</param>
        /// <param name="args">执行函数所需要的参数</param>
        protected void AppDispatcherInvoker(Action action, params object[] args)
        {
            Application app = Application.Current;
            if (app == null) return;
            app.Dispatcher.Invoke(action, args);
        }

        /// <summary>
        /// 系统主线程执行函数
        /// </summary>
        /// <param name="action">要执行的函数</param>
        /// <param name="priority">优先级</param>
        /// <param name="args">执行函数所需要的参数</param>
        protected void AppDispatcherBeginInvoker(Action action, DispatcherPriority priority = DispatcherPriority.Normal, params Object[] args)
        {
            Application app = Application.Current;
            if (app == null) return;
            app.Dispatcher.BeginInvoke(action, priority, args);
        }



        /// <summary>
        /// 提供一种用于释放非托管资源的机制
        /// </summary>
        /// <param name="dispoing">是否释放非托管资源</param>
        protected void Dispose(bool dispoing)
        {
            if (_disposed) return;
            if (dispoing)
            {
                ReleaseUnmanagedRes();
            }
            _disposed = true;
        }

        /// <summary>
        ///  释放非托管资源
        /// </summary>
        protected virtual void ReleaseUnmanagedRes()
        {

        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            Dispose(true);
        }
    }
}
