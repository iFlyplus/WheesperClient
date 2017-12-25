using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.ViewModel;
using Microsoft.Practices.Unity;
using Wheesper.Login.events;
using Wheesper.Login.Model;

using Wheesper.Messaging.events;
using ProtocolBuffer;
using System.Diagnostics;
using Wheesper.Infrastructure.events;

namespace Wheesper.Login.ViewModel
{
    public class PWModifyViewModel : NotificationObject
    {
        #region private member
        private IUnityContainer container = null;
        private IEventAggregator eventAggregator = null;
        private LoginModel loginModel = null;
        #endregion private menber

        public void Initialize(string email)
        {
            loginModel.clearAllField();
            Email = email;
        }

        #region helper function
        private void subevent()
        {
            eventAggregator.GetEvent<MsgSignupMailResponseEvent>().Subscribe(SignupMailResponseEventHandler);
            eventAggregator.GetEvent<MsgPasswordModifyCaptchaResponseEvent>().Subscribe(PasswordModifyCaptchaResponseEventHandler);
            eventAggregator.GetEvent<MsgPasswordModifyResponseEvent>().Subscribe(PasswordModifyResponseEventHandler);
        }
        #endregion helper function

        #region Constructor & deconstructor
        public PWModifyViewModel(IUnityContainer container, LoginModel loginModel)
        {
            Debug.WriteLine("PWModifyViewModel constructor");
            this.container = container;
            eventAggregator = this.container.Resolve<IEventAggregator>();
            this.loginModel = loginModel;

            subevent();
        }
        ~PWModifyViewModel()
        {
            Debug.WriteLine("PWModifyViewModel deconstructor"); 
        }
        #endregion Constructor & deconstructor

        #region helper variable
        private string emailNotExistMessage = "你输入的邮箱未注册成为Wheesper账号。";
        private string emailNotValidMessage = "你输入的邮箱格式不正确.";
        private string pwNotValidMessage = "Passwords must have at least 8 characters and contain at least two of the following: uppercase letters, lowercase letters, numbers, and symbols.";
        private string captchaSend = "We just sent a code to ";
        private string captchaInvalidMessage = "Please enter the 4-digit code. The code only contains numbers.";
        private string captchaWrongMessage = "你输入的验证码不正确，请确认后再次输入.";
        #endregion helper variable

        #region properties
        private string promtInfo = null; // email wrong, pw wrong
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

        public string Email
        {
            get
            {
                return loginModel.email;
            }
            set
            {
                loginModel.email = value;
                RaisePropertyChanged("Email");
                PWModifyMailNextCommand.RaiseCanExecuteChanged();
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
                RaisePropertyChanged("Password");
                PWModifyPWNextCommand.RaiseCanExecuteChanged();
            }
        }

        public string Captcha
        {
            get
            {
                return loginModel.captcha;
            }
            set
            {
                loginModel.captcha = value;
                RaisePropertyChanged("Captcha");
                PWModifyCaptchaNextCommand.RaiseCanExecuteChanged();
            }
        }
        #endregion proterties

        #region Command
        private DelegateCommand pwModifyMailNextCommand;
        private DelegateCommand pwModifyMailBackCommand;
        private DelegateCommand pwModifyPWNextCommand;
        private DelegateCommand pwModifyPWBackCommand;
        private DelegateCommand pwModifyCaptchaNextCommand;
        private DelegateCommand pwModifyCaptchaBackCommand;
        private DelegateCommand pwModifyCaptchaResendCommond;

        public DelegateCommand PWModifyMailNextCommand
        {
            get
            {
                if (pwModifyMailNextCommand == null)
                {
                    pwModifyMailNextCommand = new DelegateCommand(pwModifyMailNext, canPWModifyMailNext);
                }
                return pwModifyMailNextCommand;
            }
            set
            {
                pwModifyMailNextCommand = value;
            }
        }

        public DelegateCommand PWModifyMailBackCommand
        {
            get
            {
                if (pwModifyMailBackCommand == null)
                {
                    pwModifyMailBackCommand = new DelegateCommand(pwModifyPMailCancel, canPWModifyMailCancel);
                }
                return pwModifyMailBackCommand;
            }
            set
            {
                pwModifyMailBackCommand = value;
            }
        }

        public DelegateCommand PWModifyPWNextCommand
        {
            get
            {
                if (pwModifyPWNextCommand == null)
                {
                    pwModifyPWNextCommand = new DelegateCommand(pwModifyPWModifyNext, canPWModifyPWModifyNext);
                }
                return pwModifyPWNextCommand;
            }
            set
            {
                pwModifyPWNextCommand = value;
            }
        }

        public DelegateCommand PWModifyPWBackCommand
        {
            get
            {
                if (pwModifyPWBackCommand == null)
                {
                    pwModifyPWBackCommand = new DelegateCommand(pwModifyPWModifyBack, canPWModifyPWModifyBack);
                }
                return pwModifyPWBackCommand;
            }
            set
            {
                pwModifyPWBackCommand = value;
            }
        }

        public DelegateCommand PWModifyCaptchaNextCommand
        {
            get
            {
                if (pwModifyCaptchaNextCommand == null)
                {
                    pwModifyCaptchaNextCommand = new DelegateCommand(pwModifyCaptchaNext, canPWModifyCaptchaNext);
                }
                return pwModifyCaptchaNextCommand;
            }
            set
            {
                pwModifyCaptchaNextCommand = value;
            }
        }

        public DelegateCommand PWModifyCaptchaBackCommand
        {
            get
            {
                if (pwModifyCaptchaBackCommand == null)
                {
                    pwModifyCaptchaBackCommand = new DelegateCommand(pwModifyCaptchaBack, canPWModifyCaptchaBack);
                }
                return pwModifyCaptchaBackCommand;
            }
            set
            {
                pwModifyCaptchaBackCommand = value;
            }
        }

        public DelegateCommand PWModifyCaptchaResendCommand
        {
            get
            {
                if(pwModifyCaptchaResendCommond==null)
                {
                    pwModifyCaptchaResendCommond = new DelegateCommand(pwModifyCaptchaResend, canPWModifyCaptchaResend);
                }
                return pwModifyCaptchaResendCommond;
            }
            set
            {
                pwModifyCaptchaResendCommond = value;
            }
        }
        #endregion Command

        #region Command Delegate Method
        private void pwModifyMailNext()
        {
            if (loginModel.isEmailAddress(Email))
            {
                // TODO: enter animate_3
                PromtInfo = null;
                loginModel.sendSignupMailRequest();
            }
            else
            {
                // TODO:
                PromtInfo = emailNotValidMessage;
            }
        }
        private bool canPWModifyMailNext()
        {
            return !string.IsNullOrEmpty(Email);
        }

        private void pwModifyPMailCancel()
        {
            loginModel.currentState = State.SIGNIN;
            Debug.Write("Client State change to ");
            Debug.WriteLine(loginModel.currentState.ToString());
            eventAggregator.GetEvent<PWModifyMailCancelEvent>().Publish(Email);
        }
        private bool canPWModifyMailCancel()
        {
            return true;
        }

        private void pwModifyPWModifyNext()
        {
            if(loginModel.isPWQualified(Password))
            {
                eventAggregator.GetEvent<PWModifyPWNextEvent>().Publish(0);
                PromtInfo = null;
            }
            else
            {
                PromtInfo = pwNotValidMessage;
            }
        }
        private bool canPWModifyPWModifyNext()
        {
            return !string.IsNullOrWhiteSpace(Password);
        }

        private void pwModifyPWModifyBack()
        {
            eventAggregator.GetEvent<PWModifyPWBackEvent>().Publish(0);
        }
        private bool canPWModifyPWModifyBack()
        {
            return true;
        }
        
        private void pwModifyCaptchaResend()
        {
            PromtInfo = captchaSend.Insert(captchaSend.Length, Email);
            loginModel.sendPWCaptchaRequest();
        }
        private bool canPWModifyCaptchaResend()
        {
            return true;
        }


        private void pwModifyCaptchaNext()
        {
            if (loginModel.isCaptchaQualified(Captcha))
            {
                loginModel.sendPWModifyRequest();
            }
            else
            {
                PromtInfo = captchaInvalidMessage;
            }
        }
        private bool canPWModifyCaptchaNext()
        {
            // TODO: after press the get captcha button and the server send the user's mail a captcha, then the user can press the next
            bool flag = false;
            flag = !string.IsNullOrWhiteSpace(Captcha);
            return !string.IsNullOrWhiteSpace(Captcha);
        }

        private void pwModifyCaptchaBack()
        {
            Captcha = null;
            PromtInfo = null;
            eventAggregator.GetEvent<PWModifyCaptchaBackEvent>().Publish(0);
        }
        private bool canPWModifyCaptchaBack()
        {
            return true;
        }
        #endregion Command Delegate Method

        #region event handler
        private void SignupMailResponseEventHandler(ProtoMessage message)
        {
            if (loginModel.currentState != State.RESETPW)
                return;
            bool status = message.SignupMailResponse.Status;
            if (status)
            {
                PromtInfo = emailNotExistMessage;
            }
            else
            {
                loginModel.sendPWCaptchaRequest();
            }
        }

        private void PasswordModifyCaptchaResponseEventHandler(ProtoMessage message)
        {

            bool status = message.PasswordModifyCaptchaResponse.Status;
            if(status)
            {
                // TODO: exit animate_3 and enter pwModify pw view
                eventAggregator.GetEvent<PWModifyMailNextEvent>().Publish(0);
            }
            else
            {
                // TODO: exit animate_3 and return pwModify mail view
                PromtInfo = emailNotExistMessage.Insert(0, Email);
            }
        }

        private void PasswordModifyResponseEventHandler(ProtoMessage message)
        {
            bool status = message.PasswordModifyResponse.Status;
            if (status)
            {
                // TODO: enter congratulation
                //loginModel.sendSigninRequest();
                //eventAggregator.GetEvent<LoginEvent>().Publish(Email);
                eventAggregator.GetEvent<PWModifyCaptchaNextEvent>().Publish(0);
            }
            else
            {
                // TODO:
                PromtInfo = captchaWrongMessage;
            }
        }
        #endregion event handler
    }
}