﻿using System;

namespace Giant.Msg
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class BaseAttribute : Attribute
    {
        public Type AttributeType { get; }

        public BaseAttribute()
        {
            AttributeType = GetType();
        }
    }

    public class MessageAttribute : BaseAttribute
    {
        public ushort Opcode { get; }

        public MessageAttribute(ushort opcode)
        {
            Opcode = opcode;
        }
    }
}