using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZSFund.LogService;

namespace GrpcDemo.Server.Common
{
    public class LoggerSource
    {
        public static readonly string DefaultAppSource = "WindFuturePriceSync";
    }
    /// <summary>
    /// 写日志
    /// </summary>
    public static class Logger
    {
        private static ZSFund.LogService.ILogService logger = new ZSFund.LogService.Api.Logger();
        public static bool Write(string title, string message, string logType, string logType2, LogLevel level, int priority, string loginName = null, string clientIP = null)
        {
            return logger.Write(LoggerSource.DefaultAppSource, title, message, logType, logType2, level, priority, loginName, clientIP).GetAwaiter().GetResult();
        }
    }
}
