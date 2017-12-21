using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.ViewModel;
using Microsoft.Practices.Unity;
using Wheesper.Login.events;
using Wheesper.Login.Model;

using Wheesper.Messaging.events;
using ProtocolBuffer;
using System.Diagnostics;

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
            eventAggregator.GetEvent<MsgSignupMailResponseEvent>().Subscribe(PasswordModifyMailResponseEventHandler);
            eventAggregator.GetEvent<MsgPasswordModifyResponseEvent>().Subscribe(PasswordModifyResponseEventHandler);
        }
        #endregion helper function

        #region Constructor
        public PWModifyViewModel(IUnityContainer container, LoginModel loginModel)
        {
            Debug.WriteLine("PWModifyViewModel constructor");
            this.container = container;
            eventAggregator = this.container.Resolve<IEventAggregator>();
            this.loginModel = loginModel;

            subevent();
        }
        #endregion Constructor

        #region helper variable
        private string emailNotExistMessage = " is already a Microsoft account. Try another name or claim one of these that's available. If it's yours, sign in now.";
        private string emailNotValidMessage = "The email address you entered isn's vaild.";
        private string pwNotValidMessage = "Passwords must have at least 8 characters and contain at least two of the following: uppercase letters, lowercase letters, numbers, and symbols.";
        private string captchaSend = "We just sent a code to ";
        private string captchaInvalidMessage = "Please enter the 4-digit code. The code only contains numbers.";
        private string captchaWrongMessage = "That code didn't work. Check the code and try again.";
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
                RaisePropertyChanged("Promt");
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
        private DelegateCommand pwModifyPWModifyBackCommand;
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
                if (pwModifyPWModifyBackCommand == null)
                {
                    pwModifyPWModifyBackCommand = new DelegateCommand(pwModifyPWModifyBack, canPWModifyPWModifyBack);
                }
                return pwModifyPWModifyBackCommand;
            }
            set
            {
                pwModifyPWModifyBackCommand = value;
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
                    pwModifyCaptchaResendCommond = new DelegateCommand(pwModifyCaptchaResend);
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
                pwModifyCaptchaResend();
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
            PromtInfo = captchaSend.Insert(PromtInfo.Length, Email);
            loginModel.sendPWCaptchaRequest();
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
        private void PasswordModifyMailResponseEventHandler(ProtoMessage message)
        {
            bool status = message.SignupMailResponse.Status;
            if(status)
            {
                // TODO: exit animate_3 and enter pwModify pw view
                eventAggregator.GetEvent<PWModifyMailNextEvent>().Publish(0);
            }
            else
            {
                // TODO: exit animate_3 and return spwModify mail view
                PromtInfo = emailNotExistMessage.Insert(0, Email);
            }
        }
        private void PasswordModifyResponseEventHandler(ProtoMessage message)
        {
            bool status = message.PassordModifyResponse.Status;
            if (status)
            {
                // TODO: enter congratulation
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