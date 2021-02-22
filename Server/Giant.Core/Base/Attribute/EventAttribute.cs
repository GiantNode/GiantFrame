﻿using Giant.EnumUtil;
using System;

namespace Giant.Core
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class EventAttribute : ObjectAttribute
    {
        public EventType EventType { get; }

        public EventAttribute(EventType type)
        {
            EventType = type;
        }
    }
}
