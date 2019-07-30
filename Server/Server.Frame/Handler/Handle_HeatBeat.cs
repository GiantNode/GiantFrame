﻿using Giant.Log;
using Giant.Msg;
using Giant.Net;
using Giant.Share;
using System.Threading.Tasks;

namespace Server.Frame
{
    [MessageHandler(AppType.AllServer)]
    public class Handle_HeatBeat_Ping : MRpcHandler<Msg_HeartBeat_Ping, Msg_HeartBeat_Pong>
    {
        public override Task Run(Session session, Msg_HeartBeat_Ping request, Msg_HeartBeat_Pong response)
        {
            Logger.Info($"heart beat ping from appType {(AppType)request.AppType} appId {request.AppId}");

            response.AppType = (int)Framework.AppType;
            response.AppId = Framework.AppId;

            return Task.CompletedTask;
        }
    }
}
