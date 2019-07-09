﻿using Giant.Log;
using Giant.Msg;
using Giant.Net;
using Giant.Share;

namespace Server.App
{
    [MessageHandler(AppType.AllServer)]
    public class Handle_HeatBeat_Ping : MHandler<HeartBeat_Ping>
    {
        public override void Run(Session session, HeartBeat_Ping message)
        {
            Logger.Info($"heart beat ping from appType {(AppType)message.AppType} appId {message.AppId}");

            HeartBeat_Pong pong = new HeartBeat_Pong()
            {
                AppType = (int)AppService.Instacne.AppType,
                AppId = AppService.Instacne.AppId,
            };

            session.Notify(pong);
        }
    }

    [MessageHandler(AppType.AllServer)]
    public class Handle_HeatBeat_Pong : MHandler<HeartBeat_Pong>
    {
        public override void Run(Session session, HeartBeat_Pong message)
        {
            Logger.Info($"heart beat pong from appType {(AppType)message.AppType} appId {message.AppId}");
        }
    }
}
