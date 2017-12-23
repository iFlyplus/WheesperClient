using Microsoft.Practices.Prism.ViewModel;
using System;

namespace Wheesper.Chat.Model
{
    public class UserInfo : NotificationObject
    {
        private string email = null;
        private string nickname = null;
        private string sex = null;
        private int age = 0;
        private string country = null;
        private string province = null;
        private string city = null;
        private string createDate = null;

        public string EMail
        {
            get { return email; }
            set
            {
                email = value;
                RaisePropertyChanged("Email");
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
        public string Country
        {
            get { return country; }
            set
            {
                country = value;
                RaisePropertyChanged("Country");
            }
        }
        public string Province
        {
            get { return province; }
            set
            {
                province = value;
                RaisePropertyChanged("Province");
            }
        }
        public string City
        {
            get { return city; }
            set
            {
                city = value;
                RaisePropertyChanged("City");
            }
        }
        public string CreateDate
        {
            get { return createDate; }
            set
            {
                createDate = value;
                RaisePropertyChanged("CreateDate");
            }
        }


        private DateTime date = new DateTime();
        public DateTime Date
        {
            get { return date; }
            set
            {
                date = value;
                RaisePropertyChanged("Date");
            }
        }
    }
}
