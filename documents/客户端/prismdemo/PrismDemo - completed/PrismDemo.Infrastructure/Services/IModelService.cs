using PrismDemo.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace PrismDemo.Infrastructure.Services
{
  public interface IModelService
  {
    Customer GetCustomer(int customerNumber);
    Collection<Customer> GetAllCustomers();
    Order GetOrder(int orderNumber);
    Collection<Order> GetAllOrders();
    Collection<Order> GetAllCustomerOrders(Customer customer);
    Collection<Customer> GetAllCustomersAndOrders();
  }
}
