﻿using Giant.Core;
using Giant.EnumUtil;
using Giant.Framework;

namespace Server.Zone
{
    [Event(EventType.CommandLine)]
    public class CommandLineEvent : Event<string>
    {
        public override void Handle(string a)
        {
            switch (a)
            {
                case "load":
                    Scene.EventSystem.Load();
                    break;
                case "BK":
                    Scene.Pool.GetComponent<BenchmarkComponent>()?.Test();
                    break;
                case "HttpBK":
                    Scene.Pool.GetComponent<HttpBenchmarkComponent>()?.Get();
                    Scene.Pool.GetComponent<HttpBenchmarkComponent>()?.Post();
                    break;
            }
        }
    }
}
