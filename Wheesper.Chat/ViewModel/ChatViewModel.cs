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
        public Contact CurrentContact
        {
            get { return currentContact; }
            private set
            {
                currentContact = value;
            }
        }
        public ListCollectionView  Root { get; private set; }
        public ListCollectionView SystemMessages { get; private set; }
        public string SearchBox_UserEMail
        {
            get { return searchBox_UserEMail; }
            set
            {
                searchBox_UserEMail = value;
                RaisePropertyChanged("SearchBox_UserEMail");
                Debug.WriteLine(searchBox_UserEMail);
                SearchUserCommond.RaiseCanExecuteChanged();
                addContactRequestCommand.RaiseCanExecuteChanged();
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
        public string ChatMessageTextBox
        {
            get { return chatMessageTextBox; }
            set
            {
                chatMessageTextBox = value;
                Debug.WriteLine(chatMessageTextBox);
                RaisePropertyChanged("ChatMessageTextBox");
            }
        }

        private UserInfo currentUser = new UserInfo();
        private Contact currentContact = new Contact();
        public ObservableCollection<ContactList> root = new ObservableCollection<ContactList>();
        private ObservableCollection<SystemMessage> systemMessages = new ObservableCollection<SystemMessage>();
        private string searchBox_UserEMail = null;
        private string addContactDiscription = "Hello";
        private string chatMessageTextBox = null;
        #endregion properties


        #region Commond
        public DelegateCommand SearchUserCommond
        {
            get
            {
                if (searchUserCommand == null)
                    searchUserCommand = new DelegateCommand(searchUser, canSearchUser);
                return searchUserCommand;
            }
            private set { }
        }
        public DelegateCommand AddContactRequest
        {
            get
            {
                if (addContactRequestCommand == null)
                    addContactRequestCommand = new DelegateCommand(addContactRequest, canAddContactRequest);
                return addContactRequestCommand;
            }
            private set { }
        }
        public DelegateCommand ModifyContactInfoCommond
        {
            get
            {
                if (modifyContactInfoCommand == null)
                    modifyContactInfoCommand = new DelegateCommand(modifyContactInfo, canModifyContactInfo);
                return modifyContactInfoCommand;
            }
            private set { }
        }
        public DelegateCommand ShowSystemMessageCommond
        {
            get
            {
                if (showSystemMessageCommand == null)
                    showSystemMessageCommand = new DelegateCommand(showSystemMessage, canShowsystemMessage);
                return showSystemMessageCommand;
            }
            private set { }
        }
        public DelegateCommand CloseSystemMessageCommond
        {
            get
            {
                if (closeSystemMessageCommand == null)
                    closeSystemMessageCommand = new DelegateCommand(closeSystemMessage, canCloseSystemMessage);
                return closeSystemMessageCommand;
            }
            private set { }
        }
        public DelegateCommand SendChatMessageCommond
        {
            get
            {
                if (sendChatMessageCommand == null)
                    sendChatMessageCommand = new DelegateCommand(sendChatMessage, canSendChatMessage);
                return sendChatMessageCommand;
            }
            private set { }
        }

        //private DelegateCommand 
        private DelegateCommand searchUserCommand = null;
        private DelegateCommand addContactRequestCommand = null;
        private DelegateCommand modifyContactInfoCommand = null;
        private DelegateCommand showSystemMessageCommand = null;
        private DelegateCommand closeSystemMessageCommand = null;
        private DelegateCommand sendChatMessageCommand = null;
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
            eventAggregator.GetEvent<LoadContactViewEvent>().Publish(0);
        }
        private bool canCloseSystemMessage()
        {
            return true;
        }

        private void sendChatMessage()
        {
            model.sendPrivateMessageRequest(CurrentContact.EMail, ChatMessageTextBox);
            string receiverEMail = CurrentContact.EMail;
            foreach (ContactList group in Root)
            {
                foreach (Contact contact in group.Contacts)
                {
                    if (receiverEMail == contact.EMail)
                    {
                        //Debug.Write()
                        Message m = new Message()
                        {
                            SenderEMail = model.CurrentUser.EMail,
                            RecevieEMail = receiverEMail,
                            Content = ChatMessageTextBox,
                            Data_time = DateTime.Now.ToString()
                        };
                        contact.ChatMessageList.Add(m);
                        break;
                    }
                }
            }
            ChatMessageTextBox = null;
        }
        private bool canSendChatMessage()
        {
            return !string.IsNullOrWhiteSpace(ChatMessageTextBox);
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
            //SystemMessages.CurrentChanged += systemMessageSelectedItemChaged;
            //ContactsList.CurrentChanged += contactSelectedItemChaged;
            eventAggregator.GetEvent<LoginEvent>().Subscribe(loginEventHandler, true);
            eventAggregator.GetEvent<MsgUserInfoQueryResponseEvent>().Subscribe(userInfoQueryResponseEventHandler, true);
            eventAggregator.GetEvent<MsgContactListResponseEvent>().Subscribe(contactListResponseEventHandler, ThreadOption.UIThread, true);
            eventAggregator.GetEvent<MsgContactMailCheckResponseEvent>().Subscribe(contactMailCheckResponseEventHandler, true);
            eventAggregator.GetEvent<MsgContactApplyResponseEvent>().Subscribe(contactApplyResponseEventHandler, ThreadOption.UIThread, true);
            eventAggregator.GetEvent<MsgContactApplyingInfoPushMessageEvent>().Subscribe(contactApplyingInfoPushMessageEventHandler, ThreadOption.UIThread, true);
            // MsgUserInfoModifyResponseEvent
            // MsgContactReplyResponseEvent
            // MsgContactReplyingInfoPushMessageEvent
            // MsgContactRemarkModifyResponseEvent

            // Chat Module
            // MsgChatPrivateMessageResponseEvent
            // MsgChatPrivateMessagePushMessageEvent
            

            // Event from view: 
            eventAggregator.GetEvent<MouseKeyDownAContactEvent>().Subscribe(mouseKeyDownAContactEventHandler, ThreadOption.UIThread, true);
            eventAggregator.GetEvent<MouseKeyDownASystemMessageEvent>().Subscribe(mouseKeyDownASystemMessageEventHandler, ThreadOption.UIThread, true);
        }
        private void init()
        {
            //ContactsList = new ListCollectionView(contactsList);
            SystemMessages = new ListCollectionView(systemMessages);
            Root = new ListCollectionView(root);
        }
        #endregion helper function

        #region event handler

        private void mouseKeyDownAContactEventHandler(string contactEMail)
        {
            Debug.Write("Current Selected Contact: ");
            Debug.WriteLine(contactEMail);
        }

        private void mouseKeyDownASystemMessageEventHandler(int id)
        {
            Debug.Write("Message ID: ");
            Debug.WriteLine(id);
            Debug.Write("- ");
            Debug.WriteLine(systemMessages[id].ID);
            SystemMessageType type = systemMessages[id].type;
            switch (type)
            {
                case SystemMessageType.ContactApplySended:
                    break;
                case SystemMessageType.ContactApplyRequest:
                    eventAggregator.GetEvent<ShowSolveContactApplyViewEvent>().Publish(systemMessages[id].OriginMessage);
                    break;
                default:
                    break;
            }
        }

        private void loginEventHandler(string email)
        {
            Debug.Write("From loginEventHandler in ChatViewModel: ", email);
            model.sendUserInfoQueryRequest(email);
            //model.sendContactReplyRequest("439@qq.com","441@qq.com", true, "Hi");
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
            model.CurrentUser.EMail = _user.MailAddress;
            model.CurrentUser.Nickname = _user.Nickname;
            model.CurrentUser.Sex = _user.Sex;
            model.CurrentUser.Age = _user.Age;
            model.CurrentUser.Country = _user.Country;
            model.CurrentUser.Province = _user.Province;
            model.CurrentUser.City = _user.City;
            model.CurrentUser.CreateDate = _user.CreateDate;
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

                for(int j = 0; j < root.Count; j++)
                {
                    Debug.Write("add into group:");
                    Debug.WriteLine(c.Group);
                    if (root[j].Groupname == c.Group)
                    {
                        root[j].Add(c);
                        isGroupExist = true;
                    }
                }
                if (isGroupExist == false)
                {
                    Debug.Write("create group:");
                    Debug.WriteLine(c.Group);
                    root.Add(new ContactList(c.Group));
                    for (int j = 0; j < root.Count; j++)
                    {
                        if (root[j].Groupname == c.Group)
                        {
                            root[j].Add(c);
                            isGroupExist = true;
                        }
                    }
                }
            }
            Debug.WriteLine("all in root:");
            for(int i = 0; i < root.Count; i++)
            {
                Debug.Write("- ");
                Debug.WriteLine(root[i].Groupname);
                Debug.Write("--- ");
                Debug.Write("count: ");
                Debug.WriteLine(root[i].Contacts.Count);
                Debug.Write("--- ");
                Debug.WriteLine(root[i].Contacts.ToString());

            }
            //model.sendContactApplyRequest(currentUser.EMail, SearchBox_UserEMail, "hello");
        }

        private void contactMailCheckResponseEventHandler(ProtoMessage message)
        {
            Debug.WriteLine("ContactMailCheckResponseEvent handler");
            
        }

        private void contactApplyResponseEventHandler(ProtoMessage message)
        {
            Debug.WriteLine("ContactApplyResponseEvent handler");
            SystemMessage m = new SystemMessage() { Message = "Contact Apply Request has been send!", type = SystemMessageType.ContactApplySended, IsRead = false };
            systemMessages.Add(m);

            Debug.WriteLine("create a systemMessages:");
            Debug.Write("- ");
            Debug.Write("ID: ");
            Debug.WriteLine(m.ID);
            Debug.Write("- ");
            Debug.Write("IsRead: ");
            Debug.WriteLine(m.IsRead);
            Debug.Write("- ");
            Debug.Write("Message: ");
            Debug.WriteLine(m.Message);
        }

        private void contactApplyingInfoPushMessageEventHandler(ProtoMessage message)
        {
            Debug.WriteLine("ContactApplyingInfoPushMessageEvent handler");
            SystemMessage m = new SystemMessage() { Message = "A new Contact Apply Request!", type = SystemMessageType.ContactApplyRequest, IsRead = false, OriginMessage = message };
            systemMessages.Add(m);

            Debug.WriteLine("create a systemMessages:");
            Debug.Write("- ");
            Debug.Write("ID: ");
            Debug.WriteLine(m.ID);
            Debug.Write("- ");
            Debug.Write("IsRead: ");
            Debug.WriteLine(m.IsRead);
            Debug.Write("- ");
            Debug.Write("Message: ");
            Debug.WriteLine(m.Message);
        }

        private void chatPrivateMessageResponseEventHandler(ProtoMessage message)
        {
        }

        private void chatPrivateMessagePushMessageEvent(ProtoMessage message)
        {
            Debug.WriteLine("CchatPrivateMessagePushMessageEvent handler");
            ChatPrivateMessagePushMessage incomeMessage = message.ChatPrivateMessagePushMessage;
            string senderEMail = incomeMessage.SenderEmail;
            ChatMessage chatmess = incomeMessage.Message;
            foreach(ContactList group in Root)
            {
                foreach(Contact contact in group.Contacts)
                {
                    if (senderEMail == contact.EMail)
                    {
                        //Debug.Write()
                        Message m = new Message()
                        {
                            SenderEMail = senderEMail,
                            RecevieEMail = model.CurrentUser.EMail,
                            Content=chatmess.MsgContents,
                            Data_time=chatmess.DateTime 
                        };
                        contact.ChatMessageList.Add(m);
                        break;
                    }
                }
            }
        }
        #endregion event handler
    }
}
