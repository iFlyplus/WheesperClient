using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Data;

namespace Wheesper.Chat.Model
{
    public class ContactList
    {
        public ContactList(string listname)
        {
            Groupname = listname;

            Contacts = new ListCollectionView(contacts);
            Contacts.CurrentChanged += contactSelectedItemChanged;
        }

        public string Groupname
        {
            get { return groupname; }
            set { groupname = value; }
        }
        public ListCollectionView Contacts { get; private set; }

        private string groupname = null;
        private ObservableCollection<Contact> contacts = new ObservableCollection<Contact>();

        public void Add(Contact contact)
        {
            contacts.Add(contact);
        }
        public void Remove(Contact contact)
        {
            contacts.Remove(contact);
        }

        private void contactSelectedItemChanged(object sender, EventArgs e)
        {
            /*
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
             */
            Contact currentContact = Contacts.CurrentItem as Contact;
            Debug.WriteLine("current select contact:");
            Debug.WriteLine(currentContact.EMail);
        }
    }
}
