using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.ViewModel;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Prism.Interactivity;
using System.Threading;
using System.Windows.Input;

using Wheesper.Login.Model;

namespace Wheesper.Login.ViewModel
{
    public class LoginViewModel : NotificationObject
    {
        private IUnityContainer container = null;
        private IEventAggregator eventAggragator = null;
        private LoginModel loginModel = null;

        #region Constructor
        public LoginViewModel(IUnityContainer container)
        {
            this.container = container;
            eventAggragator = this.container.Resolve<IEventAggregator>();
            loginModel = this.container.Resolve<LoginModel>();
        }
        #endregion Constructor

        #region properties
        public string EMail
        {
            get
            {
                return loginModel.email;
            }
            set
            {
                loginModel.email = value;
                RaisePropertyChanged("EMail");
            }
        }

        public string Password
        {
            set
            {
                loginModel.password = value;
            }
        }

        public string Nickname
        {
            get
            {
                return loginModel.nickname;
            }
            set
            {
                loginModel.nickname = value;
                RaisePropertyChanged("NickName");
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
            }
        }

        public string Sex
        {
            get
            {
                return loginModel.sex;
            }
            set
            {
                loginModel.sex = value;
                RaisePropertyChanged("Sex");
            }
        }

        public int Age
        {
            get
            {
                return loginModel.age;
            }
            set
            {
                loginModel.age = value;
                RaisePropertyChanged("Age");
            }
        }

        public string City
        {
            get
            {
                return loginModel.city;
            }
            set
            {
                loginModel.city = value;
                RaisePropertyChanged("City");
            }
        }

        public string Province
        {
            get
            {
                return loginModel.province;
            }
            set
            {
                loginModel.province = value;
                RaisePropertyChanged("Province");
            }
        }

        public string Country
        {
            get
            {
                return loginModel.country;
            }
            set
            {
                loginModel.country = value;
                RaisePropertyChanged("Country");
            }
        }
        #endregion proterties

        #region Command
        private DelegateCommand signinMailNextCommand;
        private DelegateCommand signinPWNextCommand;
        private DelegateCommand signinPWBackCommand;
        private DelegateCommand signinSigninCommand;
        private DelegateCommand createAccountCommand;
        private DelegateCommand forgetPWCommand;
        private DelegateCommand signupNextCommand;
        private DelegateCommand signupBackCommand;
        private DelegateCommand signupDetailsNextCommand;
        private DelegateCommand signupDetailsBackCommand;
        private DelegateCommand signupCaptchaNextCommand;
        private DelegateCommand signupCaptchaBackCommand;
        private DelegateCommand pwResetNextCommand;
        private DelegateCommand pwResetBackCommand;
        private DelegateCommand pwResetPWModifyNextCommand;
        private DelegateCommand pwResetPWModifyBackCommand;
        private DelegateCommand pwResetCaptchaNextCommand;
        private DelegateCommand pwResetCaptchaBackCommand;
        
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

        public DelegateCommand SigninSigninCommand
        {
            get
            {
                if (signinSigninCommand == null)
                {
                    signinSigninCommand = new DelegateCommand(signinSignin, canSigninSignin);
                }
                return signinSigninCommand;
            }
            set
            {
                signinSigninCommand = value;
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

        public DelegateCommand SignupNextCommand
        {
            get
            {
                if (signupNextCommand == null)
                {
                    signupNextCommand = new DelegateCommand(signupNext, canSignupNext);
                }
                return signupNextCommand;
            }
            set
            {
                signupNextCommand = value;
            }
        }

        public DelegateCommand SignupBackCommand
        {
            get
            {
                if (signupBackCommand == null)
                {
                    signupBackCommand = new DelegateCommand(signupBack, canSignupBack);
                }
                return signupBackCommand;
            }
            set
            {
                signupBackCommand = value;
            }
        }

        public DelegateCommand SignupDetailsNextCommand
        {
            get
            {
                if (signupDetailsNextCommand == null)
                {
                    signupDetailsNextCommand = new DelegateCommand(signupDetailsNext, canSignupDetailsNext);
                }
                return signupDetailsNextCommand;
            }
            set
            {
                signupDetailsNextCommand = value;
            }
        }

        public DelegateCommand SignupDetailsBackCommand
        {
            get
            {
                if (signupDetailsBackCommand == null)
                {
                    signupDetailsBackCommand = new DelegateCommand(signupDetailsBack, canSignupDetailsBack);
                }
                return signupDetailsBackCommand;
            }
            set
            {
                signupDetailsBackCommand = value;
            }
        }

        public DelegateCommand SignupCaptchaNextCommand
        {
            get
            {
                if (signupCaptchaNextCommand == null)
                {
                    signupCaptchaNextCommand = new DelegateCommand(signupCaptchaNext, canSignupCaptchaNext);
                }
                return signupCaptchaNextCommand;
            }
            set
            {
                signupCaptchaNextCommand = value;
            }
        }

        public DelegateCommand SignupCaptchaBackCommand
        {
            get
            {
                if (signupCaptchaBackCommand == null)
                {
                    signupCaptchaBackCommand = new DelegateCommand(signupCaptchaBack, canSignupCaptchaBack);
                }
                return signupCaptchaBackCommand;
            }
            set
            {
                signupCaptchaBackCommand = value;
            }
        }

        public DelegateCommand PWResetNextCommand
        {
            get
            {
                if (pwResetNextCommand == null)
                {
                    pwResetNextCommand = new DelegateCommand(pwResetNext, canPWResetNext);
                }
                return pwResetNextCommand;
            }
            set
            {
                pwResetNextCommand = value;
            }
        }

        public DelegateCommand PWResetBackCommand
        {
            get
            {
                if (pwResetBackCommand == null)
                {
                    pwResetBackCommand = new DelegateCommand(pwResetBack, canPWResetBack);
                }
                return pwResetBackCommand;
            }
            set
            {
                pwResetBackCommand = value;
            }
        }

        public DelegateCommand PWResetPWModifyNextCommand
        {
            get
            {
                if (pwResetPWModifyNextCommand == null)
                {
                    pwResetPWModifyNextCommand = new DelegateCommand(pwResetPWModifyNext, canPWResetPWModifyNext);
                }
                return pwResetPWModifyNextCommand;
            }
            set
            {
                pwResetPWModifyNextCommand = value;
            }
        }

        public DelegateCommand PWResetPWModifyBackCommand
        {
            get
            {
                if (pwResetPWModifyBackCommand == null)
                {
                    pwResetPWModifyBackCommand = new DelegateCommand(pwResetPWModifyBack, canPWResetPWModifyBack);
                }
                return pwResetPWModifyBackCommand;
            }
            set
            {
                pwResetPWModifyBackCommand = value;
            }
        }

        public DelegateCommand PWResetCaptchaNextCommand
        {
            get
            {
                if (pwResetCaptchaNextCommand == null)
                {
                    pwResetCaptchaNextCommand = new DelegateCommand(pwResetCaptchaNext, canPWResetCaptchaNext);
                }
                return pwResetCaptchaNextCommand;
            }
            set
            {
                pwResetCaptchaNextCommand = value;
            }
        }

        public DelegateCommand PWResetCaptchaBackCommand
        {
            get
            {
                if (pwResetCaptchaBackCommand == null)
                {
                    pwResetCaptchaBackCommand = new DelegateCommand(pwResetCaptchaBack, canPWResetCaptchaBack);
                }
                return pwResetCaptchaBackCommand;
            }
            set
            {
                pwResetCaptchaBackCommand = value;
            }
        }
        #endregion Command

        #region Command Delegate Method
        private void signinMailNext()
        {

        }
        private bool canSigninMailNext()
        {
            return true;
        }

        private void signinPWNext()
        {
        }
        private bool canSigninPWNext()
        {
            return true;
        }

        private void signinPWBack()
        {
        }
        private bool canSigninPWBack()
        {
            return true;
        }

        private void signinSignin()
        {
        }
        private bool canSigninSignin()
        {
            return true;
        }

        private void createAccount()
        {
        }
        private bool canCreateAccount()
        {
            return true;
        }

        private void forgetPW()
        {
        }
        private bool canForgetPW()
        {
            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        private void signupNext()
        {
        }
        private bool canSignupNext()
        {
            return true;
        }

        private void signupBack()
        {
        }
        private bool canSignupBack()
        {
            return true;
        }

        private void signupDetailsNext()
        {
        }
        private bool canSignupDetailsNext()
        {
            return true;
        }

        private void signupDetailsBack()
        {
        }
        private bool canSignupDetailsBack()
        {
            return true;
        }

        private void signupCaptchaNext()
        {
        }
        private bool canSignupCaptchaNext()
        {
            return true;
        }

        private void signupCaptchaBack()
        {
        }
        private bool canSignupCaptchaBack()
        {
            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        private void pwResetNext()
        {
        }
        private bool canPWResetNext()
        {
            return true;
        }

        private void pwResetBack()
        {
        }
        private bool canPWResetBack()
        {
            return true;
        }
        
        private void pwResetPWModifyNext()
        {
        }
        private bool canPWResetPWModifyNext()
        {
            return true;
        }

        private void pwResetPWModifyBack()
        {
        }
        private bool canPWResetPWModifyBack()
        {
            return true;
        }

        private void pwResetCaptchaNext()
        {
        }
        private bool canPWResetCaptchaNext()
        {
            return true;
        }

        private void pwResetCaptchaBack()
        {
        }
        private bool canPWResetCaptchaBack()
        {
            return true;
        }
        #endregion Command Delegate Method
    }
}
