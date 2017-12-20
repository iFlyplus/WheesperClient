using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;

namespace Wheesper.Messaging
{
    class MessagingModule : IModule
    {
        private IUnityContainer container = null;

        public MessagingModule(IUnityContainer container)
        {
            this.container = container;
        }

        #region IModule Members
        public void Initialize()
        {
            throw new System.NotImplementedException();
        }
        #endregion IModule Members
    }
}
