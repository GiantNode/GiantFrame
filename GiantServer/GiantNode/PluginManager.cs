﻿using Giant.Framework;
using System.Collections.Generic;

namespace GiantNode
{
    class PluginManager
    {
        public static void StartPlugins()
        {
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("FrontPort", "8099");

            List<uint> allNodes = new List<uint>(1);

            NodeHandle nodeHandle = new NodeHandle("GatewayPlugin.dll", new NodeRuntime(1, 1, "GatewayPlugin", allNodes.ToArray(), param));

            nodeHandle.ToStart();

            m_Plugins.Add(nodeHandle);
        }


        static List<NodeHandle> m_Plugins = new List<NodeHandle>();
    }
}