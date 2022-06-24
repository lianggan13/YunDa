using System.Collections.ObjectModel;
using Y.ASIS.App.Models;

namespace Y.ASIS.App.Common
{
    class AppMessages
    {
        private static AppMessages instance;
        public static AppMessages Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AppMessages();
                }
                return instance;
            }
        }

        public static ObservableCollection<Message> Messages { get; } = new ObservableCollection<Message>();

        public void AddMessage(Message message)
        {
            if (Messages.Count == 1000)
            {
                Messages.RemoveAt(0);
            }
            Messages.Add(message);
        }

        public void AddOrdinaryMessage(string content)
        {
            Message message = new Message(MessageType.Ordinary, content);
            AddMessage(message);
        }

        public void AddInfoMessage(string content)
        {
            Message message = new Message(MessageType.Info, content);
            AddMessage(message);
        }

        public void AddWarningMessage(string content)
        {
            Message message = new Message(MessageType.Warning, content);
            AddMessage(message);
        }

        public void AddSafeMessage(string content)
        {
            Message message = new Message(MessageType.Safe, content);
            AddMessage(message);
        }

        public void AddDangerousMessage(string content)
        {
            Message message = new Message(MessageType.Dangerous, content);
            AddMessage(message);
        }
    }
}
