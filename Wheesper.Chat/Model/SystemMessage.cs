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
        public int ID;
        public static int count = 0;
        public object OriginMessage = null;

        private string message = null;
        private bool isRead = false;

        public SystemMessage()
        {
            ID = count;
            count++;
        }
    }
}
