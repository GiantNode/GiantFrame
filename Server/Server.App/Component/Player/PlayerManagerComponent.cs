﻿using Giant.Core;
using System.Collections.Generic;

namespace Server.App
{
    class PlayerManagerComponent : InitSystem, IUpdate
    {
        private Dictionary<int, Player> playerList = new Dictionary<int, Player>();
        private Dictionary<int, Player> offlinePlayerList = new Dictionary<int, Player>();

        public static PlayerManagerComponent Instance { get; private set; }

        public override void Init()
        {
            Instance = this;
        }

        public void Update(double dt)
        {
        }

        public Player GetPlayer(int uid)
        {
            playerList.TryGetValue(uid, out var player);
            return player;
        }

        public Player GetOfflinePlayer(int uid)
        {
            offlinePlayerList.TryGetValue(uid, out var player);
            return player;
        }
    }
}
