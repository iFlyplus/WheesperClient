using Microsoft.Practices.Prism.Events;

namespace Wheesper.Infrastructure.events
{
    public class ShowLoginFaceViewEvent : CompositePresentationEvent<string> { }
    public class ShowWheesperViewEvent : CompositePresentationEvent<object> { }
    public class LoginEvent : CompositePresentationEvent<string> { }
}
