using PrismDemo.Infrastructure.Services;
using PrismDemo.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace PrismDemo.ModelDataAccess
{
  public class ModelService : IModelService
  {
    private ModelDataEntities entities = null;

    public ModelService()
    {
      this.entities = new ModelDataEntities();
    }

    #region IModelService Members

    public Customer GetCustomer(int customerNumber)
    {
      Customer customer = null;
      customer = this.entities.Customers.FirstOrDefault(f => f.CustomerNumber == customerNumber);
      return customer;
    }

    public Collection<Customer> GetAllCustomers()
    {
      var customers = new Collection<Customer>();
      foreach (var data in this.entities.Customers)
      {
        customers.Add(data);
      }
      return customers;
    }

    public Order GetOrder(int orderNumber)
    {
      Order order = new Order();
      order = this.entities.Orders.FirstOrDefault(f => f.OrderNumber == orderNumber);
      return order;
    }

    public Collection<Order> GetAllOrders()
    {
      var orders = new Collection<Order>();
      foreach (Order data in this.entities.Orders)
      {
        orders.Add(data);
      }
      return orders;
    }

    public Collection<Order> GetAllCustomerOrders(Customer customer)
    {
      var orders = new Collection<Order>();
      foreach (Order data in this.entities.Orders.Where(w => w.CustomerNumber == customer.CustomerNumber))
      {
        orders.Add(data);
      }
      return orders;
    }

    public Collection<Customer> GetAllCustomersAndOrders()
    {
      var customersAndOrders = new Collection<Customer>();

      this.entities.Customers.ToList().ForEach(f =>
        {
          f.Orders = this.entities.Orders.Where(w => w.CustomerNumber == f.CustomerNumber).ToList();
          customersAndOrders.Add(f);
        });

      return customersAndOrders;
    }

    #endregion IModelService Members
  }
}