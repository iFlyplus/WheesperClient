using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using System.Diagnostics;
using Wheesper.Infrastructure;
using Wheesper.Infrastructure.events;
using Wheesper.Login.events;
using Wheesper.Login.View;
using Wheesper.Login.ViewModel;
using System;

namespace Wheesper.Login
{
    public class LoginController
    {
        #region private menber
        private IUnityContainer container = null;
        private IEventAggregator eventAggregator = null;
        private IRegionManager regionManager = null;
        IRegion mainRegion = null;
        #endregion private menber

        #region view model
        private SigninViewModel signinViewModel = null;
        private SignupViewModel signupViewModel = null;
        private PWModifyViewModel pwModifyViewModel = null;
        #endregion view model

        #region view
        // make sure are there is the only view
        // this maybe sth not needed
        private SigninMailView signinMailView = null;
        private SigninPWView signinPWView = null;
        private SignupInfoView signupInfoView = null;
        private PWModifyMailView pwModifyMailView = null;
        private PWModifyPWView pwModifyPWView = null;
        private PWModifyCaptchaView pwModifyCaptchaView = null;
        private SignupDetailsView signupDetailsView = null;
        private SignupCaptchaView signupCaptchaView = null;
        private WelcomeView welcomeView = null;
        #endregion view

        #region Constructor & Deconstructor
        public LoginController(IUnityContainer container)
        {

            Debug.WriteLine("LoginController");
            this.container = container;
            eventAggregator = this.container.Resolve<IEventAggregator>();
            regionManager = this.container.Resolve<IRegionManager>();

            mainRegion = regionManager.Regions[RegionNames.MainRegion];

            if (signinMailView == null)
            {
                signinMailView = (SigninMailView)container.Resolve(typeof(SigninMailView));
                var viewModel = (SigninViewModel)container.Resolve(typeof(SigninViewModel));
                signinMailView.DataContext = viewModel;
            }

            loadView(signinMailView);

            // subscribe event to setup communication betwween different viewModel
            subEvent();
        }

        ~LoginController()
        {
            Debug.WriteLine("LoginController Destructor.");
        }
        #endregion Constructor & Deconstructor

        #region helper function
        private void subEvent()
        {
            Debug.WriteLine("LoginController subscribe evnet.");
            eventAggregator.GetEvent<SigninMailNextEvent>().Subscribe(signinMailNextEventHandler, ThreadOption.UIThread, true);
            eventAggregator.GetEvent<SigninPWBackEvent>().Subscribe(signinPWBackEventHandler, ThreadOption.UIThread, true);
            eventAggregator.GetEvent<SigninPWNextEvent>().Subscribe(signinPWNextEventHandler, ThreadOption.UIThread, true);
            eventAggregator.GetEvent<CreateAccountEvent>().Subscribe(createAccountEventHandler, ThreadOption.UIThread, true);
            eventAggregator.GetEvent<ForgetPWEvent>().Subscribe(forgetPWEventHandler, ThreadOption.UIThread, true);

            eventAggregator.GetEvent<SignupInfoNextEvent>().Subscribe(signupInfoNextEventHandler, ThreadOption.UIThread, true);
            eventAggregator.GetEvent<SignupInfoBackEvent>().Subscribe(signupInfoBackEventHandler, ThreadOption.UIThread, true);
            eventAggregator.GetEvent<SignupDetailsNextEvent>().Subscribe(signupDetailsNextEventHandler, ThreadOption.UIThread, true);
            eventAggregator.GetEvent<SignupDetailsBackEvent>().Subscribe(signupDetailsBackEventHandler, ThreadOption.UIThread, true);
            eventAggregator.GetEvent<SignupCaptchaNextEvent>().Subscribe(signupCaptchaNextEventHandler, ThreadOption.UIThread, true);
            eventAggregator.GetEvent<SignupCaptchaBackEvent>().Subscribe(signupCaptcahBackEventHandler, ThreadOption.UIThread, true);

            eventAggregator.GetEvent<PWModifyMailNextEvent>().Subscribe(pwModifyMailNextEventHandler, ThreadOption.UIThread, true);
            eventAggregator.GetEvent<PWModifyMailCancelEvent>().Subscribe(pwModifyMailCancelEventHander, ThreadOption.UIThread, true);
            eventAggregator.GetEvent<PWModifyPWNextEvent>().Subscribe(pwModifyPWNextEventHandler, ThreadOption.UIThread, true);
            eventAggregator.GetEvent<PWModifyPWBackEvent>().Subscribe(pwModifyPWBackEventHandler, ThreadOption.UIThread, true);
            eventAggregator.GetEvent<PWModifyCaptchaNextEvent>().Subscribe(pwModifyCaptchaNextEventHandler, ThreadOption.UIThread, true);
            eventAggregator.GetEvent<PWModifyCaptchaBackEvent>().Subscribe(pwModifyCaptchaBackEventHandler, ThreadOption.UIThread, true);
        }

        private void loadView(object view)
        {
            foreach (var v in mainRegion.Views)
            {
                mainRegion.Remove(v);
            }
            mainRegion.Add(view);
            mainRegion.Activate(view);
        }
        #endregion helper function

        #region event handler
        private void signinMailNextEventHandler(object o)
        {
            Debug.WriteLine("SigninMailNextEvent hadler");
            if (signinPWView==null)
            {
                signinPWView = (SigninPWView)container.Resolve(typeof(SigninPWView));
                var viewModel = (SigninViewModel)container.Resolve(typeof(SigninViewModel));
                signinPWView.DataContext = viewModel;
            }
            loadView(signinPWView);
        }
        private void signinPWNextEventHandler(object o)
        {
            Debug.WriteLine("SigninPWNextEvent hadler");
            eventAggregator.GetEvent<ShowWheesperViewEvent>().Publish(0);
        }
        private void signinPWBackEventHandler(string email)
        {
            Debug.WriteLine("SigninPWBackEvent hadler");
            loadView(signinMailView);
        }
        private void createAccountEventHandler(object o)
        {
            Debug.WriteLine("CreateAccountEvent hadler");
            if (signupInfoView == null)
            {
                signupInfoView = (SignupInfoView)container.Resolve(typeof(SignupInfoView));
                var viewModel = (SignupViewModel)container.Resolve(typeof(SignupViewModel));
                signupInfoView.DataContext = viewModel;
            }
            loadView(signupInfoView);
        }
        private void forgetPWEventHandler(string email)
        {
            Debug.WriteLine("ForgetPWEvent hadler");
            if (pwModifyMailView == null)
            {
                pwModifyMailView = (PWModifyMailView)container.Resolve(typeof(PWModifyMailView));
                var viewModel = (PWModifyViewModel)container.Resolve(typeof(PWModifyViewModel));
                pwModifyMailView.DataContext = viewModel;
            }
            loadView(pwModifyMailView);
        }

        private void signupInfoNextEventHandler(object o)
        {
            Debug.WriteLine("SignupInfoNextEvent hadler");
            if (signupDetailsView == null)
            {
                signupDetailsView = (SignupDetailsView)container.Resolve(typeof(SignupDetailsView));
                var viewModel = (SignupViewModel)container.Resolve(typeof(SignupViewModel));
                signupDetailsView.DataContext = viewModel;
            }
            loadView(signupDetailsView);
        }
        private void signupInfoBackEventHandler(object o)
        {
            Debug.WriteLine("SignupInfoBackEvent hadler");
            loadView(signinMailView);
        }
        private void signupDetailsNextEventHandler(object o)
        {
            Debug.WriteLine("SignupDetailsNextEvent hadler");
            if (signupCaptchaView == null)
            {
                signupCaptchaView = (SignupCaptchaView)container.Resolve(typeof(SignupCaptchaView));
                var viewModel = (SignupViewModel)container.Resolve(typeof(SignupViewModel));
                signupCaptchaView.DataContext = viewModel;
            }
            loadView(signupCaptchaView);
        }
        private void signupDetailsBackEventHandler(object o)
        {
            Debug.WriteLine("SignupDetailsBackEvent hadler");
            loadView(signupInfoView);
        }
        private void signupCaptchaNextEventHandler(string name)
        {
            Debug.WriteLine("SignupCaptchaNextEvent hadler");
            if (welcomeView == null)
            {
                welcomeView = (WelcomeView)container.Resolve(typeof(WelcomeView));
                var viewModel = (WelcomeViewModel)container.Resolve(typeof(WelcomeViewModel));

                string welcomeMessage_1 = "Hi, ";
                welcomeMessage_1 = welcomeMessage_1.Insert(welcomeMessage_1.Length, name);
                string welcomeMessage_2 = " Welcome to Wheesper!";
                viewModel.Initialize(welcomeMessage_1, welcomeMessage_2);

                welcomeView.DataContext = viewModel;
            }
            loadView(welcomeView);
        }
        private void signupCaptcahBackEventHandler(object o)
        {
            Debug.WriteLine("SignupCaptchaBackEvent hadler");
            loadView(signupDetailsView);
        }

        private void pwModifyMailNextEventHandler(object o)
        {
            Debug.WriteLine(" hadler");
            if (pwModifyPWView == null)
            {
                pwModifyPWView = (PWModifyPWView)container.Resolve(typeof(PWModifyPWView));
                var viewModel = (PWModifyViewModel)container.Resolve(typeof(PWModifyViewModel));
                pwModifyPWView.DataContext = viewModel;
            }
            loadView(pwModifyPWView);
        }
        private void pwModifyMailCancelEventHander(string email)
        {
            Debug.WriteLine("PWModifyMailCancelEvent hadler");
            loadView(signinPWView);
        }
        private void pwModifyPWNextEventHandler(object o)
        {
            Debug.WriteLine("PWModifyPWNextEvent hadler");
            if (pwModifyCaptchaView == null)
            {
                pwModifyCaptchaView = (PWModifyCaptchaView)container.Resolve(typeof(PWModifyCaptchaView));
                var viewModel = (PWModifyViewModel)container.Resolve(typeof(PWModifyViewModel));
                pwModifyCaptchaView.DataContext = viewModel;
            }
            loadView(pwModifyCaptchaView);
        }
        private void pwModifyPWBackEventHandler(object o)
        {
            Debug.WriteLine("PWModifyPWBackEvent hadler");
            loadView(pwModifyMailView);
        }
        private void pwModifyCaptchaNextEventHandler(object o)
        {
            Debug.WriteLine("PWModifyCaptchaNextEvent hadler");
            if (welcomeView == null)
            {
                welcomeView = (WelcomeView)container.Resolve(typeof(WelcomeView));
                var viewModel = (WelcomeViewModel)container.Resolve(typeof(WelcomeViewModel));

                string welcomeMessage_1 = "Congratulations!";
                string welcomeMessage_2 = " Password reset successful";
                viewModel.Initialize(welcomeMessage_1, welcomeMessage_2);

                welcomeView.DataContext = viewModel;
            }
            loadView(welcomeView);
        }
        private void pwModifyCaptchaBackEventHandler(object o)
        {
            Debug.WriteLine("PWModifyCaptchaBackEvent hadler");
            loadView(pwModifyPWView);
        }
        #endregion event handler
    }
}
