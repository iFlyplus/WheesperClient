using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using Wheesper.Infrastructure;

using System.Diagnostics;

namespace Wheesper.Chat
{
    public class ChatController
    {
        private IUnityContainer container = null;
        private IEventAggregator eventAggregator = null;
        private IRegionManager regionManager = null;
        IRegion mainRegion = null;

        #region constructor
        public ChatController(IUnityContainer container)
        {
            Debug.WriteLine("ChatController constructor");
            this.container = container;

            eventAggregator = this.container.Resolve<IEventAggregator>();
            regionManager = this.container.Resolve<IRegionManager>();

            mainRegion = regionManager.Regions[RegionNames.MainRegion];
        }
        #endregion constructor
    }
}
