﻿using Giant.DB;
using Giant.Log;
using Giant.Net;
using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace Giant.Frame
{
    public class BaseService
    {
        public NetworkService NetworkService { get; protected set; }

        public DataBaseService DBService { get; protected set; }

        public int MainId { get; private set; }

        public ServerType ServerType { get; set; }



        public virtual void Init()
        {
            SetConsoleCtrlHandler(cancelHandler, true);

            // 异步方法全部会回掉到主线程
            SynchronizationContext.SetSynchronizationContext(OneThreadSynchronizationContext.Instance);

            //框架的各种初始化工作

        }

        public virtual void Update()
        {
            try
            {
                OneThreadSynchronizationContext.Instance.Update();

                this.NetworkService.Update();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
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
