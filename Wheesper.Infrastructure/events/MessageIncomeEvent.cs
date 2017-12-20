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
}
