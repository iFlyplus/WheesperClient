using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;

using System.Diagnostics;

namespace Wheesper.Test
{
    public class TestModule : IModule
    {
        #region private member
        IUnityContainer container = null;
        TestController controller = null;
        #endregion private member

        #region constructor
        public TestModule(IUnityContainer container)
        {
            Debug.WriteLine("TestModule constructor");
            this.container = container;
        }
        #endregion constructor

        #region IModule Method
        public void Initialize()
        {
            controller = container.Resolve<TestController>();
        }
        #endregion IModule Method
    }
}
