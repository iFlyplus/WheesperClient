using Microsoft.Practices.Unity;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.UnityExtensions;
using System.Windows;
using System;


using Wheesper.Infrastructure.services;
using Wheesper.Messaging;
using ProtocolBuffer;
using System.Diagnostics;
using Google.Protobuf;
using Wheesper.Messaging.events;

namespace Wheesper.Test
{
    public class MessagingTest :IModule
    {
        private IUnityContainer container = null;
        private EventAggregator eventAggregator = null;
        private IMessagingService messagingService = null;

        public MessagingTest(IUnityContainer container)
        {
            this.container = container;
            eventAggregator = this.container.Resolve<EventAggregator>();
            messagingService = this.container.Resolve<IMessagingService>();
        }

        private void starttest()
        {
            string mail = "iFLy_2015@outlook.com";
            string password = "4587623145";
            Debug.WriteLine("sign up test");
            Debug.WriteLine("mail: {0}", mail);
            // Debug.WriteLine("password: {0}", password);
            ProtoMessage message = new ProtoMessage();
            SignupMailRequest request = new SignupMailRequest() { MailAddress = mail };
            
            Debug.WriteLine(message.MsgCase);
            message.SignupMailRequest = request;
            Debug.WriteLine(message.MsgCase);

            byte[] bufferForMessage = message.ToByteArray();
            int messageLength = bufferForMessage.Length;
            byte[] bufferForLength = BitConverter.GetBytes(messageLength);
            byte[] package = new byte[bufferForLength.Length + bufferForMessage.Length];
            System.Buffer.BlockCopy(bufferForLength, 0, package, 0, bufferForLength.Length);
            System.Buffer.BlockCopy(bufferForMessage, 0, package, bufferForLength.Length, bufferForMessage.Length);
            messagingService.SendMessage(message);
        }


        private void handle_signupMail_resp(ProtoMessage message)
        {
            bool state = message.SignupMailResponse.Status;
            if(state)
            {
                Debug.WriteLine("the mail is avaliable");
            }
        }

        #region Module Method
        public void Initialize()
        {
            #region event subscribe

            eventAggregator.GetEvent<MsgSignupMailResponseEvent>().Subscribe(handle_signupMail_resp, ThreadOption.BackgroundThread);

            #endregion event subscribe

            starttest();
        }
        #endregion Module Methon
    }
}
