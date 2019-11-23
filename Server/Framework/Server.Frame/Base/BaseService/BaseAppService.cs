﻿using Giant.Data;
using Giant.Log;
using Giant.Net;
using Giant.Share;
using System;

namespace Server.Frame
{
    public abstract partial class BaseAppService
    {
        public BaseServerFactory ServerFactory { get; protected set; }
        public NetProxyManager NetProxyManager { get; private set; }
        public InnerNetworkService InnerNetworkService { get; private set; }
        public OutterNetworkService OutterNetworkService { get; private set; }
        public AppConfig AppConfig { get; private set; }

        public AppType AppType => Framework.AppType;
        public int AppId => Framework.AppId;
        public int SubId => Framework.SubId;

        public virtual void Update(float dt)
        {
            try
            {
                OneThreadSynchronizationContext.Instance.Update();//异步回调处理

                Timer.Instance.Update();//定时器

                InnerNetworkService.Update();
                NetProxyManager.Update();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        public virtual void InitDone()
        {
            NetProxyManager.Start();

            Framework.AppState = AppState.Started;
        }

        public virtual void StopApp()
        {
            Framework.AppState = AppState.Stopping;
        }
    }
}