using NLog;
using NLog.Targets;

namespace URLChecker
{
    public class Logger
    {
        public static void Configure(string ErrorFilename, string OkFilename)
        {
            var config = new NLog.Config.LoggingConfiguration();

            // Targets where to log to: File and Console
            var logfile = new FileTarget("logfile")
            {
                FileName = OkFilename,
                Layout = "${longdate}|${level:uppercase=true}|${logger}|${message}"
            };

            var logError = new FileTarget("logfile")
            {
                FileName = ErrorFilename,
                Layout = "${longdate}|${level:uppercase=true}|${logger}|${message}"
            };
            //var logconsole = new NLog.Targets.ConsoleTarget("logconsole");

            // Rules for mapping loggers to targets            
            // config.AddRule(LogLevel.Info, LogLevel.Fatal, logconsole);
            config.AddRule(LogLevel.Info, LogLevel.Info, logfile);
            config.AddRule(LogLevel.Error, LogLevel.Fatal, logError);

            // Apply config           
            LogManager.Configuration = config;
        }
    }
}
