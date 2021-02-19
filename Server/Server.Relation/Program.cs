﻿using Giant.Core;
using Giant.Framework;

namespace Server.Relation
{
    class Program
    {
        static void Main(string[] args)
        {
            ComponentFactory.CreateComponent<BaseServiceComponent, string[]>(args);
        }
    }
}
