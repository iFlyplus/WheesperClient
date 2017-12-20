using PrismDemo.Model;
using PrismDemo.ModelDataAccess.Database;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PrismDemo.ModelDataAccess
{
  public static class Extensions
  {
    public static Customer AddCustomer(this CustomerData customerData, string firstName, string lastName, string address, string city, string state, string zipCode)
    {
      int customerNumber = customerData.Any() ? customerData.Max(m => m.CustomerNumber) + 1 : CustomerData.CustomerNumberSeed;
      Customer customer = new Customer() { CustomerNumber = customerNumber, FirstName = firstName, LastName = lastName, Address = address, City = city, State = state, ZipCode = zipCode };
      customerData.Add(customer);
      return customer;
    }

    public static Order AddOrder(this OrderData orderData, Customer customer, DateTime orderDate)
    {
      int orderNumber = orderData.Any() ? orderData.Max(m => m.OrderNumber) + 1 : OrderData.OrderNumberSeed;
      Order order = new Order() { CustomerNumber = customer.CustomerNumber, OrderNumber = orderNumber, OrderDate = orderDate, OrderDetails = new List<string>() };
      orderData.Add(order);
      return order;
    }

    public static Order AddOrderDetails(this Order order, string orderDetail)
    {
      order.OrderDetails.Add(orderDetail);
      return order;
    }
  }
}