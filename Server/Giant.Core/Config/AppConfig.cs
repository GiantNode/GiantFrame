﻿using System.Collections.Generic;

namespace Giant.Core
{
    public class AppConfig
    {
        public AppType AppType { get; set; }
        public int AppId { get; set; }
        public int SubId { get; set; }
        public string InnerAddress { get; set; }
        public string OutterAddress { get; set; }

        public List<int> HttpPorts { get; set; }
    }
}