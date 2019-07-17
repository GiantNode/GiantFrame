﻿using Giant.Msg;
using Giant.Net;
using Giant.Share;

namespace Client
{
    public partial class Player
    {
        public Session Session;
        private long heartBeatLastTime = TimeHelper.NowSeconds;

        public string Account { get; private set; }
        public int Uid { get; private set; }

        public Player(string account, int uid, Session session)
        {
            this.Account = account;
            this.Session = session;
            this.Uid = uid;
        }

        public void Update()
        {
            CheckHeartBeat();
        }

        public void OnDisconnected()
        {
        }

        private async void CheckHeartBeat()
        {
            if (Session == null || !Session.IsConnected)
            {
                return;
            }

            if ((TimeHelper.NowSeconds - heartBeatLastTime) > 30)
            {
                heartBeatLastTime = TimeHelper.NowSeconds;

                Msg_CG_HeartBeat_Ping msg = new Msg_CG_HeartBeat_Ping();
                await Session.Call(msg);
            }
        }
    }
}
