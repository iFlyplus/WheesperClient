using Microsoft.Practices.Prism.Events;
using PrismDemo.Model;

namespace PrismDemo.Customers.Events
{
  /// <summary>
  /// Event that requests a Customer
  /// The TPayload of the event represents the Customer which will be passed to the subscriber to the event.
  /// </summary>
  public class ShowCustomerEvent : CompositePresentationEvent<Customer> { }
}