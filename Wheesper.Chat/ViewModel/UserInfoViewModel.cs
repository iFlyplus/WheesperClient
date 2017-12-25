using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.ViewModel;
using Microsoft.Practices.Unity;
using System.Diagnostics;
using Wheesper.Chat.Model;
using Wheesper.Login.events;

namespace Wheesper.Chat.ViewModel
{
    public class UserInfoViewModel : NotificationObject
    {
        #region private menber
        private IUnityContainer container = null;
        private IEventAggregator eventAggregator = null;
        private WheesperModel model = null;
        #endregion private menber

        #region properties
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

        private string email = null;
        private string nickname = null;
        private string sex = null;
        private int age = 0;
        private string country = null;
        private string province = null;
        private string city = null;
        #endregion properties

        #region Commond
        public DelegateCommand ApplyCommand
        {
            get
            {
                if (applyCommond == null)
                {
                    applyCommond = new DelegateCommand(apply, canApply);
                }
                return applyCommond;
            }
            private set { }
        }
        public DelegateCommand CancelCommand
        {
            get
            {
                if (cancelCommand == null)
                {
                    cancelCommand = new DelegateCommand(cancel, canCancel);
                }
                return cancelCommand;
            }
            private set { }
        }

        private DelegateCommand applyCommond = null;
        private DelegateCommand cancelCommand = null;
        #endregion Commond

        #region Command Delegate Method
        private void apply()
        {
            model.sendUserInfoModifyRequest(EMail, Nickname, Sex, Age, Country, Province, City);
        }
        private bool canApply()
        {
            return !string.IsNullOrWhiteSpace(Nickname);
        }

        private void cancel()
        {
            eventAggregator.GetEvent<CloseUserInfoViewEvent>().Publish(0);
        }
        private bool canCancel()
        {
            return true;
        }
        #endregion Command Delegate Method

        #region Constructor & deconstructor
        public UserInfoViewModel(IUnityContainer container)
        {
            Debug.WriteLine("UserInfoViewModel constructor");
            this.container = container;
            eventAggregator = this.container.Resolve<IEventAggregator>();
            model = this.container.Resolve<WheesperModel>();

            subevent();
        }

        ~UserInfoViewModel()
        {
            Debug.WriteLine("UserInfoViewModel Deconstrutor");
        }
        #endregion Constructor & deconstructor

        #region helper functoin
        private void subevent()
        {
            Debug.WriteLine("ChangeContactInfoViewModel subscribe event");
        }
        #endregion helper function
    }
}
