using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Data;

namespace Wheesper.Desktop
{
    public class ContactList
    {
        public ContactList(string listname)
        {
            Groupname = listname;

            Contacts = new ListCollectionView(contacts);
            //Contacts.CurrentChanged += contactSelectedItemChanged;
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
            Contact currentContact = Contacts.CurrentItem as Contact;
            Debug.WriteLine("current select contact:");
            Debug.WriteLine(currentContact.EMail);*/
        }
    }
}
