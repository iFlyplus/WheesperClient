using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;

using System.Diagnostics;

namespace Wheesper.Chat
{
    public class ChatModule : IModule
    {
        private IUnityContainer container = null;
        private ChatController controller = null;

        #region constructor
        public ChatModule(IUnityContainer container)
        {
            Debug.WriteLine("ChatModule constructor");
            this.container = container;
        }
        #endregion constructor

        #region IModule method
        public void Initialize()
        {
            controller = container.Resolve<ChatController>();
        }
        #endregion IModule method
    }
}
