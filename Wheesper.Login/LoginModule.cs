using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;
using Wheesper.Login.ViewModel;
using Wheesper.Login.Model;

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

        }
        #endregion Constructor

        # region Method
        private void RegisterInstanceEventArgs()
        {
            LoginModel loginModel = container.Resolve<LoginModel>();
            SigninViewModel signinViewModel = container.Resolve<SigninViewModel>();
            SignupViewModel signupViewModel = container.Resolve<SignupViewModel>();
            PWModifyViewModel pwModifyViewModel = container.Resolve<PWModifyViewModel>();

            container.RegisterInstance(typeof(LoginModel), loginModel);
            container.RegisterInstance(typeof(SigninViewModel), signinViewModel);
            container.RegisterInstance(typeof(SignupViewModel), signupViewModel);
            container.RegisterInstance(typeof(PWModifyViewModel), pwModifyViewModel);
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
