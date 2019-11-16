﻿using Giant.Core;
using Giant.Log;
using Giant.Msg;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Giant.Net
{
    public class Session : IDisposable
    {
        private int rpcId;
        private readonly BaseChannel channel;//通讯对象
        private readonly byte[] opcodeBytes = new byte[2];
        private readonly Dictionary<int, Action<IResponse>> responseCallback = new Dictionary<int, Action<IResponse>>();//消息回调

        public NetworkService NetworkService { get; private set; }

        public long Id { get; private set; }

        public bool IsConnected => channel.IsConnected;
        public IPEndPoint RemoteIPEndPoint => channel.IPEndPoint;

        private Action<Session, bool> onConnectCallback;
        public event Action<Session, bool> OnConnectCallback
        {
            add { onConnectCallback += value; }
            remove { onConnectCallback -= value; }
        }


        public Session(NetworkService networkService, BaseChannel baseChannel)
        {
            Id = baseChannel.InstanceId;
            NetworkService = networkService;
            channel = baseChannel;

            channel.OnReadCallback += OnRead;
            channel.OnErrorCallback += OnError;
            channel.OnConnectCallback += OnConnect;
        }

        public void Reply(IMessage message)
        {
            Notify(message);
        }

        public void Notify(IMessage message)
        {
            ushort opcode = NetworkService.MessageDispatcher.GetOpcode(message.GetType());
            Notify(opcode, message);
        }

        public Task<IResponse> Call(IRequest request)
        {
            request.RpcId = ++rpcId;

            ushort opcode = NetworkService.MessageDispatcher.GetOpcode(request.GetType());

            TaskCompletionSource<IResponse> tcs = new TaskCompletionSource<IResponse>();

            responseCallback[rpcId] = (response) =>
            {
                try
                {
                    tcs.SetResult(response);

                    //不能以异常的形式返回，客户端需要更具具体的错误码来做相应的操作
                    //if (response.Error == ErrorCode.ERR_Success)
                    //{
                    //    tcs.SetResult(response);
                    //}
                    //else
                    //{
                    //    tcs.SetException(new Exception($"ErrorCode {response.Error} Message {response.Message}"));
                    //}
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                }
            };

            Notify(opcode, request);

            return tcs.Task;
        }

        public Task<IResponse> Call(IRequest request, CancellationToken cancellation)
        {
            request.RpcId = ++rpcId;

            ushort opcode = NetworkService.MessageDispatcher.GetOpcode(request.GetType());

            TaskCompletionSource<IResponse> tcs = new TaskCompletionSource<IResponse>();

            responseCallback[rpcId] = (response) =>
            {
                try
                {
                    tcs.SetResult(response);

                    //不能以异常的形式返回，客户端需要更具具体的错误码来做相应的操作
                    //if (response.Error == ErrorCode.ERR_Success)
                    //{
                    //    tcs.SetResult(response);
                    //}
                    //else
                    //{
                    //    tcs.SetException(new Exception($"ErrorCode {response.Error} Message {response.Message}"));
                    //}
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                }
            };

            cancellation.Register(() => responseCallback.Remove(rpcId));

            Notify(opcode, request);

            return tcs.Task;
        }

        public void Start()
        {
            channel.Start();
        }

        public void Dispose()
        {
            channel.Dispose();

            //清空所有消息回调
            responseCallback.Clear();
        }

        private void Notify(ushort opcode, IMessage message)
        {
            var stream = channel.Stream;
            opcodeBytes.WriteTo(0, opcode);

            stream.Seek(0, SeekOrigin.Begin);
            stream.Write(opcodeBytes, 0, opcodeBytes.Length);
            stream.SetLength(Packet.MessageIndex);

            ProtoHelper.ToStream(stream, message);
            stream.Seek(0, SeekOrigin.Begin);

            channel.Send(stream);
        }

        private void OnRead(MemoryStream memoryStream)
        {
            //消息id
            ushort opcode = BitConverter.ToUInt16(memoryStream.GetBuffer(), Packet.OpcodeIndex);
            memoryStream.Seek(Packet.MessageIndex, SeekOrigin.Begin);

            Type msgType = NetworkService.MessageDispatcher.GetMessageType(opcode);
            IMessage message = NetworkService.MessageParser.DeserializeFrom(memoryStream, msgType) as IMessage;

            if (message is IResponse response)
            {
                if (responseCallback.TryGetValue(response.RpcId, out var action))
                {
                    action(response);
                    responseCallback.Remove(response.RpcId);
                }
            }
            else
            {
                NetworkService.MessageDispatcher.Dispatch(this, opcode, message);
            }
        }

        public void OnError(object error)
        {
            switch (error)
            {
                case int errorCode:
                    {
                        Logger.Error($"ErrorCode {errorCode}");
                    }
                    break;
                case SocketError socketError:
                    Logger.Error($"SocketError {socketError}");
                    break;
                default:
                    Logger.Error(error);
                    break;
            }

            NetworkService.Remove(this);

            Dispose();
        }

        public void OnConnect(bool connState)
        {
            onConnectCallback?.Invoke(this, connState);
        }

    }
}
