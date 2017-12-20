using Microsoft.Practices.Prism.Logging;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using PrismDemo.Dashboard.Views;
using PrismDemo.Infrastructure;

namespace PrismDemo.Dashboard
{
  public class DashboardModule : IModule
  {
    private IUnityContainer container = null;
    private DashboardController controller = null;

    public DashboardModule(IUnityContainer container)
    {
      /// We are using the UnityContainer's constructor injection to get a reference to the container itself.
      this.container = container;
    }

    #region IModule Members

    /// <summary>
    /// Notifies the module that it has be initialized.
    /// </summary>
    /// <remarks>
    /// We are using the Initialize method to do two things:
    /// 1. Create a controller that will listen for any events to which this module wants to respond.
    /// 2. Register a view with the BodyRegion of the Shell.
    /// </remarks>
    public void Initialize()
    {
      /// Creating a controller for the module
      /// We are using the IOC container to get us a new DashboardController.
      this.controller = this.container.Resolve<DashboardController>();

      /// Using View Discovery
      /// 1. Get a reference to the RegionManager from the IOC Container.
      /// 2. Register the desired view(UserControl) with the region.
      /// See the code-behind of CustomerListView for setting the DataContext = ViewModel.
      var regionManager = this.container.Resolve<IRegionManager>();
      regionManager.RegisterViewWithRegion(RegionNames.HeaderRegion, typeof(ApplicationHeaderView));

      /// Using View Discovery
      /// 1. Get a reference to the RegionManager from the IOC Container.
      /// 2. Register the desired view(UserControl) with the region.
      /// See the code-behind of CustomerListView for setting the DataContext = ViewModel.
      regionManager.RegisterViewWithRegion(RegionNames.BodyRegion, typeof(DashboardView));

#if DEBUG
      var logger = this.container.Resolve<ILoggerFacade>();
      logger.Log("Dashboard Module Initialized", Category.Debug, Priority.None);
#endif
    }

    #endregion IModule Members
  }
}