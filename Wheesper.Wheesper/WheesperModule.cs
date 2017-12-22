using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;

using System.Diagnostics;

namespace Wheesper.Wheesper
{
    public class WheesperModule : IModule
    {
        #region private member
        IUnityContainer container = null;
        WheesperController controller = null;
        #endregion private member

        #region constructor
        public WheesperModule(IUnityContainer container)
        {
            Debug.WriteLine("WheesperModule constructor");
            this.container = container;
        }
        #endregion constructor

        #region IModule Method
        public void Initialize()
        {
            controller = container.Resolve<WheesperController>();
        }
        #endregion IModule Method
    }
}
