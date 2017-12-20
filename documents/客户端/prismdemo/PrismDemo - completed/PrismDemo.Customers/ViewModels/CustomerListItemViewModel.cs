using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.Logging;
using Microsoft.Practices.Prism.ViewModel;
using Microsoft.Practices.Unity;
using PrismDemo.Infrastructure.Services;
using PrismDemo.Model;

namespace PrismDemo.Customers.ViewModels
{
  public class CustomerListItemViewModel : NotificationObject
  {
    private IUnityContainer container = null;
    private ILoggerFacade logger = null;
    private IModelService modelService = null;
    private IEventAggregator eventAggregator = null;

    #region Customer

    private Customer customer;

    public Customer Customer
    {
      get
      {
        return customer;
      }

      set
      {
        customer = value;
        RaisePropertyChanged(() => this.Customer);
      }
    }

    #endregion Customer

    #region First Name

    private string firstName = string.Empty;

    public string FirstName
    {
      get
      {
        if (this.customer != null)
        {
          this.firstName = this.customer.FirstName;
        }
        return firstName;
      }

      set
      {
        firstName = value;
        RaisePropertyChanged(() => this.FirstName);
      }
    }

    #endregion First Name

    #region Last Name

    private string lastName = string.Empty;

    public string LastName
    {
      get
      {
        if (this.customer != null)
        {
          this.lastName = this.customer.LastName;
        }
        return lastName;
      }

      set
      {
        lastName = value;
        RaisePropertyChanged(() => this.LastName);
      }
    }

    #endregion Last Name

    public CustomerListItemViewModel(IUnityContainer container)
    {
      this.container = container;
      this.logger = this.container.Resolve<ILoggerFacade>();
      this.eventAggregator = this.container.Resolve<IEventAggregator>();
      this.modelService = this.container.Resolve<IModelService>();
    }

    public void Initialize(Customer customer)
    {
      this.Customer = customer;
    }
  }
}