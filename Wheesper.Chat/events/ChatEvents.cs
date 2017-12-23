﻿using Microsoft.Practices.Prism.Events;

namespace Wheesper.Login.events
{
    public class ShowSystemMessageViewEvent : CompositePresentationEvent<object> { };
    public class CloseSystemMessageViewEvent : CompositePresentationEvent<object> { };
    public class ShowSolveContactApplyViewEvent : CompositePresentationEvent<object> { };
    public class CloseSolveContactApplyViewEvent : CompositePresentationEvent<object> { };
}