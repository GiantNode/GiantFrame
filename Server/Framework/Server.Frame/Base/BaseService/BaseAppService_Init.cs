﻿using Giant.Data;
using Giant.DB;
using Giant.Log;
using Giant.Net;
using Giant.Redis;
using Giant.Share;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;

namespace Server.Frame
{
    public partial class BaseAppService
    {
        public abstract void Start(string[] args);

        public virtual void Init(string[] args)
        {
            //框架的各种初始化
            this.InitBase(args);
            this.InitLogConfig();

            this.InitData();
            this.InitNetwork();
            this.InitProtocol();
            this.InitDBService();
            this.InitRedisService();
            this.InitServerFactory();
            this.InitNetworkTopology();
        }

        public virtual void InitData()
        {
            DataManager.Instance.Init();

            DBConfig.Init();
            AppConfigLibrary.Init();
            NetTopologyLibrary.Init();
        }

        public virtual void OnAccept(Session session, bool isConnect) { }

        protected virtual void InitServerFactory()
        {
            ServerFactory = new BaseServerFactory(this);
        }


        private void InitBase(string[] args)
        {
            Framework.Init(this, args);
            Framework.AppState = AppState.Starting;

            //窗口关闭事件
            SetConsoleCtrlHandler(cancelHandler, true);

            // 异步方法全部会回掉到主线程
            SynchronizationContext.SetSynchronizationContext(OneThreadSynchronizationContext.Instance);

            IdGenerator.AppId = this.AppId;
        }

        //日志配置
        private void InitLogConfig()
        {
            Logger.Init(false, this.AppType.ToString(), this.AppId, this.SubId);
        }

        //网络服务
        private void InitNetwork()
        {
            AppConfig config = AppConfigLibrary.GetNetConfig(this.AppType, this.AppId, this.SubId);
            this.InnerNetworkService = new InnerNetworkService(NetworkType.Tcp, config.InnerAddress);

            //部分App只有内部服务，Zone
            if (!string.IsNullOrEmpty(config.OutterAddress))
            {
                this.OutterNetworkService = new OutterNetworkService(NetworkType.Tcp, config.OutterAddress, OnAccept);
            }
        }

        //注册消息响应
        private void InitProtocol()
        {
            Assembly entryAssembly = Assembly.GetEntryAssembly();
            Assembly currendAssembly = Assembly.GetExecutingAssembly();
            this.InnerNetworkService.MessageDispatcher.RegisterHandler(this.AppType, entryAssembly);
            this.InnerNetworkService.MessageDispatcher.RegisterHandler(this.AppType, currendAssembly);

            if (this.OutterNetworkService != null)
            {
                this.OutterNetworkService.MessageDispatcher.RegisterHandler(this.AppType, entryAssembly);
                this.OutterNetworkService.MessageDispatcher.RegisterHandler(this.AppType, currendAssembly);
            }
        }

        //数据库服务
        private void InitDBService()
        {
            if (this.AppType.NeedDBService())
            {
                DataBaseService.Instance.Init(DataBaseType.MongoDB, DBConfig.DBHost, DBConfig.DBName,
                    DBConfig.DBAccount, DBConfig.DBPwd, DBConfig.DBTaskCount);
            }
        }

        //Redis服务
        private void InitRedisService()
        {
            if (this.AppType.NeedRedisServer())
            {
                RedisService.Instance.Init(DBConfig.RedisHost, DBConfig.RedisPwd, DBConfig.RedisTaskCount, 0);
            }
        }

        //网络拓扑
        private void InitNetworkTopology()
        {
            this.NetProxyManager = new NetProxyManager(this);
            this.NetProxyManager.Init();
        }


        #region 窗口关闭事件

        delegate bool ControlCtrlHandle(int ctrlType);

        [DllImport("kernel32.dll")]
        private static extern bool SetConsoleCtrlHandler(ControlCtrlHandle HandlerRoutine, bool Add);
        private static readonly ControlCtrlHandle cancelHandler = new ControlCtrlHandle(HandleMathord);

        private static bool HandleMathord(int ctrlType)
        {
            switch (ctrlType)
            {
                case 0:
                    Logger.Warn("无法使用 Ctrl+C 强制关闭窗口"); //Ctrl+C关闭
                    return true;
                case 2:
                    Logger.Warn("工具被强制关闭");//按控制台关闭按钮关闭
                    return true;
            }

            return false;
        }

        #endregion
    }
}
