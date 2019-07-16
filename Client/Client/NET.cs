﻿using Giant.Net;
using Giant.Share;
using Giant.Msg;
using System;
using System.Reflection;

namespace Client
{
    public partial class NET
    {
        static long heartBeatLastTime = TimeHelper.NowSeconds; 
        static NetworkService networkService;

        static Session session;
        public static Session Session => session;

        public static bool IsConnected => session != null && session.IsConnected;

        public static void Init()
        {
            networkService = new OutterNetworkService(NetworkType.Tcp)
            {
                MessageParser = new ProtoPacker(),
                MessageDispatcher = new MessageDispatcher()
            };
            RegistHandler();

            session = networkService.Create("127.0.0.1:8001");
            session.OnConnectCallback += OnConnect;
            session.Start();
        }

        public static void Update()
        {
            CheckHeartBeat();
        }

        private static void RegistHandler()
        {
            networkService.MessageDispatcher.RegisterHandler(AppType.AllServer, Assembly.GetEntryAssembly());
        }

        private static void OnConnect(Session session, bool state)
        {
            if (!state)
            {
                Console.WriteLine($"Disconnected from {session.RemoteIPEndPoint.ToString()}");
            }
            else
            {
                Console.WriteLine($"Connected to {session.RemoteIPEndPoint.ToString()} success");
            }
        }

        private static async void CheckHeartBeat()
        {
            if (session == null || !IsConnected)
            {
                return;
            }

            if ((TimeHelper.NowSeconds - heartBeatLastTime) > 30)
            {
                Msg_CG_HeartBeat_Ping msg = new Msg_CG_HeartBeat_Ping();
                await session.Call(msg);
                heartBeatLastTime = TimeHelper.NowSeconds;
            }
        }
             
    }
}
