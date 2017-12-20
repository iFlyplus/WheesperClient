using System.Collections.Generic;

namespace PrismDemo.Model
{
  public class Customer
  {
    public int CustomerNumber { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Address { get; set; }

    public string City { get; set; }

    public string State { get; set; }

    public string ZipCode { get; set; }

    public List<Order> Orders { get; set; }
  }
}