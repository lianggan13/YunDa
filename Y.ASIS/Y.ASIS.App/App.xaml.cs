using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Y.ASIS.App.Windows;

namespace Y.ASIS.App
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        private static Mutex mutex;
        private const string AppName = "Y.ASIS.App";
        public App()
        {
            AppDomain.CurrentDomain.FirstChanceException += Domain_FirstChanceException;
            AppDomain.CurrentDomain.UnhandledException += Domain_UnhandledException;
        }

        /// <summary>
        /// UI unhandled exception
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            Task.Delay(100);
            // e.Exception.InnerException.Message
            // e.Exception.Message
            // e.Dispatcher.Thread.ManagedThreadId
            var ex = e.Exception;
            LogHelper.Fatal($"{ex.Message}", ex);

            // Add entry to event log
            //EventLog.WriteEntry(AppName, $"{e.Exception}", EventLogEntryType.Error);

            // Keep application running in the face of this exception
            e.Handled = true;
        }

        /// <summary>
        /// Domain first chance exception
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Domain_FirstChanceException(object sender, System.Runtime.ExceptionServices.FirstChanceExceptionEventArgs e)
        {
            Task.Delay(100);

            var ex = e.Exception;
            LogHelper.Fatal($"{ex.Message}", ex);


        }

        /// <summary>
        /// Domain unhandled exception
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Domain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Task.Delay(100);
            if (e.ExceptionObject is Exception ex)
            {
                LogHelper.Fatal($"{ex.Message}", ex);
            }
        }


        private void OnExit(object sender, ExitEventArgs e)
        {
            if (mutex != null)
            {
                mutex.ReleaseMutex();
                mutex.Close();
                Environment.Exit(0);
            }
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            if (HasApplicationStarted())
            {
                MessageWindow.Show("程序已在运行中!");
                Shutdown();
            }
        }

        private bool HasApplicationStarted()
        {
            mutex = new Mutex(true, AppName, out bool hasApp);
            return !hasApp;
        }
    }
}