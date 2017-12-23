﻿using Microsoft.Practices.Prism.Commands;
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
using Wheesper.Login.events;

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
        //public ListCollectionView ContactsList { get; private set; }
        public ObservableCollection<ContactList> Root = new ObservableCollection<ContactList>();
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
        public ListCollectionView SystemMessages { get; private set; }

        private UserInfo currentUser = new UserInfo();
        //private ObservableCollection<Contact> contactsList = new ObservableCollection<Contact>();
        private string searchBox_UserEMail = "441@qq.com";
        private string addContactDiscription = "Hello";
        private ObservableCollection<SystemMessage> systemMessages = new ObservableCollection<SystemMessage>();
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
        public DelegateCommand ShowSystemMessageCommond
        {
            get
            {
                if (showSystemMessageCommond == null)
                    showSystemMessageCommond = new DelegateCommand(showSystemMessage, canShowsystemMessage);
                return showSystemMessageCommond;
            }
            private set { }
        }
        public DelegateCommand CloseSystemMessageCommond
        {
            get
            {
                if (closeSystemMessageCommond == null)
                    closeSystemMessageCommond = new DelegateCommand(closeSystemMessage, canCloseSystemMessage);
                return closeSystemMessageCommond;
            }
            private set { }
        }

        //private DelegateCommand 
        private DelegateCommand searchUserCommond = null;
        private DelegateCommand addContactRequestCommond = null;
        private DelegateCommand modifyContactInfoCommond = null;
        private DelegateCommand showSystemMessageCommond = null;
        private DelegateCommand closeSystemMessageCommond = null;

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

        private void showSystemMessage()
        {
            eventAggregator.GetEvent<ShowSystemMessageViewEvent>().Publish(0);
        }
        private bool canShowsystemMessage()
        {
            return true;
        }

        private void closeSystemMessage()
        {
            eventAggregator.GetEvent<CloseSystemMessageViewEvent>().Publish(0);
        }
        private bool canCloseSystemMessage()
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

            init();
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
            SystemMessages.CurrentChanged += systemMessageSelectedItemChaged;
            eventAggregator.GetEvent<LoginEvent>().Subscribe(loginEventHandler, true);
            eventAggregator.GetEvent<MsgUserInfoQueryResponseEvent>().Subscribe(userInfoQueryResponseEventHandler, true);
            eventAggregator.GetEvent<MsgContactListResponseEvent>().Subscribe(contactListResponseEventHandler, true);
            eventAggregator.GetEvent<MsgContactMailCheckResponseEvent>().Subscribe(contactMailCheckResponseEventHandler, true);
            eventAggregator.GetEvent<MsgContactApplyResponseEvent>().Subscribe(contactApplyResponseEventHandler, true);
            eventAggregator.GetEvent<MsgContactApplyingInfoPushMessageEvent>().Subscribe(contactApplyingInfoPushMessageEventHandler, true);
            // MsgUserInfoModifyResponseEvent
            // MsgContactReplyResponseEvent
            // MsgContactReplyingInfoPushMessageEvent
            // MsgContactRemarkModifyResponseEvent


        }
        private void init()
        {
            //ContactsList = new ListCollectionView(contactsList);
            SystemMessages = new ListCollectionView(systemMessages);
        }
        #endregion helper function

        #region event handler
        private void systemMessageSelectedItemChaged(object sender, EventArgs e)
        {
            // Customer current = Customers.CurrentItem as Customer;
            SystemMessage systemMessage = SystemMessages.CurrentItem as SystemMessage;

            systemMessages[systemMessages.IndexOf(systemMessage)].IsRead = true;
                //ShowSolveContactApplyViewEvent
            if(systemMessage.type==SystemMessageType.ContactApplyRequest)
            {
                Debug.WriteLine("show ");
                eventAggregator.GetEvent<ShowSolveContactApplyViewEvent>().Publish(systemMessage.OriginMessage);
            }
        }


        private void loginEventHandler(string email)
        {
            Debug.Write("From loginEventHandler in ChatViewModel: ", email);
            model.sendUserInfoQueryRequest(email);
            model.sendContactListRequest(email);
            //model.sendContactMailCheckRequest(SearchBox_UserEMail);
        }
        private void userInfoQueryResponseEventHandler(ProtoMessage message)
        {
            Debug.WriteLine("UserInfoQueryResponseEvent handler");
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
            //model.sendContactApplyRequest(currentUser.EMail, SearchBox_UserEMail, AddContactDiscription);
        }
        private void contactListResponseEventHandler(ProtoMessage message)
        {
            Debug.WriteLine("ContactListResponseEvent handler");
            Google.Protobuf.Collections.RepeatedField<ContactListResponse.Types.Contact> tempList 
                         = message.ContactListResponse.Contacts;
            
            for (int i = 0; i < tempList.Count; i++)
            {
                Contact c = new Contact()
                {
                    EMail = tempList[i].MailAddress,
                    Nickname = tempList[i].Nickname,
                    Group = tempList[i].Group,
                    Remarks = tempList[i].Remarks
                };
                Debug.Write("Contact[");
                Debug.Write(i);
                Debug.Write("]: ");
                Debug.Write("email: ");
                Debug.Write(c.EMail);
                Debug.Write("group: ");
                Debug.WriteLine(c.Group);
                bool isGroupExist = false;
                for(int j = 0; j < Root.Count; j++)
                {
                    Debug.Write("add into group:");
                    Debug.WriteLine(c.Group);
                    if (Root[j].Groupname == c.Group)
                    {
                        Root[j].Add(c);
                        isGroupExist = true;
                    }
                }
                if (isGroupExist == false)
                {
                    Debug.Write("create group:");
                    Debug.WriteLine(c.Group);
                    Root.Add(new ContactList(c.Group));
                    for (int j = 0; j < Root.Count; j++)
                    {
                        if (Root[j].Groupname == c.Group)
                        {
                            Root[j].Add(c);
                            isGroupExist = true;
                        }
                    }
                }

            }

            Debug.WriteLine("all in Root:");
            for(int i = 0; i < Root.Count; i++)
            {
                Debug.Write("- ");
                Debug.WriteLine(Root[i].Groupname);
                Debug.Write("--- ");
                Debug.Write("count: ");
                Debug.WriteLine(Root[i].Contacts.Count);
                Debug.Write("--- ");
                Debug.WriteLine(Root[i].Contacts.ToString());
                for (int j = 0; j < Root[i].Contacts.Count; j++)
                {

                }
            }
        }
        private void contactMailCheckResponseEventHandler(ProtoMessage message)
        {
            Debug.WriteLine("ContactMailCheckResponseEvent handler");
            
        }
        private void contactApplyResponseEventHandler(ProtoMessage message)
        {
            Debug.WriteLine("ContactApplyResponseEvent handler");
            systemMessages.Add(new SystemMessage() { Message = "Contact Apply Request has been send!", type = SystemMessageType.ContactApplySended, IsRead = false });
        }
        private void contactApplyingInfoPushMessageEventHandler(ProtoMessage message)
        {
            Debug.WriteLine("ContactApplyingInfoPushMessageEvent handler");
            systemMessages.Add(new SystemMessage() { Message = "A new Contact Apply Request!", type = SystemMessageType.ContactApplyRequest, IsRead = false, OriginMessage=message });

            Debug.WriteLine("Accept Contact Application");
            ContactApplyingInfoPushMessage m = message.ContactApplyingInfoPushMessage;
            model.sendContactReplyRequest(m.ApplyerMailAddress, m.TargetMailAddress, true, "Hi");
        }
        #endregion event handler
    }
}
