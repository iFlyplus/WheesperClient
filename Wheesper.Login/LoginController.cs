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
        private IUnityContainer container = null;
        private IEventAggregator eventAggregator = null;
        private IRegionManager regionManager = null;
        IRegion mainRegion = null;
        IRegion loginFunctionRegion = null;
        #region Constructor
        public LoginController(IUnityContainer container)
        {

            Debug.WriteLine("LoginController");
            this.container = container;
            eventAggregator = this.container.Resolve<IEventAggregator>();
            regionManager = this.container.Resolve<IRegionManager>();

            mainRegion = regionManager.Regions[RegionNames.MainRegion];
            // loginFunctionRegion = regionManager.Regions[RegionNames.LoginFunctionRegion];

            subEvent();
            loadSigninMailView(null);
        }
        #endregion Constructor


        #region helper function
        private void subEvent()
        {
            Debug.WriteLine("subEvent");
            // , ThreadOption.UIThread
            eventAggregator.GetEvent<ShowLoginFaceViewEvent>().Subscribe(loadSigninMailView);
            eventAggregator.GetEvent<SigninMailNextEvent>().Subscribe(signinMailNextEventHandler);
            eventAggregator.GetEvent<SigninPWBackEvent>().Subscribe(signinPWBackEventHandler);
            eventAggregator.GetEvent<SigninPWSigninEvent>().Subscribe(signinPWSigninEventHandler);
            eventAggregator.GetEvent<CreateAccountEvent>().Subscribe(createAccountEventHandler);
            eventAggregator.GetEvent<ForgetPWEvent>().Subscribe(forgetPWEventHandler);

            eventAggregator.GetEvent<SignupInfoNextEvent>().Subscribe(signupInfoNextEventHandler);
            eventAggregator.GetEvent<SignupInfoBackEvent>().Subscribe(signupInfoBackEventHandler);
            eventAggregator.GetEvent<SignupDetailsNextEvent>().Subscribe(signupDetailsNextEventHandler);
            eventAggregator.GetEvent<SignupDetailsBackEvent>().Subscribe(signupDetailsBackEventHandler);
            eventAggregator.GetEvent<SignupCaptchaNextEvent>().Subscribe(signupCaptchaNextEventHandler);
            eventAggregator.GetEvent<SignupCaptchaBackEvent>().Subscribe(signupCaptcahBackEventHandler);

            eventAggregator.GetEvent<PWModifyMailNextEvent>().Subscribe(pwModifyMailNextEventHandler);
            eventAggregator.GetEvent<PWModifyMailCancelEvent>().Subscribe(pwModifyMailCancelEventHander);
            eventAggregator.GetEvent<PWModifyPWNextEvent>().Subscribe(pwModifyPWNextEventHandler);
            eventAggregator.GetEvent<PWModifyPWBackEvent>().Subscribe(pwModifyPWBackEventHandler);
            eventAggregator.GetEvent<PWModifyCaptchaNextEvent>().Subscribe(pwModifyCaptchaNextEventHandler);
            eventAggregator.GetEvent<PWModifyCaptchaBackEvent>().Subscribe(pwModifyCaptchaBackEventHandler);
        }
        private void loadSigninMailView(string email)
        {
            //var viewModel = container.Resolve<SigninViewModel>();
            var view = container.Resolve<SigninMailView>();
            var viewModel = (SigninViewModel)container.Resolve(typeof(SigninViewModel));
            viewModel.Initialize(email);
            view.DataContext = viewModel;
            loadView(view);
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
        /*
        private void loadLoginFucntionView()
        {

            var loginFucntionView = container.Resolve<LoginFunctionView>();
            foreach (var v in mainRegion.Views)
            {
                mainRegion.Remove(v);
            }
            mainRegion.Activate(loginFucntionView);
        }
        
        private void loadFunc2FunctionRegion(object view)
        {
            foreach(var v in loginFunctionRegion.Views)
            {
                loginFunctionRegion.Remove(v);
            }
            loginFunctionRegion.Add(view);
            loginFunctionRegion.Activate(view);
        }
        */
        #endregion helper function

        #region event handler
        private void signinMailNextEventHandler(object o)
        {
            Debug.WriteLine("next hadler");
            // var viewModel = container.Resolve<SigninViewModel>();
            //container.RegisterInstance<SigninViewModel>(viewModel);
            //container.
            var viewModel = (SigninViewModel)container.Resolve(typeof(SigninViewModel));
            var view = container.Resolve<SigninPWView>();
            view.DataContext = viewModel;
            loadView(view);
        }
        private void signinPWSigninEventHandler(object o)
        {
            eventAggregator.GetEvent<ShowWheesperViewEvent>().Publish(0);
        }
        private void signinPWBackEventHandler(string email)
        {
            loadSigninMailView(email);
        }
        private void createAccountEventHandler(object o)
        {
            var viewModel = container.Resolve<SignupViewModel>();
            var view = container.Resolve<SignupInfoView>();
            viewModel.Initialize(null);
            view.DataContext = viewModel;
            loadView(view);
        }
        private void forgetPWEventHandler(string email)
        {
            var viewModel = container.Resolve<PWModifyViewModel>();
            var view = container.Resolve<PWModifyMailView>();
            viewModel.Initialize(email);
            view.DataContext = viewModel;
            loadView(view);
        }

        private void signupInfoNextEventHandler(object o)
        {
            var viewModel = container.Resolve<SignupViewModel>();
            var view = container.Resolve<SignupDetailsView>();
            view.DataContext = viewModel;
            loadView(view);
        }
        private void signupInfoBackEventHandler(object o)
        {
            var viewModel = container.Resolve<SignupViewModel>();
            var view = container.Resolve<SignupInfoView>();
            view.DataContext = viewModel;
            loadView(view);
        }
        private void signupDetailsNextEventHandler(object o)
        {
            var viewModel = container.Resolve<SignupViewModel>();
            var view = container.Resolve<SignupCaptchaView>();
            view.DataContext = viewModel;
            loadView(view);
        }
        private void signupDetailsBackEventHandler(object o)
        {
            var viewModel = container.Resolve<SignupViewModel>();
            var view = container.Resolve<SignupInfoView>();
            view.DataContext = viewModel;
            loadView(view);
        }
        private void signupCaptchaNextEventHandler(string name)
        {
            var viewModel = container.Resolve<WelcomeViewModel>();
            var view = container.Resolve<WelcomeView>();
            string welcomeMessage_1 = "Hi, ";
            welcomeMessage_1 = welcomeMessage_1.Insert(welcomeMessage_1.Length, name);
            viewModel.Initialize(welcomeMessage_1, " Welcome to Wheesper!");
            view.DataContext = viewModel;
            loadView(view);
        }
        private void signupCaptcahBackEventHandler(object o)
        {
            var viewModel = container.Resolve<SignupViewModel>();
            var view = container.Resolve<SignupDetailsView>();
            view.DataContext = viewModel;
            loadView(view);
        }

        private void pwModifyMailNextEventHandler(object o)
        {
            var viewModel = container.Resolve<PWModifyViewModel>();
            var view = container.Resolve<PWModifyPWView>();
            view.DataContext = viewModel;
            loadView(view);
        }
        private void pwModifyMailCancelEventHander(string email)
        {
            var viewModel = container.Resolve<SigninViewModel>();
            var view = container.Resolve<SigninPWView>();
            viewModel.Initialize(email);
            view.DataContext = viewModel;
            loadView(view);
        }
        private void pwModifyPWNextEventHandler(object o)
        {
            var viewModel = container.Resolve<PWModifyViewModel>();
            var view = container.Resolve<PWModifyCaptchaView>();
            view.DataContext = viewModel;
            loadView(view);
        }
        private void pwModifyPWBackEventHandler(object o)
        {
            var viewModel = container.Resolve<PWModifyViewModel>();
            var view = container.Resolve<PWModifyMailView>();
            view.DataContext = viewModel;
            loadView(view);
        }
        private void pwModifyCaptchaNextEventHandler(object o)
        {
            var viewModel = container.Resolve<WelcomeViewModel>();
            var view = container.Resolve<WelcomeView>();
            viewModel.Initialize("Congratulations!", " Password reset successful");
            view.DataContext = viewModel;
            loadView(view);
        }
        private void pwModifyCaptchaBackEventHandler(object o)
        {
            var viewModel = container.Resolve<PWModifyViewModel>();
            var view = container.Resolve<PWModifyPWView>();
            view.DataContext = viewModel;
            loadView(view);
        }
        #endregion event handler
    }
}
