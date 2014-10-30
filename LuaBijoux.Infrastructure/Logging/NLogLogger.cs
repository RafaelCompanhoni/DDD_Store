using System;
using LuaBijoux.Core.Logging;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace LuaBijoux.Infrastructure.Logging
{
    public class NLogLogger : ILogger
    {
        public static Logger Instance { get; private set; }

        public NLogLogger()
        {
            #if DEBUG
                // Setup the logging view for Sentinel - http://sentinel.codeplex.com
                var sentinalTarget = new NLogViewerTarget()
                {
                    Name = "sentinel",
                    Address = "udp://127.0.0.1:9999",
                    IncludeNLogData = false
                };
                var sentinalRule = new LoggingRule("*", LogLevel.Trace, sentinalTarget);
                LogManager.Configuration.AddTarget("sentinel", sentinalTarget);
                LogManager.Configuration.LoggingRules.Add(sentinalRule);
            #endif

            LogManager.ReconfigExistingLoggers();
            Instance = LogManager.GetCurrentClassLogger();
        }

        public void Log(string message)
        {
            Instance.Info(message);
        }

        public void Log(Exception ex)
        {
            Instance.Error(ex);
        }
    }
}
