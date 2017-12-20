using PrismDemo.ModelDataAccess.Database;
using System;

namespace PrismDemo.ModelDataAccess
{
  public class ModelDataEntities
  {
    public CustomerData Customers { get; private set; }

    public OrderData Orders { get; private set; }

    public ModelDataEntities()
    {
      LoadFakeData();
    }

    private void LoadFakeData()
    {
      this.Customers = new CustomerData();
      this.Orders = new OrderData();

      var aaronRodgers = this.Customers.AddCustomer("Aaron", "Rodgers", "1265 Lombardi Avenue", "Green Bay", "WI", "54304");

      this.Orders
        .AddOrder(aaronRodgers, DateTime.Today.AddDays(-21))
        .AddOrderDetails("Canoe")
        .AddOrderDetails("Paddle");
      this.Orders
        .AddOrder(aaronRodgers, DateTime.Today.AddDays(-14))
        .AddOrderDetails("Hammock")
        .AddOrderDetails("Frying Pan");

      var peytonManning = this.Customers.AddCustomer("Peyton", "Manning", "2755 West 17th Avenue", "Denver", "CO", "80204");

      this.Orders
        .AddOrder(peytonManning, DateTime.Today.AddDays(-11))
        .AddOrderDetails("Skis")
        .AddOrderDetails("Mountain Bike");
      this.Orders
        .AddOrder(peytonManning, DateTime.Today.AddDays(-4))
        .AddOrderDetails("Backpack")
        .AddOrderDetails("Binoculars");
    }
  }
}