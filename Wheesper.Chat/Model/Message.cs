
namespace Wheesper.Chat.Model
{
    public class Message
    {
        public string SenderEMail
        {
            get { return senderEMail; }
            set { senderEMail = value; }
        }
        public string RecevieEMail
        {
            get { return recevieEMail; }
            set { recevieEMail = value; }
        }
        public string Content
        {
            get { return content; }
            set { content = value; }
        }
        public string Data_time
        {
            get { return data_time; }
            set { data_time = value; }
        }
        public string SenderNickname
        {
            get { return senderNickname; }
            set { senderNickname = value; }
        }
        public string RecevieNickname
        {
            get { return recevieNickname; }
            set { recevieNickname = value; }
        }

        private string senderEMail = null;
        private string recevieEMail = null;
        private string content = null;
        private string data_time = null;
        private string senderNickname = null;
        private string recevieNickname = null;
    }
}
