using ProtocolBuffer;

namespace Wheesper.Infrastructure.services
{
    public interface IMessagingService
    {
        void SendMessage(ProtoMessage wheesper);
    }
}
