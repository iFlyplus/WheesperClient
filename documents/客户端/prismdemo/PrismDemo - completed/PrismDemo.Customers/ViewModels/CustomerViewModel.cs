using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.ViewModel;
using Microsoft.Practices.Unity;
using PrismDemo.Infrastructure.Events;
using PrismDemo.Infrastructure.Services;
using PrismDemo.Model;
using System.Windows.Input;

namespace PrismDemo.Customers.ViewModels
{
  public class CustomerViewModel : NotificationObject
  {
    private IUnityContainer container = null;
    private IModelService modelService = null;
    private IEventAggregator eventAggregator = null;

    private Customer customer = null;

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

    #region Address

    private string address = string.Empty;

    public string Address
    {
      get
      {
        if (this.customer != null)
        {
          this.address = this.customer.Address;
        }
        return address;
      }

      set
      {
        address = value;
        RaisePropertyChanged(() => this.Address);
      }
    }

    #endregion Address

    #region City

    private string city = string.Empty;

    public string City
    {
      get
      {
        if (this.customer != null)
        {
          this.city = this.customer.City;
        }
        return city;
      }

      set
      {
        city = value;
        RaisePropertyChanged(() => this.City);
      }
    }

    #endregion City

    #region State

    private string state = string.Empty;

    public string State
    {
      get
      {
        if (this.customer != null)
        {
          this.state = this.customer.State;
        }
        return state;
      }

      set
      {
        state = value;
        RaisePropertyChanged(() => this.State);
      }
    }

    #endregion State

    #region ZipCode

    private string zipCode = string.Empty;

    public string ZipCode
    {
      get
      {
        if (this.customer != null)
        {
          this.zipCode = this.customer.ZipCode;
        }
        return zipCode;
      }

      set
      {
        zipCode = value;
        RaisePropertyChanged(() => this.ZipCode);
      }
    }

    #endregion ZipCode

    #region View Outstanding Orders Command

    private ICommand viewOutstandingOrdersCommand;

    public ICommand ViewOutstandingOrdersCommand
    {
      get
      {
        if (this.viewOutstandingOrdersCommand == null)
        {
          this.viewOutstandingOrdersCommand = new DelegateCommand(this.ViewOutstandingOrders);
        }
        return this.viewOutstandingOrdersCommand;
      }

      set
      {
        this.viewOutstandingOrdersCommand = value;
      }
    }

    public void ViewOutstandingOrders()
    {
      // No business logic here, just showing that we can send a message from here.
      this.eventAggregator.GetEvent<ShowApplicationMessageEvent>().Publish("There are no outstanding orders for this customer");
    }

    #endregion View Outstanding Orders Command

    public CustomerViewModel(IUnityContainer container)
    {
      this.container = container;
      this.eventAggregator = this.container.Resolve<IEventAggregator>();
      this.modelService = this.container.Resolve<IModelService>();
    }

    public void Initialize(Customer customer)
    {
      this.customer = customer;
    }
  }
}