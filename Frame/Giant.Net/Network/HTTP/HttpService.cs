﻿using Giant.Log;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace Giant.Net
{
    public class HttpService
    {
        private HttpListener httpListener;

        public void Start(List<int> ports)
        {
            try
            {
                httpListener = new HttpListener();
                ports.ForEach(port =>
                {
                    httpListener.Prefixes.Add($"http://*:{port}/");
                    Logger.Debug("Http listen port" + port);
                });

                httpListener.Start();
                AcceptAsync();
            }
            catch (HttpListenerException e)
            {
                if (e.ErrorCode == 5)
                {
                    throw new Exception($"CMD管理员中输入: netsh http add urlacl url=http://*:8080/ user=Everyone", e);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private async void AcceptAsync()
        {
            while (true)
            {
                var context = await httpListener.GetContextAsync();
                DoContext(context);
            }
        }

        private async void DoContext(HttpListenerContext context)
        {
            try
            {
                //网页请求
                //if (context.Request.RawUrl.Contains("favicon.ico"))
                //{
                //    context.Response.Close();
                //    context.Response.Abort();
                //    return;
                //}


                Dictionary<string, string> param = new Dictionary<string, string>();

                switch (context.Request.HttpMethod)
                {
                    case ("GET"):
                        foreach (string key in context.Request.QueryString)
                        {
                            param.Add(key, context.Request.QueryString[key]);
                        }
                        break;
                    case "POST":
                        using (StreamReader reader = new StreamReader(context.Request.InputStream))
                        {
                            string content = await reader.ReadToEndAsync();
                            param = HttpHelper.ParaseContent(content);
                        }
                        break;
                }

                using (StreamWriter stream = new StreamWriter(context.Response.OutputStream))
                {
                    await stream.WriteAsync(DateTime.Now.ToString());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

    }
}
