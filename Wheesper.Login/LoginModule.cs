using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;
using Wheesper.Login.ViewModel;
using Wheesper.Login.Model;
using Wheesper.Login.View;

namespace Wheesper.Login
{
    public class LoginModule : IModule
    {
        private IUnityContainer container = null;
        private LoginController controller = null;

        #region Constructor
        public LoginModule(IUnityContainer container)
        {
            this.container = container;
            RegisterInstance();
        }
        #endregion Constructor

        # region Private Method
        private void RegisterInstance()
        {
            LoginModel loginModel = container.Resolve<LoginModel>();
            container.RegisterInstance(typeof(LoginModel), loginModel);

            SigninViewModel signinViewModel = container.Resolve<SigninViewModel>();
            container.RegisterInstance(typeof(SigninViewModel), signinViewModel);
            SignupViewModel signupViewModel = container.Resolve<SignupViewModel>();
            container.RegisterInstance(typeof(SignupViewModel), signupViewModel);
            PWModifyViewModel pwModifyViewModel = container.Resolve<PWModifyViewModel>(); 
            container.RegisterInstance(typeof(PWModifyViewModel), pwModifyViewModel);


            SigninMailView signinMailView = container.Resolve<SigninMailView>();
            container.RegisterInstance(typeof(SigninMailView), signinMailView);
            SigninPWView signinPWView = container.Resolve<SigninPWView>();
            container.RegisterInstance(typeof(SigninPWView), signinPWView);
            /*

            SignupInfoView signupInfoView = container.Resolve<SignupInfoView>();
            container.RegisterInstance(typeof(SignupInfoView), signupInfoView);
            SignupDetailsView signupDetailsView = container.Resolve<SignupDetailsView>();
            container.RegisterInstance(typeof(SignupDetailsView), signupDetailsView);
            SignupCaptchaView signupCaptchaView = container.Resolve<SignupCaptchaView>();
            container.RegisterInstance(typeof(SignupCaptchaView), signupCaptchaView);
            PWModifyMailView pwModifyMailView = container.Resolve<PWModifyMailView>();
            container.RegisterInstance(typeof(PWModifyMailView), pwModifyMailView);
            PWModifyCaptchaView pwModifyCaptchaView = container.Resolve<PWModifyCaptchaView>();
            container.RegisterInstance(typeof(PWModifyCaptchaView), pwModifyCaptchaView);
            PWModifyPWView pwModifyPWView = container.Resolve<PWModifyPWView>();
            container.RegisterInstance(typeof(PWModifyPWView), pwModifyPWView);
            WelcomeView welcomeView = container.Resolve<WelcomeView>();
            container.RegisterInstance(typeof(WelcomeView), welcomeView);
            */

        }
        # endregion Private Method

        #region IModule Methods
        public void Initialize()
        {
            controller = container.Resolve<LoginController>();
        }
        #endregion IModele Methods
    }
}
