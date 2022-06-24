using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;


namespace Y.ASIS.Common.MVVMFoundation
{
    /// <summary>
    /// 向客户端发出某一属性值发生改变的基类
    /// </summary>
    public class NotifyObjectBase : INotifyPropertyChanged
    {
        /// <summary>
        /// 某一属性发生改变事件
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 触发属性改变事件
        /// </summary>
        /// <param name="propertyName">属性名称</param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged == null) return;
            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// 设置需通知的属性的值
        /// </summary>
        /// <typeparam name="T">属性的类型</typeparam>
        /// <param name="item">属性</param>
        /// <param name="value">值</param>
        /// <param name="propertyName">属性名称</param>
        protected virtual void SetProperty<T>(ref T item, T value, [CallerMemberName] string propertyName = null)
        {
            if (!EqualityComparer<T>.Default.Equals(item, value))
            {
                item = value;
                OnPropertyChanged(propertyName);
            }
        }

        /// <summary>
        /// 尝试查找系统资源
        /// </summary>
        /// <param name="resKey">资源的键</param>
        /// <returns>返回查找结果</returns>
        protected object TryFindResource(string resKey)
        {
            return Application.Current.TryFindResource(resKey);
        }

        public virtual IDictionary<string, object> GetProperties()
        {
            return null;
        }

        public virtual object GetProperties(string propertyName)
        {
            return null;
        }
    }
}
