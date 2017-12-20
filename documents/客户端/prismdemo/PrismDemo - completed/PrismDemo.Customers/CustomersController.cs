using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.Logging;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using PrismDemo.Customers.Events;
using PrismDemo.Customers.ViewModels;
using PrismDemo.Customers.Views;
using PrismDemo.Infrastructure;
using PrismDemo.Model;

namespace PrismDemo.Customers
{
  /// <summary>
  /// The job of the controller is to listen and respond to events. The most frequent action of the controller is to
  /// inject a view into a region or to remove a view from a region.
  /// </summary>
  public class CustomersController
  {
    private IUnityContainer container = null;
    private ILoggerFacade logger = null;
    private IRegionManager regionManager = null;
    private IEventAggregator eventAggregator = null;

    public CustomersController(IUnityContainer container)
    {
      this.container = container;
      this.logger = this.container.Resolve<ILoggerFacade>();
      this.eventAggregator = this.container.Resolve<IEventAggregator>();
      this.regionManager = this.container.Resolve<IRegionManager>();

      /// ***  The controller remains active because event subscriptions(see keepSubscriberReferenceAlive) - but that is it's reason for existence. ***
      eventAggregator.GetEvent<ShowCustomerEvent>().Subscribe(ShowCustomer, true);
    }

    // Using View Injection to show the CustomerView.
    private void ShowCustomer(Customer customer)
    {
      var region = this.regionManager.Regions[RegionNames.CustomerRegion];
      var viewModel = this.container.Resolve<CustomerViewModel>();
      viewModel.Initialize(customer);
      var view = this.container.Resolve<CustomerView>();
      foreach (var v in region.Views)
      {
        region.Remove(v);
      }
      region.Add(view, typeof(CustomerView).Name);
      view.DataContext = viewModel;
      region.Activate(view);
    }
  }
}