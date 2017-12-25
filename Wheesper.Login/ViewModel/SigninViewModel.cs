using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.ViewModel;
using Microsoft.Practices.Unity;
using Wheesper.Login.events;
using Wheesper.Login.Model;

using System.Text.RegularExpressions;
using Wheesper.Messaging.events;
using ProtocolBuffer;
using System.Diagnostics;
using Wheesper.Infrastructure.events;

namespace Wheesper.Login.ViewModel
{
    public class SigninViewModel : NotificationObject
    {
        #region private menber
        private IUnityContainer container = null;
        private IEventAggregator eventAggregator = null;
        private LoginModel loginModel = null;
        #endregion private menber

        #region properties
        private string promtInfo = null;
        public string Email
        {
            get
            {
                return loginModel.email;
            }
            set
            {
                loginModel.email = value;
                Debug.WriteLine(Email);
                RaisePropertyChanged("Email");
                SigninMailNextCommand.RaiseCanExecuteChanged();
            }
        }
        public string Password
        {
            get
            {
                return loginModel.password;
            }
            set
            {
                loginModel.password = value;
                Debug.WriteLine(Email);
                Debug.WriteLine(Password);
                RaisePropertyChanged("Password");
                SigninPWNextCommand.RaiseCanExecuteChanged();
            }
        }
        public string PromtInfo
        {
            get
            {
                return promtInfo;
            }
            set
            {
                promtInfo = value;
                RaisePropertyChanged("PromtInfo");
            }
        }
        #endregion properties

        #region command
        private DelegateCommand signinMailNextCommand;
        private DelegateCommand signinPWNextCommand;
        private DelegateCommand signinPWBackCommand;
        private DelegateCommand createAccountCommand;
        private DelegateCommand forgetPWCommand;

        public DelegateCommand SigninMailNextCommand
        {
            get
            {
                if (signinMailNextCommand == null)
                {
                    signinMailNextCommand = new DelegateCommand(signinMailNext, canSigninMailNext);
                }
                return signinMailNextCommand;
            }
            set
            {
                signinMailNextCommand = value;
            }
        }

        public DelegateCommand SigninPWNextCommand
        {
            get
            {
                if (signinPWNextCommand == null)
                {
                    signinPWNextCommand = new DelegateCommand(signinPWNext, canSigninPWNext);
                }
                return signinPWNextCommand;
            }
            set
            {
                signinPWNextCommand = value;
            }
        }

        public DelegateCommand SigninPWBackCommand
        {
            get
            {
                if (signinPWBackCommand == null)
                {
                    signinPWBackCommand = new DelegateCommand(signinPWBack, canSigninPWBack);
                }
                return signinPWBackCommand;
            }
            set
            {
                signinPWBackCommand = value;
            }
        }

        public DelegateCommand CreateAccountCommand
        {
            get
            {
                if (createAccountCommand == null)
                {
                    createAccountCommand = new DelegateCommand(createAccount, canCreateAccount);
                }
                return createAccountCommand;
            }
            set
            {
                createAccountCommand = value;
            }
        }

        public DelegateCommand ForgetPWCommand
        {
            get
            {
                if (forgetPWCommand == null)
                {
                    forgetPWCommand = new DelegateCommand(forgetPW, canForgetPW);
                }
                return forgetPWCommand;
            }
            set
            {
                forgetPWCommand = value;
            }
        }
        #endregion command

        public void Initialize(string email)
        {
            loginModel.clearAllField();
            Email = email;
        }

        #region private helper variable
        private string emailInvalidMessage = "你输入的邮箱格式不正确";
        private string pwWrongMessage = "你输入的邮箱或密码不正确. ";
        private string networkFailMessage = "There's some problem with network. Please try again. If you continue to get this message, try again later.";
        #endregion helper variable

        #region helper function
        private void subevent()
        {
            eventAggregator.GetEvent<MsgSigninResponseEvent>().Subscribe(SigninResponseEventHandler, true);
        }
        
        private void prepare()
        {
            Password = null;
            PromtInfo = null;
        }
        #endregion helper function

        #region Constructor & deconstructor
        public SigninViewModel(IUnityContainer container, LoginModel loginModel)
        {
            Debug.WriteLine("SigninViewModel constructor");
            this.container = container;
            eventAggregator = this.container.Resolve<IEventAggregator>();
            this.loginModel = loginModel;

            subevent();
        }
        ~SigninViewModel()
        {
            Debug.WriteLine("SigninViewModel deconstructor");
        }
        #endregion Constructor & deconstructor

        #region Command Delegate Method
        private void signinMailNext()
        {
            if (loginModel.isEmailAddress(Email))
            {
                prepare();
                Debug.WriteLine("mail valid");
                eventAggregator.GetEvent<SigninMailNextEvent>().Publish(0);
            }
            else
            {
                Debug.WriteLine("mail invalid");
                PromtInfo = emailInvalidMessage;
            }
        }
        private bool canSigninMailNext()
        {
            return !string.IsNullOrWhiteSpace(Email);
        }

        private void signinPWNext()
        {
            // TODO: show some animate which also help avoid user to click back
            // Password = "123456789";
            // Email = "sdf@sdf.com";
            loginModel.sendSigninRequest();
        }
        private bool canSigninPWNext()
        {
            return !string.IsNullOrWhiteSpace(Password);
            //return true;
        }

        private void signinPWBack()
        {
            // TODO: when user send signin request the button should be shutdowm?
            Password = null;
            PromtInfo = null;
            Debug.WriteLine("signin pw back");
            eventAggregator.GetEvent<SigninPWBackEvent>().Publish(Email);
            // var e = eventAggregator.GetEvent<SigninPWBackEvent>();
            
        }
        private bool canSigninPWBack()
        {
            return true;
        }

        private void createAccount()
        {
            loginModel.currentState = State.SIGNUP;
            Debug.Write("Client State change to ");
            Debug.WriteLine(loginModel.currentState.ToString());
            eventAggregator.GetEvent<CreateAccountEvent>().Publish(0);
        }
        private bool canCreateAccount()
        {
            return true;
        }

        private void forgetPW()
        {
            loginModel.currentState = State.RESETPW;
            Debug.Write("Client State change to ");
            Debug.WriteLine(loginModel.currentState.ToString());
            eventAggregator.GetEvent<ForgetPWEvent>().Publish(Email);
        }
        private bool canForgetPW()
        {
            return true;
        }
        #endregion Command Delegate Method

        #region event handler
        private void SigninResponseEventHandler(ProtoMessage message)
        {
            if (loginModel.currentState != State.SIGNIN)
            {
                return;
            }
            bool status = message.SigninResponse.Status;
            Debug.WriteLine(status);
            if (status)
            {
                // sign in successfully
                eventAggregator.GetEvent<LoginEvent>().Publish(Email);
                eventAggregator.GetEvent<SigninPWNextEvent>().Publish(0);
            }
            else
            {
                // clear the pw and promt the user to try again or reset pw
                Password = null;
                PromtInfo = pwWrongMessage;
            }
        }
        #endregion event handler
    }
}
