using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;
using EasyConsole;


namespace HelloHome.Central.Hub.ConsoleApp.LoggingConfig
{
    public class SetDefaultLogLevelPage(EasyConsole.Program program) : Page("Default log level", program)
    {
        public override void Display()
        {
            base.Display();

            var level = Input.ReadEnum<LogLevel>("Enter new log level :");
            Program.NavigateHome();
        }

        private enum LogLevel
        {
            Debug,
            Info,
            Warning,
            Error,
        }
    }
}