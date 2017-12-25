using System.Text.RegularExpressions;
using Microsoft.Practices.Unity;
using Wheesper.Infrastructure;
using Google.Protobuf;

using Wheesper.Infrastructure.services;
using ProtocolBuffer;
using System.Diagnostics;
using System;

namespace Wheesper.Chat.Model
{
    public class WheesperModel
    {
        #region private member
        private IUnityContainer container = null;
        private IMessagingService messagingService = null;
        private Regex emailRgx = null;
        private string emailPattern = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$";
        private UserInfo currentUser = new UserInfo();
        #endregion private member

        public UserInfo CurrentUser
        {
            get
            {
                return currentUser;
            }
            private set { }
        }

        #region constructor
        public WheesperModel(IUnityContainer container)
        {
            Debug.WriteLine("WheesperModel constructor");
            this.container = container;
            messagingService = this.container.Resolve<IMessagingService>();

            emailRgx = new Regex(emailPattern, RegexOptions.IgnoreCase);
        }
        #endregion constructor


        #region Utility Function
        public bool isEmailAddress(string address)
        {
            return emailRgx.IsMatch(address);
        }
        #endregion Utility FUnction
        #region Contact Function Request
        public void sendUserInfoQueryRequest(string email)
        {
            ProtoMessage message = new ProtoMessage();
            message.UserInfoQueryRequest = new UserInfoQueryRequest();
            message.UserInfoQueryRequest.MailAddress = email;
            messagingService.SendMessage(message);
        }

        public void sendUserInfoModifyRequest(string email, string nickname, string sex, int age, string country, string province, string city)
        {
            ProtoMessage message = new ProtoMessage();
            message.UserInfoModifyRequest = new UserInfoModifyRequest();
            message.UserInfoModifyRequest.MailAddress = email;
            message.UserInfoModifyRequest.Nickname = nickname;
            message.UserInfoModifyRequest.Sex = sex;
            message.UserInfoModifyRequest.Age = age;
            message.UserInfoModifyRequest.Country = country;
            message.UserInfoModifyRequest.Province = province;
            message.UserInfoModifyRequest.City = city;
            messagingService.SendMessage(message);
        }

        public void sendContactListRequest(string email)
        {
            ProtoMessage message = new ProtoMessage();
            message.ContactListRequest = new ContactListRequest();
            message.ContactListRequest.MailAddress = email;
            messagingService.SendMessage(message);
        }
        
        public void sendContactMailCheckRequest(string email)
        {
            ProtoMessage message = new ProtoMessage();
            message.ContactMailCheckRequest = new ContactMailCheckRequest();
            message.ContactMailCheckRequest.MailAddress = email;
            messagingService.SendMessage(message);
        }

        public void sendContactApplyRequest(string applierEMail, string targetEMail, string discription)
        {
            ProtoMessage message = new ProtoMessage();
            message.ContactApplyRequest = new ContactApplyRequest();
            message.ContactApplyRequest.ApplyerMailAddress = applierEMail;
            message.ContactApplyRequest.TargetMailAddress = targetEMail;
            message.ContactApplyRequest.AdditionalMsg = discription;
            messagingService.SendMessage(message);
        }

        public void sendContactReplyRequest(string applierEMail, string targetEMail, bool isAccept, string discription)
        {
            ProtoMessage message = new ProtoMessage();
            message.ContactReplyRequest = new ContactReplyRequest();
            message.ContactReplyRequest.ApplyerMailAddress = applierEMail;
            message.ContactReplyRequest.TargetMailAddress = targetEMail;
            message.ContactReplyRequest.IsAccepted = isAccept;
            message.ContactReplyRequest.AdditionalMsg = discription;
            messagingService.SendMessage(message);
        }

        public void sendContactRemarkModifyRequest(string contactEMail, string contactRemark, string contactGroup)
        {
            ProtoMessage message = new ProtoMessage();
            message.ContactRemarkModifyRequest = new ContactRemarkModifyRequest();
            message.ContactRemarkModifyRequest.ContactEmail = contactEMail;
            message.ContactRemarkModifyRequest.ContactRemark = contactRemark;
            message.ContactRemarkModifyRequest.ContactGroup = contactGroup;
            messagingService.SendMessage(message);
        }

        public void sendPrivateMessageRequest(string recevierEMail, string contents)
        {
            ProtoMessage message = new ProtoMessage();
            var chatMessage = new ChatMessage()
            {
                SenderEmail = CurrentUser.EMail,
                DateTime = DateTime.Now.ToString(),
                MsgContents=contents 
            };
            message.ChatPrivateMessageRequest = new ChatPrivateMessageRequest()
            {
                Message = chatMessage,
                ReciiverEmail = recevierEMail
            };
            messagingService.SendMessage(message);
        }

        public void sendGroupMessageRequest(int groupID, string contents)
        {
            ProtoMessage message = new ProtoMessage();
            var chatMessage = new ChatMessage()
            {
                SenderEmail = CurrentUser.EMail,
                DateTime = DateTime.Now.ToString(),
                MsgContents = contents
            };
            message.ChatGroupMessageRequest = new ChatGroupMessageRequest()
            {
                Message = chatMessage,
                GroupID=groupID
            };
            messagingService.SendMessage(message);
        }
        #endregion Contact Function Request
    }

    public enum SystemMessageType
    {
        ContactApplySended,
        ContactApplyRequest
    }

    public enum SystemState
    {
        JustLogin,
        Chatting
    }
}
