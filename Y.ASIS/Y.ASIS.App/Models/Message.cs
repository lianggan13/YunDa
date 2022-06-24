using System;
using System.Windows.Media;
using Y.ASIS.Common.MVVMFoundation;
namespace Y.ASIS.App.Models
{
    public class Message : NotifyObjectBase
    {
        public Message(MessageType type, string content, DateTime time)
        {
            Type = type;
            Content = content;
            Time = time;
        }

        public Message(MessageType type, string content)
        {
            Type = type;
            Content = content;
            Time = DateTime.Now;
        }

        private DateTime time;
        public DateTime Time
        {
            get { return time; }
            set { SetProperty(ref time, value); }
        }

        private MessageType type;
        public MessageType Type
        {
            get { return type; }
            set { SetProperty(ref type, value); }
        }

        private string content;
        public string Content
        {
            get { return content; }
            set { SetProperty(ref content, value); }
        }

        public SolidColorBrush Brush
        {
            get
            {
                switch (Type)
                {
                    case MessageType.Ordinary:
                        return Brushes.White;
                    case MessageType.Info:
                        return Brushes.LightBlue;
                    case MessageType.Warning:
                        return Brushes.Yellow;
                    case MessageType.Safe:
                        return new SolidColorBrush(Color.FromRgb(34, 172, 56));
                    case MessageType.Dangerous:
                        return Brushes.Red;
                    default:
                        throw new Exception();
                }
            }
        }
    }

    public enum MessageType
    {
        Ordinary,
        Info,
        Warning,
        Safe,
        Dangerous
    }
}
