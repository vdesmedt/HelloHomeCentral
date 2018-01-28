using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;
using EasyConsole;
using NLog;

namespace HelloHome.Central.Hub.ConsoleApp.LoggingConfig
{
    public class SetDefaultLogLevelPage : Page
    {
        public SetDefaultLogLevelPage(EasyConsole.Program program) : base("Default log level", program)
        {
        }

        public override void Display()
        {
            base.Display();

            var level = Input.ReadEnum<LogLevel>("Enter new log level :");
            var logRule = LogManager.Configuration.LoggingRules.Single(_ => _.LoggerNamePattern == "*");
            foreach(var ll in logRule.Levels)
                logRule.DisableLoggingForLevel(ll);
            logRule.EnableLoggingForLevels(Level2Level(level), NLog.LogLevel.Fatal);
            LogManager.ReconfigExistingLoggers();
            Program.NavigateHome();
        }

        private enum LogLevel
        {
            Debug,
            Info,
            Warning,
            Error,
        }

        private NLog.LogLevel Level2Level(LogLevel lvl)
        {
            switch (lvl)
            {
                case LogLevel.Debug:
                    return NLog.LogLevel.Debug;
                case LogLevel.Info:
                    return NLog.LogLevel.Info;
                case LogLevel.Warning:
                    return NLog.LogLevel.Warn;
                case LogLevel.Error:
                    return NLog.LogLevel.Error;
                default:
                    return NLog.LogLevel.Off;
            }
        }
    }
}