﻿using System;

namespace Giant.Core
{
    public interface IEvent
    {
        void Run() { }
        void Run(object a) { }
        void Run(object a, object b) { }
        void Run(object a, object b, object c) { }
        void Run(object a, object b, object c, object d) { }
    }

    public abstract class Event : IEvent
    {
        public void Run()
        {
            Handle();
        }

        public abstract void Handle();
    }

    public abstract class Event<A> : IEvent
    {
        public void Run(object o)
        {
            Handle((A)o);
        }

        public new Type GetType() => typeof(A);

        public abstract void Handle(A a);
    }

    public abstract class Event<A, B> : IEvent
    {
        public void Run(object a, object b)
        {
            Handle((A)a, (B)b);
        }

        public new Type GetType() => typeof(A);

        public abstract void Handle(A a, B b);
    }

    public abstract class Event<A, B, C> : IEvent
    {
        public void Run(object a, object b, object c)
        {
            Handle((A)a, (B)b, (C)c);
        }

        public new Type GetType() => typeof(A);

        public abstract void Handle(A self, B a, C b);
    }

    public abstract class Event<A, B, C, D> : IEvent
    {
        public void Run(object a, object b, object c, object d)
        {
            Handle((A)a, (B)b, (C)c, (D)d);
        }

        public new Type GetType() => typeof(A);

        public abstract void Handle(A self, B a, C b, D d);
    }
}
