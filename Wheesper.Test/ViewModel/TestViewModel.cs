using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.ViewModel;
using Microsoft.Practices.Unity;

using System.Text.RegularExpressions;
using System.Diagnostics;
using Prism.Mvvm;
using System;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Data;

namespace Wheesper.Test.ViewModel
{
    public class TestViewModel:BindableBase
    {
        #region private menber
        private IUnityContainer container = null;
        private IEventAggregator eventAggregator = null;
        #endregion private menber
        private bool isInit = true;

        public ListCollectionView Customers { get; private set; }

        ObservableCollection<Customer> customers = new ObservableCollection<Customer>();
        #region Constructor
        public TestViewModel(IUnityContainer container)
        {
            Debug.WriteLine("SigninViewModel constructor");
            this.container = container;
            eventAggregator = this.container.Resolve<IEventAggregator>();

            Customers = new ListCollectionView(customers);
            customers.Add(new Customer("A", 1));
            customers.Add(new Customer("B", 2));
            customers.Add(new Customer("C", 3));
            Customers.CurrentChanged += SelectedItemChanged;
        }
        #endregion Constructor

        private void SelectedItemChanged(object sender, EventArgs e)
        {
            if (isInit == true)
            {
                isInit = false;
                return;
            }
            Customer current = Customers.CurrentItem as Customer;
            Debug.WriteLine(current.Name);
            Debug.WriteLine(current.Age);
            //Customers.AddNewItem(current);
            customers.Add(new Customer(current.Name, current.Age));
        }
    }

    public class Customer:NotificationObject
    {
        public string Name { get; private set; }
        public int Age { get { return age; } private set { age = value; } }

        private int age;
        public Customer(string name, int age)
        {
            Name = name;
            this.age = age;
        }
    }
}
