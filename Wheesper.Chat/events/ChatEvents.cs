using Microsoft.Practices.Prism.Events;

namespace Wheesper.Login.events
{
    public class LoadContactViewEvent : CompositePresentationEvent<object> { };
    public class LoadChatViewEvent : CompositePresentationEvent<object> { };
    public class ShowSystemMessageViewEvent : CompositePresentationEvent<object> { };
    public class CloseSystemMessageViewEvent : CompositePresentationEvent<object> { };
    public class ShowSolveContactApplyViewEvent : CompositePresentationEvent<object> { };
    public class CloseSolveContactApplyViewEvent : CompositePresentationEvent<object> { };
    public class ShowChangeContactInfoViewEvent : CompositePresentationEvent<object> { };
    public class CloseChangeContactInfoViewEvent : CompositePresentationEvent<object> { };


    public class ShowUserNotExistViewEvent : CompositePresentationEvent<string> { };
    public class ShowUserExistViewEvent : CompositePresentationEvent<string> { };
    public class CloseUserExistOrNotExistViewEvent : CompositePresentationEvent<bool> { };



    public class ShowUserInfoViewEvent : CompositePresentationEvent<object> { };
    public class CloseUserInfoViewEvent : CompositePresentationEvent<object> { };

    // thrown by view
    public class MouseKeyDownAContactEvent : CompositePresentationEvent<string> { };
    public class MouseKeyDownASystemMessageEvent : CompositePresentationEvent<int> { };
}