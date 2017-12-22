using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.ViewModel;
using Microsoft.Practices.Unity;
using Wheesper.Login.events;

using System.Diagnostics;
using Wheesper.Infrastructure.events;
using Wheesper.Login.Model;
using Wheesper.Messaging.events;
using ProtocolBuffer;

namespace Wheesper.Login.ViewModel
{
    public class WelcomeViewModel : NotificationObject
    {
        #region private member
        private IUnityContainer container = null;
        private IEventAggregator eventAggregator = null;
        private LoginModel loginModel = null;
        #endregion private member

        #region Constructor & deconstructor
        public WelcomeViewModel(IUnityContainer container)
        {
            Debug.WriteLine("WelcomeViewModel constructor");
            this.container = container;
            eventAggregator = this.container.Resolve<IEventAggregator>();
            loginModel = this.container.Resolve<LoginModel>();

            subevent();
        }

        ~WelcomeViewModel()
        {
            Debug.WriteLine("WelcomeViewModel deconstructor");
        }
        #endregion Constructor & deconstructor

        public void Initialize(string welcomeMessage_1, string welcomeMessage_2)
        {
            WelcomeMessage_1 = welcomeMessage_1;
            WelcomeMessage_2 = welcomeMessage_2;
        }
        public void SetNickname(string name)
        {
            Nickname = name;
        }

        #region helper functoin
        private void subevent()
        {
            Debug.WriteLine("WelcomeViewModel subscribe event");
            eventAggregator.GetEvent<MsgSigninResponseEvent>().Subscribe(SigninResponseEventHandler, true);
        }
        #endregion helper function

        #region properties
        private string nickname = null;
        private string welcomeMessage_1;
        private string welcomeMessage_2;

        public string Nickname
        {
            get { return nickname; }
            set { nickname = value; }
        }
        public string WelcomeMessage_1
        {
            get
            {
                return welcomeMessage_1;
            }
            set
            {
                welcomeMessage_1 = value;
                RaisePropertyChanged("WelcomeMessage_1");
            }
        }
        public string WelcomeMessage_2
        {
            get
            {
                return welcomeMessage_2;
            }
            set
            {
                welcomeMessage_2 = value;
                RaisePropertyChanged("WelcomeMessage_2");
            }
        }
        #endregion properties

        #region Command
        private DelegateCommand startCommnd;

        public DelegateCommand StartCommand
        {
            get
            {
                if (startCommnd == null)
                {
                    startCommnd = new DelegateCommand(start);
                }
                return startCommnd;
            }
        }
        #endregion Command

        #region Command Delegate Method
        private void start()
        {
            // when user click start, send a signin request
            loginModel.sendSigninRequest();
            // eventAggregator.GetEvent<ShowWheesperViewEvent>().Publish(0);
        }
        #endregion Command Delegate Method

        #region event handler

        private void SigninResponseEventHandler(ProtoMessage message)
        {
            if (loginModel.currentState != State.SIGNUP)
            {
                return;
            }
            bool status = message.SigninResponse.Status;
            Debug.WriteLine(status);
            if (status)
            {
                // sign in successfully
                Debug.WriteLine("enter wheesper from signup scenory");
                eventAggregator.GetEvent<LoginEvent>().Publish(loginModel.email);
                eventAggregator.GetEvent<ShowWheesperViewEvent>().Publish(0);
            }
            else
            {
                // clear the pw and promt the user to try again or reset pw
                //Password = null;
                //PromtInfo = pwWrongMessage;
            }
        }
        #endregion event handler
    }
}
