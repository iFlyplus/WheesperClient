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

using Wheesper.Chat.Model;
using Wheesper.Infrastructure.events;
using Wheesper.Messaging.events;
using ProtocolBuffer;

namespace Wheesper.Chat.ViewModel
{
    public class ChatViewModel : BindableBase
    {
        #region private menber
        private IUnityContainer container = null;
        private IEventAggregator eventAggregator = null;
        private WheesperModel model = null;
        #endregion private menber

        #region private method
        
        #endregion private method

        #region properties
        public UserInfo CurrentUser
        {
            get
            {
                if (currentUser == null)
                    currentUser = new UserInfo();
                return currentUser;
            }
            private set
            {
                currentUser = value;
            }
        }
        public ListCollectionView Contacts { get; private set; }
        public string SearchBox_UserEMail
        {
            get { return searchBox_UserEMail; }
            set
            {
                searchBox_UserEMail = value;
                RaisePropertyChanged("SearchBox_UserEMail");
                SearchUserCommond.RaiseCanExecuteChanged();
            }
        }
        public string AddContactDiscription
        {
            get { return addContactDiscription; }
            set
            {
                addContactDiscription = value;
                RaisePropertyChanged("AddContactDiscription");
            }
        }

        private UserInfo currentUser = null;
        private string searchBox_UserEMail = "440@qq.com";
        private string addContactDiscription = "Hello";
        #endregion properties

        #region Commond
        public DelegateCommand SearchUserCommond
        {
            get
            {
                if (searchUserCommond == null)
                    searchUserCommond = new DelegateCommand(searchUser, canSearchUser);
                return searchUserCommond;
            }
            private set { }
        }
        public DelegateCommand AddContactRequest
        {
            get
            {
                if (addContactRequestCommond == null)
                    addContactRequestCommond = new DelegateCommand(addContactRequest, canAddContactRequest);
                return addContactRequestCommond;
            }
            private set { }
        }
        public DelegateCommand ModifyContactInfoCommond
        {
            get
            {
                if (modifyContactInfoCommond == null)
                    modifyContactInfoCommond = new DelegateCommand(modifyContactInfo, canModifyContactInfo);
                return modifyContactInfoCommond;
            }
            private set { }
        }
        //private DelegateCommand 
        private DelegateCommand searchUserCommond = null;
        private DelegateCommand addContactRequestCommond = null;
        private DelegateCommand modifyContactInfoCommond = null;
        #endregion Commond

        #region Command Delegate Method
        private void searchUser()
        {
            if (model.isEmailAddress(SearchBox_UserEMail))
            {
                Debug.WriteLine("mail valid");
                model.sendContactMailCheckRequest(SearchBox_UserEMail);
            }
            else
            {
                Debug.WriteLine("mail invalid");
                SearchBox_UserEMail = null;
            }
        }
        private bool canSearchUser()
        {
            return !string.IsNullOrWhiteSpace(SearchBox_UserEMail);
        }

        private void addContactRequest()
        {
            if (model.isEmailAddress(SearchBox_UserEMail))
            {
                Debug.WriteLine("mail valid");
                model.sendContactApplyRequest(currentUser.EMail, SearchBox_UserEMail, AddContactDiscription);
            }
            else
            {
                Debug.WriteLine("mail invalid");
                SearchBox_UserEMail = null;
            }
        }
        private bool canAddContactRequest()
        {
            return !string.IsNullOrWhiteSpace(SearchBox_UserEMail);
        }

        private void modifyContactInfo()
        {
        }
        private bool canModifyContactInfo()
        {
            return true;
        }
        #endregion Command Delegate Method

        #region Constructor & deconstructor
        public ChatViewModel(IUnityContainer container)
        {
            Debug.WriteLine("ChatViewModel constructor");
            this.container = container;
            eventAggregator = this.container.Resolve<IEventAggregator>();
            model = this.container.Resolve<WheesperModel>();

            subevent();
        }

        ~ChatViewModel()
        {
            Debug.WriteLine("ChatViewModel Deconstrutor");
        }
        #endregion Constructor & deconstructor

        #region helper functoin
        private void subevent()
        {
            Debug.WriteLine("ChatViewModel subscribe event");
            // Contacts.CurrentChanged += contactSelectedItemChanged;
            eventAggregator.GetEvent<LoginEvent>().Subscribe(loginEventHandler, true);
            eventAggregator.GetEvent<MsgUserInfoQueryResponseEvent>().Subscribe(userInfoQueryResponseEventHandler, true);
            eventAggregator.GetEvent<MsgContactListResponseEvent>().Subscribe(contactListResponseEventHandler, true);
            eventAggregator.GetEvent<MsgContactMailCheckResponseEvent>().Subscribe(contactMailCheckResponseEventHandler, true);
            eventAggregator.GetEvent<MsgContactApplyResponseEvent>().Subscribe(contactApplyResponseEventHandler, true);
            // MsgUserInfoModifyResponseEvent
            // MsgContactApplyResponseEvent
            // MsgContactReplyResponseEvent
            // MsgContactApplyingInfoPushMessageEvent
            // MsgContactReplyingInfoPushMessageEvent
            // MsgContactRemarkModifyResponseEvent
        }
        #endregion helper function

        #region event handler
        private void contactSelectedItemChanged(object sender, EventArgs e)
        {

        }

        private void loginEventHandler(string email)
        {
            Debug.Write("From loginEventHandler in ChatViewModel: ", email);
            model.sendUserInfoQueryRequest(email);
            model.sendContactListRequest(email);
            model.sendContactMailCheckRequest(searchBox_UserEMail);
        }
        private void userInfoQueryResponseEventHandler(ProtoMessage message)
        {
            Debug.WriteLine("UserInfoQueryResponseEvent handler");
            currentUser = new UserInfo();
            var _user = message.UserInfoQueryResponse;
            CurrentUser.EMail = _user.MailAddress;
            CurrentUser.Nickname = _user.Nickname;
            CurrentUser.Sex = _user.Sex;
            CurrentUser.Age = _user.Age;
            CurrentUser.Country = _user.Country;
            CurrentUser.Province = _user.Province;
            CurrentUser.City = _user.City;
            CurrentUser.CreateDate = _user.CreateDate;
            Debug.WriteLine("CURRENT USERINFO");
            Debug.Write(CurrentUser.Age.ToString());
            Debug.Write(CurrentUser.City);
            Debug.Write(CurrentUser.Country);
            Debug.Write(CurrentUser.CreateDate);
            Debug.Write(CurrentUser.EMail);
            Debug.Write( CurrentUser.Nickname);
            Debug.WriteLine(CurrentUser.Province);

            model.sendContactApplyRequest(currentUser.EMail, SearchBox_UserEMail, AddContactDiscription);
        }
        private void contactListResponseEventHandler(ProtoMessage message)
        {
            Debug.WriteLine("ContactListResponseEvent handler");
            Google.Protobuf.Collections.RepeatedField<ContactListResponse.Types.Contact> contacts 
                         = message.ContactListResponse.Contacts;
            ObservableCollection<Contact> _contacts = new ObservableCollection<Contact>();
            
            for (int i = 0; i < contacts.Count; i++){
                Contact c = new Contact();
                c.EMail = contacts[i].MailAddress;
                c.Nickname = contacts[i].Nickname;
                c.Group = contacts[i].Group;
                c.Remarks = contacts[i].Remarks;
                _contacts.Add(c);
            }
            Contacts = new ListCollectionView(_contacts);
            if (_contacts.Count == 0)
                Debug.WriteLine("no contact exist now");
        }
        private void contactMailCheckResponseEventHandler(ProtoMessage message)
        {
            Debug.WriteLine("ContactMailCheckResponseEvent handler");
            
        }
        private void contactApplyResponseEventHandler(ProtoMessage message)
        {
            Debug.WriteLine("ContactApplyResponseEvent handler");
        }
        #endregion event handler
    }
}
