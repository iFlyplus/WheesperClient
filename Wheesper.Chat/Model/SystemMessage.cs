using Microsoft.Practices.Prism.ViewModel;

namespace Wheesper.Chat.Model
{
    public class SystemMessage : NotificationObject
    {
        public string Message
        {
            get
            {
                if (message == null)
                    message = "";
                return message;
            }
            set
            {
                message = value;
                RaisePropertyChanged("Message");
            }
        }
        public bool IsRead
        {
            get { return isRead; }
            set
            {
                isRead = value;
                RaisePropertyChanged("IsRead");
            }
        }
        public SystemMessageType type { get; set; }
        public string ID { get { return id.ToString(); }private set { } }
        public static int count = 0;
        public object OriginMessage = null;
        public int Get_ID()
        {
            return id;
        }

        private string message = null;
        private bool isRead = false;
        private int id = 0;
        public SystemMessage()
        {
            id = count;
            count++;
        }

    }
}
