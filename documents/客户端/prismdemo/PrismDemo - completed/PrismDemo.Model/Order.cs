using System;
using System.Collections.Generic;

namespace PrismDemo.Model
{
  public class Order
  {
    public int CustomerNumber { get; set; }

    public int OrderNumber { get; set; }

    public DateTime OrderDate { get; set; }

    public List<string> OrderDetails { get; set; }
  }
}