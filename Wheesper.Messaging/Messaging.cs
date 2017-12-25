using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System;
using System.Net;
using System.Net.Sockets;

using Microsoft.Practices.Prism.UnityExtensions;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Prism.Events;
using Google.Protobuf;

using Wheesper.Infrastructure;
using Wheesper.Infrastructure.events;
using Wheesper.Infrastructure.services;
using Wheesper.Messaging.events;
using ProtocolBuffer;
using System.Diagnostics;

namespace Wheesper.Messaging
{
    public class Messaging : IMessagingService
    {
        private IUnityContainer container = null;
        private IEventAggregator eventAggregator = null;
        private Thread listenThread = null;
        private Socket client = null;
        private const string serverIP = "110.64.89.243";
        private const int serverPort = 12345;
        //private const string serverIP = "127.0.0.1";
        // private const int serverPort = 10103;

        #region Constructor
        public Messaging(IUnityContainer container)
        {
            Debug.WriteLine("messaging constructor");
            this.container = container;
            eventAggregator = this.container.Resolve<IEventAggregator>();

            eventAggregator.GetEvent<MessageIncomeEvent>().Subscribe(dispatchMessage, true);

            bool state = setupClient();
            if(state ==true)
            {
                listenThread = new Thread(startListen);
                Debug.WriteLine("listenThread Info:", listenThread.ManagedThreadId, listenThread.ToString());
                Debug.WriteLine(listenThread.ManagedThreadId);
                Debug.WriteLine(listenThread.GetHashCode());
                listenThread.Start();
            }
        }
        #endregion Constructor

        private bool setupClient()
        {
            bool flag = true;
            IPAddress serverIPAddress = IPAddress.Parse(serverIP);
            IPEndPoint serverEndPoint = new IPEndPoint(serverIPAddress, serverPort);
            client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                client.Connect(serverEndPoint);
                Debug.WriteLine("connect successfully");
            }
            catch (Exception e)
            {
                eventAggregator.GetEvent<ConnectFailEvent>().Publish(e);
                flag = false;
                Debug.WriteLine("connect fail");
            }
            return flag;
        }

        private void startListen()
        {
            int messageLength;
            ProtoMessage message;
            byte[] bufferForLength = new byte[4];
            byte[] bufferForMessage;
            try
            {
                while (true)
                {
                    
                    client.Receive(bufferForLength, 4, SocketFlags.None);
                    byte[] temp = new byte[4];
                    for (int i = 0; i < 4; i++)
                    {
                        temp[3 - i] = bufferForLength[i];
                    }
                    bufferForLength = temp;
                    messageLength = BitConverter.ToInt32(bufferForLength, 0);
                    Debug.WriteLine(messageLength);
                    bufferForMessage = new byte[messageLength];
                    client.Receive(bufferForMessage, messageLength, 0);
                    message = ProtoMessage.Parser.ParseFrom(bufferForMessage);
                    Debug.WriteLine("Message Income.");
                    Debug.WriteLine(message);
                    eventAggregator.GetEvent<MessageIncomeEvent>().Publish(message);
                }
            }
            catch(Exception e)
            {
                Debug.WriteLine(e);
                startListen();
            }
        }

        private void dispatchMessage(ProtoMessage message)
        {
            ProtoMessage.MsgOneofCase messageType = message.MsgCase;
            switch(messageType)
            {
                case ProtoMessage.MsgOneofCase.SigninRequest:
                    break;
                case ProtoMessage.MsgOneofCase.SigninResponse:
                    eventAggregator.GetEvent<MsgSigninResponseEvent>().Publish(message);
                    break;
                case ProtoMessage.MsgOneofCase.SignoutRequest:
                    break;
                case ProtoMessage.MsgOneofCase.SignupMailRequest:
                    break;
                case ProtoMessage.MsgOneofCase.SignupMailResponse:
                    eventAggregator.GetEvent<MsgSignupMailResponseEvent>().Publish(message);
                    break;
                case ProtoMessage.MsgOneofCase.SignupCaptchaRequest:
                    break;
                case ProtoMessage.MsgOneofCase.SignupCaptchaResponse:
                    eventAggregator.GetEvent<MsgSignupCaptchaResponseEvent>().Publish(message);
                    break;
                case ProtoMessage.MsgOneofCase.SignupInfoRequest:
                    break;
                case ProtoMessage.MsgOneofCase.SignupInfoResponse:
                    eventAggregator.GetEvent<MsgSignupInfoResponseEvent>().Publish(message);
                    break;
                case ProtoMessage.MsgOneofCase.PasswordModifyCaptchaRequest:
                    break;
                case ProtoMessage.MsgOneofCase.PasswordModifyCaptchaResponse:
                    eventAggregator.GetEvent<MsgPasswordModifyCaptchaResponseEvent>().Publish(message);
                    break;
                case ProtoMessage.MsgOneofCase.PasswordModifyRequest:
                    break;
                case ProtoMessage.MsgOneofCase.PasswordModifyResponse:
                    eventAggregator.GetEvent<MsgPasswordModifyResponseEvent>().Publish(message);
                    break;
                case ProtoMessage.MsgOneofCase.UserInfoQueryRequest:
                    break;
                case ProtoMessage.MsgOneofCase.UserInfoQueryResponse:
                    eventAggregator.GetEvent<MsgUserInfoQueryResponseEvent>().Publish(message);
                    break;
                case ProtoMessage.MsgOneofCase.UserInfoModifyRequest:
                    break;
                case ProtoMessage.MsgOneofCase.UserInfoModifyResponse:
                    eventAggregator.GetEvent<MsgUserInfoModifyResponseEvent>().Publish(message);
                    break;
                case ProtoMessage.MsgOneofCase.ContactListRequest:
                    break;
                case ProtoMessage.MsgOneofCase.ContactListResponse:
                    eventAggregator.GetEvent<MsgContactListResponseEvent>().Publish(message);
                    break;
                case ProtoMessage.MsgOneofCase.ContactMailCheckRequest:
                    break;
                case ProtoMessage.MsgOneofCase.ContactMailCheckResponse:
                    eventAggregator.GetEvent<MsgContactMailCheckResponseEvent>().Publish(message);
                    break;
                case ProtoMessage.MsgOneofCase.ContactApplyRequest:
                    break;
                case ProtoMessage.MsgOneofCase.ContactApplyResponse:
                    eventAggregator.GetEvent<MsgContactApplyResponseEvent>().Publish(message);
                    break;
                case ProtoMessage.MsgOneofCase.ContactReplyRequest:
                    break;
                case ProtoMessage.MsgOneofCase.ContactReplyResponse:
                    eventAggregator.GetEvent<MsgContactReplyResponseEvent>().Publish(message);
                    break;
                case ProtoMessage.MsgOneofCase.ContactApplyingInfoPushMessage:
                    eventAggregator.GetEvent<MsgContactApplyingInfoPushMessageEvent>().Publish(message);
                    break;
                case ProtoMessage.MsgOneofCase.ContactReplyingInfoPushMessage:
                    eventAggregator.GetEvent<MsgContactReplyingInfoPushMessageEvent>().Publish(message);
                    break;
                case ProtoMessage.MsgOneofCase.ContactRemarkModifyRequest:
                    break;
                case ProtoMessage.MsgOneofCase.ContactRemarkModifyResponse:
                    eventAggregator.GetEvent<MsgContactRemarkModifyResponseEvent>().Publish(message);
                    break;
                case ProtoMessage.MsgOneofCase.ChatPrivateMessageResponse:
                    eventAggregator.GetEvent<MsgChatPrivateMessageResponseEvent>().Publish(message);
                    break;
                case ProtoMessage.MsgOneofCase.ChatPrivateMessagePushMessage:
                    eventAggregator.GetEvent<MsgChatPrivateMessagePushMessageEvent>().Publish(message);
                    break;
                case ProtoMessage.MsgOneofCase.ChatGroupMessageResponse:
                    eventAggregator.GetEvent<MsgChatGroupMessageResponseEvent>().Publish(message);
                    break;
                    break;
                case ProtoMessage.MsgOneofCase.ChatGroupMessagePushMessage:
                    eventAggregator.GetEvent<MsgChatGroupMessagePushMessageEvent>().Publish(message);
                    break;
                    break;
                default:
                    break;
            }
        }
        
        public void SendMessage(ProtoMessage message)
        {
            Debug.WriteLine("request type: {0}", message.MsgCase);
            Debug.WriteLine(message);
            byte[] bufferForMessage = message.ToByteArray();
            int messageLength = bufferForMessage.Length;
            byte[] bufferForLength = BitConverter.GetBytes(messageLength);
            
            byte[] temp = new byte[4];
            for (int i = 0; i<4;i++)
            {
                temp[3 - i] = bufferForLength[i];
            }
            bufferForLength = temp;
            byte[] package = new byte[bufferForLength.Length + bufferForMessage.Length];
            Debug.WriteLine(messageLength);
            Debug.WriteLine(bufferForLength.Length); 
            System.Buffer.BlockCopy(bufferForLength, 0, package, 0, bufferForLength.Length);
            System.Buffer.BlockCopy(bufferForMessage, 0, package, bufferForLength.Length, bufferForMessage.Length);
            client.BeginSend(package, 0, package.Length, 0, new AsyncCallback(sendCallBack), client);
        }

        private void sendCallBack(IAsyncResult ar)
        {
            Socket client = (Socket)ar.AsyncState;
            int bytessSend = client.EndSend(ar);
            Debug.WriteLine("{0} bytes sent to server", bytessSend);
        }

        private void disconnectClient()
        {
            try
            {
                client.Disconnect(true);
            }
            catch (Exception e)
            {
                eventAggregator.GetEvent<DisconnectFailEvent>().Publish(e);
            }
        }

        private void closeClient()
        {
            client.Close(1000);
        }
    }
}
