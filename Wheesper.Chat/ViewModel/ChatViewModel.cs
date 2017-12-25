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
        SystemState currentSystemState = SystemState.JustLogin;
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
        public string ChatMessageTextBox
        {
            get { return chatMessageTextBox; }
            set
            {
                chatMessageTextBox = value;
                Debug.WriteLine(chatMessageTextBox);
                RaisePropertyChanged("ChatMessageTextBox");
                SendChatMessageCommond.RaiseCanExecuteChanged();
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
            eventAggregator.GetEvent<ShowChangeContactInfoViewEvent>().Publish(CurrentContact);
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
                            Data_time = DateTime.Now.ToString(),
                            SenderNickname=model.CurrentUser.Nickname 
                        };
                        contact.ChatMessageList.Add(m);
                        //CurrentContact.ChatMessageList.Add(m);
                        break;
                    }
                }
            }
            ChatMessageTextBox = null;
            eventAggregator.GetEvent<MouseKeyDownAContactEvent>().Publish(CurrentContact.EMail);
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
            eventAggregator.GetEvent<MsgContactReplyResponseEvent>().Subscribe(contactReplyResponseEventHandler, ThreadOption.UIThread, true);
            eventAggregator.GetEvent<MsgContactApplyingInfoPushMessageEvent>().Subscribe(contactApplyingInfoPushMessageEventHandler, ThreadOption.UIThread, true);
            eventAggregator.GetEvent<MsgContactReplyingInfoPushMessageEvent>().Subscribe(contactReplyingInfoPushMessageEventHandler, ThreadOption.UIThread, true);
            eventAggregator.GetEvent<MsgContactRemarkModifyResponseEvent>().Subscribe(contactRemarkModifyResponseEventHandler, ThreadOption.UIThread, true);
            // MsgUserInfoModifyResponseEvent
            // 
            // 
            // 

            // Chat Module
            eventAggregator.GetEvent<MsgChatPrivateMessageResponseEvent>().Subscribe(chatPrivateMessageResponseEventHandler, ThreadOption.UIThread, true);
            eventAggregator.GetEvent<MsgChatPrivateMessagePushMessageEvent>().Subscribe(chatPrivateMessagePushMessageEventHandler, ThreadOption.UIThread, true);

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

            foreach (ContactList group in Root)
            {
                foreach (Contact contact in group.Contacts)
                {
                    if (contactEMail == contact.EMail)
                    {
                        CurrentContact.Age = contact.Age;
                        CurrentContact.EMail = contact.EMail;
                        CurrentContact.Group = contact.Group;
                        CurrentContact.Nickname = contact.Nickname;
                        CurrentContact.Remarks = contact.Remarks;
                        CurrentContact.Sex = contact.Sex;

                        CurrentContact.ChatMessageList.Clear();
                        foreach(Message m in contact.ChatMessageList)
                        {
                            Message m_new = new Message()
                            {
                                Content = m.Content,
                                Data_time = m.Data_time,
                                RecevieEMail = m.RecevieEMail,
                                RecevieNickname = m.RecevieNickname,
                                SenderEMail = m.SenderEMail,
                                SenderNickname = m.SenderNickname
                            };
                            CurrentContact.ChatMessageList.Add(m_new);
                        }
                        break;
                    }
                }
            }
            eventAggregator.GetEvent<LoadChatViewEvent>().Publish(0);

            currentSystemState = SystemState.Chatting;
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
                    //Debug.WriteLine(((ProtoMessage)systemMessages[id].OriginMessage).ContactApplyingInfoPushMessage.ApplyerMailAddress);
                    break;
                default:
                    break;
            }
        }

        private void loginEventHandler(string email)
        {
            Debug.Write("From loginEventHandler in ChatViewModel: ", email);
            model.CurrentUser.EMail = email;
            model.sendUserInfoQueryRequest(email);
            model.sendContactListRequest(email);
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

            root.Clear();
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
                    if (root[j].Groupname == c.Group)
                    {
                        Debug.Write("add into group:");
                        Debug.WriteLine(c.Group);
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
            eventAggregator.GetEvent<LoadContactViewEvent>().Publish(0);
        }

        private void contactMailCheckResponseEventHandler(ProtoMessage message)
        {
            Debug.WriteLine("ContactMailCheckResponseEvent handler");
            bool state = message.ContactMailCheckResponse.Status;
            string email = message.ContactMailCheckResponse.MailAddress;
            if (state)
            {
                eventAggregator.GetEvent<ShowUserExistViewEvent>().Publish(email);
            }
            else
            {
                eventAggregator.GetEvent<ShowUserNotExistViewEvent>().Publish(email);
            }
        }

        private void contactApplyResponseEventHandler(ProtoMessage message)
        {
            Debug.WriteLine("ContactApplyResponseEvent handler");
            SystemMessage m = new SystemMessage() { Message = "你的好友申请已发送!", type = SystemMessageType.ContactApplySended, IsRead = false };
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

        private void contactReplyResponseEventHandler(ProtoMessage message)
        {
            model.sendContactListRequest(model.CurrentUser.EMail);
        }

        private void contactRemarkModifyResponseEventHandler(ProtoMessage message)
        {
            Debug.WriteLine("ContactRemarkModifyResponseEvent handler");
            Debug.WriteLine("Refresh contact list (resend ContactListRequest)");
            model.sendContactListRequest(model.CurrentUser.EMail);
        }

        private void contactReplyingInfoPushMessageEventHandler(ProtoMessage message)
        {
            Debug.WriteLine("ContactReplyingInfoPushMessageEvent handler");
            Debug.WriteLine("Refresh contact list (resend ContactListRequest)");
            model.sendContactListRequest(model.CurrentUser.EMail);
        }

        private void contactApplyingInfoPushMessageEventHandler(ProtoMessage message)
        {
            Debug.WriteLine("ContactApplyingInfoPushMessageEvent handler");
            SystemMessage m = new SystemMessage() { Message = "你有新的好友申请请求!", type = SystemMessageType.ContactApplyRequest, IsRead = false, OriginMessage = message };
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
            Debug.Write("- ");
            Debug.Write("Applier: ");
            Debug.WriteLine(((ProtoMessage)m.OriginMessage).ContactApplyingInfoPushMessage.ApplyerMailAddress);
        }

        private void chatPrivateMessageResponseEventHandler(ProtoMessage message)
        {
            Debug.WriteLine("ChatPrivateMessageResponseEvent Handler");
        }

        private void chatPrivateMessagePushMessageEventHandler(ProtoMessage message)
        {
            Debug.WriteLine("ChatPrivateMessagePushMessageEvent handler");
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
                            Data_time=chatmess.DateTime,
                            SenderNickname=contact.Nickname
                        };
                        contact.ChatMessageList.Add(m);
                        break;
                    }
                }
            }
            if (currentSystemState == SystemState.Chatting)
            {
                eventAggregator.GetEvent<MouseKeyDownAContactEvent>().Publish(CurrentContact.EMail);
            }
        }
        #endregion event handler
    }
}
