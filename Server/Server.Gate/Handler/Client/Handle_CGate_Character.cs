﻿using Giant.DB;
using Giant.DB.MongoDB;
using Giant.Msg;
using Giant.Net;
using Giant.Share;
using Server.Frame;
using System.Threading.Tasks;

namespace Server.Gate
{
    [MessageHandler]
    class Handle_CGate_GetCharacter : RpcMHandler<Msg_CG_GetCharacter, Msg_GC_GetCharacter>
    {
        public override async Task Run(Session session, Msg_CG_GetCharacter request, Msg_GC_GetCharacter response)
        {
            Client client = ClientManager.Instance.GetClient(session.Id);
            if (client == null)
            {
                response.Error = ErrorCode.Fail;
                return;
            }

            var query = new MongoDBQueryBatch<PlayerInfo>(DBName.Player, x => x.Account == client.Account);
            var playerInfos = await query.Task();
            if (playerInfos?.Count == 0)
            {
                response.Error = ErrorCode.HaveNoThisCharacter;
                return;
            }
            else
            {
                playerInfos.ForEach(player => response.Characters.Add(new Msg_CharacterInfo() { Uid = player.Uid, RoleId = player.RoleId }));
            }

            response.Error = ErrorCode.Success;
        }
    }

    [MessageHandler]
    class Handle_CGate_CreateCharacter : RpcMHandler<Msg_CG_CreateCharacter, Msg_GC_CreateCharacter>
    {
        public override async Task Run(Session session, Msg_CG_CreateCharacter request, Msg_GC_CreateCharacter response)
        {
            Client client = ClientManager.Instance.GetClient(session.Id);
            if (client == null)
            {
                response.Error = ErrorCode.Fail;
                return;
            }

            var query = new MongoDBQuery<PlayerInfo>(DBName.Player, x => x.Uid == request.RoleId);
            PlayerInfo playerInfo = await query.Task();
            if (playerInfo != null)
            {
                response.Error = ErrorCode.HadExistTheCharacter;
                return;
            }

            Msg_GateM_GetUid msg = new Msg_GateM_GetUid();
            Msg_MGate_GetUid result = (await AppService.Instacne.ManagerServer.Call(msg)) as Msg_MGate_GetUid;
            if (result.Error != ErrorCode.Success)
            {
                response.Error = ErrorCode.Fail;
                return;
            }

            PlayerInfo player = await CreateCharacter(response, client.Account, result.Uid, request.RoleId);
            response.Character = new Msg_CharacterInfo() { Uid = player.Uid, RoleId = request.RoleId };

            response.Error = ErrorCode.Success;
        }

        private async Task<PlayerInfo> CreateCharacter(Msg_GC_CreateCharacter response, string account, int uid, int heroId)
        {
            PlayerInfo player = new PlayerInfo()
            {
                Account = account,
                Uid = uid,
                RoleId = heroId
            };

            var query = new MongoDBInsert<PlayerInfo>(DBName.Player, player);
            await query.Task();
            return player;
        }
    }
}
