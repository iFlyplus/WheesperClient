using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using Wheesper.Infrastructure;
using Wheesper.Infrastructure.events;

using System.Diagnostics;

namespace Wheesper.Wheesper
{
    public class WheesperController
    {
        #region private member
        IUnityContainer container = null;
        IEventAggregator eventAggregator = null;
        IRegionManager regionManager = null;
        IRegion mainRegion = null;
        #endregion private member

        #region constructor
        public WheesperController(IUnityContainer container)
        {
            Debug.WriteLine("ContactController constructor");
            this.container = container;
            eventAggregator = this.container.Resolve<IEventAggregator>();
            regionManager = this.container.Resolve<IRegionManager>();
            mainRegion = regionManager.Regions[RegionNames.MainRegion];
        }
        #endregion constructor

        #region helper function
        private void subevent()
        {
            eventAggregator.GetEvent<LoginSucceedEvent>().Subscribe(null, ThreadOption.UIThread, true);
        }
        #endregion helpfunction

        #region event handler

        #endregion event handler
    }
}
