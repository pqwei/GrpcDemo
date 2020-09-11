using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using GrpcDemo.Server.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ZSFund.LogService;

namespace GrpcDemo.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            File.AppendAllText("D:\\log.txt", "服务开始启动", Encoding.UTF8);
            //Logger.Write("服务开始启动", string.Empty, string.Empty, string.Empty, LogLevel.Information, 0);
            try
            {
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                File.AppendAllText("D:\\log.txt", "服务启动失败" + ex.ToString(), Encoding.UTF8);
                //Logger.Write("服务启动失败", ex.ToString(), string.Empty, string.Empty, LogLevel.Information, 0);
            }
            File.AppendAllText("D:\\log.txt", "服务启动成功", Encoding.UTF8);
            //Logger.Write("服务启动成功", string.Empty, string.Empty, string.Empty, LogLevel.Information, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                    .ConfigureAppConfiguration((c, h) =>
                    {
                        h.AddJsonFile($"appsettings.json", true, true);
                    })
                    .UseStartup<Startup>();
                });

    }
}
