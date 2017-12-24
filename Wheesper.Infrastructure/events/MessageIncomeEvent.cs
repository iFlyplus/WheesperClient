using Microsoft.Practices.Prism.Events;
using ProtocolBuffer;

namespace Wheesper.Messaging.events
{
    public class MessageIncomeEvent : CompositePresentationEvent<ProtoMessage> { }

    public class MsgSigninResponseEvent : CompositePresentationEvent<ProtoMessage> { }
    public class MsgSignupMailResponseEvent : CompositePresentationEvent<ProtoMessage> { }
    public class MsgSignupCaptchaResponseEvent : CompositePresentationEvent<ProtoMessage> { }
    public class MsgSignupInfoResponseEvent : CompositePresentationEvent<ProtoMessage> { }
    public class MsgPasswordModifyCaptchaResponseEvent : CompositePresentationEvent<ProtoMessage> { }
    public class MsgPasswordModifyResponseEvent : CompositePresentationEvent<ProtoMessage> { }

    public class MsgUserInfoQueryResponseEvent : CompositePresentationEvent<ProtoMessage> { }
    public class MsgUserInfoModifyResponseEvent : CompositePresentationEvent<ProtoMessage> { }
    public class MsgContactListResponseEvent : CompositePresentationEvent<ProtoMessage> { }
    public class MsgContactMailCheckResponseEvent : CompositePresentationEvent<ProtoMessage> { }
    public class MsgContactApplyResponseEvent : CompositePresentationEvent<ProtoMessage> { }
    public class MsgContactReplyResponseEvent : CompositePresentationEvent<ProtoMessage> { }
    public class MsgContactApplyingInfoPushMessageEvent : CompositePresentationEvent<ProtoMessage> { }
    public class MsgContactReplyingInfoPushMessageEvent : CompositePresentationEvent<ProtoMessage> { }
    public class MsgContactRemarkModifyResponseEvent : CompositePresentationEvent<ProtoMessage> { }

    public class MsgChatPrivateMessageResponseEvent : CompositePresentationEvent<ProtoMessage> { }
    public class MsgChatPrivateMessagePushMessageEvent : CompositePresentationEvent<ProtoMessage> { }

    public class MsgChatGroupMessageResponseEvent : CompositePresentationEvent<ProtoMessage> { }
    public class MsgChatGroupMessagePushMessageEvent : CompositePresentationEvent<ProtoMessage> { }
    /*
     ChatPrivateMessageRequest chatPrivateMessageRequest = 41;
    ChatPrivateMessageResponse chatPrivateMessageResponse = 42;
    ChatPrivateMessagePushMessage chatPrivateMessagePushMessage = 43;    
    ChatPrivateMessagePushAckMessage chatPrivateMessagePushAckMessage = 44;

        ChatGroupMessageRequest chatGroupMessageRequest = 45;
    ChatGroupMessageResponse chatGroupMessageResponse = 46;
    ChatGroupMessagePushMessage chatGroupMessagePushMessage = 47;

        ChatGroupMessagePushAckMessage ChatGroupMessagePushAckMessage = 48;

   // MsgChatPrivateMessageResponseEvent
   // MsgChatPrivateMessagePushMessageEvent
     */
}
