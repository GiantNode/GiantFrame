﻿using Giant.Msg;
using Giant.Net;
using System.Threading.Tasks;

namespace Server.Manager
{
    [MessageHandler]
    public class Handle_GetUid : RpcMHandler<Msg_GateM_GetUid, Msg_MGate_GetUid>
    {
        public override Task Run(Session session, Msg_GateM_GetUid request, Msg_MGate_GetUid response)
        {
            response.Uid = AppService.Instacne.UidManager.GetUid();
            return Task.CompletedTask;
        }
    }

    //public class Handle_BalanceZone : RpcMHandler<Msg_GateM_BalanceZone, Msg_MGate_BalanceZone>
    //{
    //    public override Task Run(Session session, Msg_GateM_BalanceZone request, Msg_MGate_BalanceZone response)
    //    {
    //    }
    //}
}
