﻿using System;
using Microsoft.Practices.Prism.Events;

namespace Wheesper.Infrastructure.events
{
    public class DisconnectFailEvent : CompositePresentationEvent<Exception> { }
}
