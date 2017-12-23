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


}
