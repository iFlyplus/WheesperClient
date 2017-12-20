using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.Logging;
using Microsoft.Practices.Prism.ViewModel;
using Microsoft.Practices.Unity;
using PrismDemo.Customers.Events;
using PrismDemo.Infrastructure.Services;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace PrismDemo.Customers.ViewModels
{
  public class CustomerListViewModel : NotificationObject
  {
    private IUnityContainer container = null;
    private ILoggerFacade logger = null;
    private IModelService modelService = null;
    private IEventAggregator eventAggregator = null;

    #region Title

    private string title = "Customers";

    public string Title
    {
      get { return title; }
      set { title = value; }
    }

    #endregion Title

    #region Customer List

    private ObservableCollection<CustomerListItemViewModel> customerList = new ObservableCollection<CustomerListItemViewModel>();

    public ObservableCollection<CustomerListItemViewModel> CustomerList
    {
      get
      {
        return customerList;
      }

      set
      {
        customerList = value;
        RaisePropertyChanged(() => this.CustomerList);
      }
    }

    #endregion Customer List

    #region View Customer Command

    private ICommand viewCustomerOrdersCommand;

    public ICommand ViewCustomerOrdersCommand
    {
      get
      {
        if (this.viewCustomerOrdersCommand == null)
        {
          this.viewCustomerOrdersCommand = new DelegateCommand<CustomerListItemViewModel>(this.ViewCustomer);
        }
        return this.viewCustomerOrdersCommand;
      }

      set
      {
        this.viewCustomerOrdersCommand = value;
      }
    }

    public void ViewCustomer(CustomerListItemViewModel selectedCustomerViewModel)
    {
      this.eventAggregator.GetEvent<ShowCustomerEvent>().Publish(selectedCustomerViewModel.Customer);
    }

    #endregion View Customer Command

    public CustomerListViewModel(IUnityContainer container)
    {
      this.container = container;
      this.logger = this.container.Resolve<ILoggerFacade>();
      this.eventAggregator = this.container.Resolve<IEventAggregator>();
      this.modelService = this.container.Resolve<IModelService>();
    }

    public void Initialize()
    {
      var customers = this.modelService.GetAllCustomers().OrderBy(o => o.LastName).ThenBy(o => o.FirstName);
      if (customers != null)
      {
        foreach (var customer in customers)
        {
          var vm = this.container.Resolve<CustomerListItemViewModel>();
          vm.Initialize(customer);
          this.CustomerList.Add(vm);
        }
      }
    }
  }
}