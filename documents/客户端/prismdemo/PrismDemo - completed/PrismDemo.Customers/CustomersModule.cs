using Microsoft.Practices.Prism.Logging;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using PrismDemo.Customers.Views;
using PrismDemo.Infrastructure;

namespace PrismDemo.Customers
{
  public class CustomersModule : IModule
  {
    private IUnityContainer container = null;
    private CustomersController controller = null;

    public CustomersModule(IUnityContainer container)
    {
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
      /// We are using the IOC container to get us a new CustomerController.
      this.controller = this.container.Resolve<CustomersController>();

      /// Using View Discovery
      /// 1. Get a reference to the RegionManager from the IOC Container.
      /// 2. Register the desired view(UserControl) with the region.
      /// See the code-behind of CustomerListView for setting the DataContext = ViewModel.
      var regionManager = this.container.Resolve<IRegionManager>();
      regionManager.RegisterViewWithRegion(RegionNames.BodyRegion, typeof(CustomerListView));

#if DEBUG
      var logger = this.container.Resolve<ILoggerFacade>();
      logger.Log("Customer Module Initialized", Category.Debug, Priority.None);
#endif
    }

    #endregion IModule Members
  }
}