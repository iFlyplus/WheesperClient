using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using Wheesper.Infrastructure;
using Wheesper.Infrastructure.events;

using System.Diagnostics;
using Wheesper.Test.View;
using Wheesper.Test.ViewModel;

namespace Wheesper.Test
{
    public class TestController
    {
        #region private member
        IUnityContainer container = null;
        IEventAggregator eventAggregator = null;
        IRegionManager regionManager = null;
        IRegion mainRegion = null;
        #endregion private member

        #region view
        private TestView testView = null;
        #endregion view

        #region helper function
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

        #region constructor
        public TestController(IUnityContainer container)
        {
            Debug.WriteLine("ContactController constructor");
            this.container = container;
            eventAggregator = this.container.Resolve<IEventAggregator>();
            regionManager = this.container.Resolve<IRegionManager>();
            mainRegion = regionManager.Regions[RegionNames.MainRegion];

            if (testView == null)
            {
                testView = (TestView)container.Resolve(typeof(TestView));
                var viewModel = (TestViewModel)container.Resolve(typeof(TestViewModel));
                testView.DataContext = viewModel;
            }
            loadView(testView);
        }
        #endregion constructor
    }
}
