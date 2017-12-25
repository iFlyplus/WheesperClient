using Microsoft.Practices.Prism.ViewModel;
using System.Collections.Generic;


namespace Wheesper.Chat.Model
{
    public class Contact : NotificationObject
    {
        public string EMail
        {
            get { return email; }
            set
            {
                email = value;
                RaisePropertyChanged("EMail");
            }
        }
        public string Nickname
        {
            get { return nickname; }
            set
            {
                nickname = value;
                RaisePropertyChanged("Nickname");
            }
        }
        public string Group
        {
            get { return group; }
            set
            {
                group = value;
                RaisePropertyChanged("Group");
            }
        }
        public string Remarks
        {
            get { return remarks; }
            set
            {
                remarks = value;
                RaisePropertyChanged("Remarks");
            }
        }
        public string Sex
        {
            get { return sex; }
            set
            {
                sex = value;
                RaisePropertyChanged("Sex");
            }
        }
        public int Age
        {
            get { return age; }
            set
            {
                age = value;
                RaisePropertyChanged("Age");
            }
        }
        public List<Message> ChatMessageList
        {
            get { return chatMessageList; }
            private set
            {
                chatMessageList = value;
                RaisePropertyChanged("ChatMessageList");
            }
        }
        public string FirstC
        {
            get { return nickname[0].ToString(); }
            set { }
        }

        private string email = null;
        private string nickname = null;
        private string group = null;
        private string remarks = null;
        private string sex = null;
        private int age = 0;
        private List<Message> chatMessageList = new List<Message>();
    }
}
