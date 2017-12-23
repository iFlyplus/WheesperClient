using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;

using System.Diagnostics;
using Wheesper.Chat.ViewModel;

namespace Wheesper.Chat
{
    public class ChatModule : IModule
    {
        #region private member
        IUnityContainer container = null;
        ChatController controller = null;
        #endregion private member

        #region constructor
        public ChatModule(IUnityContainer container)
        {
            Debug.WriteLine("ChatModule constructor");
            this.container = container;

            RegisterInstance();
        }
        #endregion constructor

        # region Private Method
        private void RegisterInstance()
        {
            //LoginModel loginModel = container.Resolve<LoginModel>();
            //container.RegisterInstance(typeof(LoginModel), loginModel);

            ChatViewModel chatViewModel = container.Resolve<ChatViewModel>();
            container.RegisterInstance(typeof(ChatViewModel), chatViewModel);

            SolveContactApplyViewModel solveContactApplyViewModel = container.Resolve<SolveContactApplyViewModel>();
            container.RegisterInstance(typeof(SolveContactApplyViewModel), solveContactApplyViewModel);
        }
        # endregion Private Method

        #region IModule method
        public void Initialize()
        {
            controller = container.Resolve<ChatController>();
        }
        #endregion IModule method
    }
}
