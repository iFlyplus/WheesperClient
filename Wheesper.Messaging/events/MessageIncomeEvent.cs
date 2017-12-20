using Microsoft.Practices.Prism.Events;
using ProtocolBuffer;

namespace Wheesper.Messaging.events
{
    public class MessageIncomeEvent : CompositePresentationEvent<ProtoMessage> { }
}
