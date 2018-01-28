using System;
using EasyConsole;
using HelloHome.Central.Hub.ConsoleApp.CommWithNodes;
using HelloHome.Central.Hub.ConsoleApp.LoggingConfig;

namespace HelloHome.Central.Hub.ConsoleApp
{
    public class MainPage : MenuPage
    {
        public MainPage(EasyConsole.Program program) : base("Main", program, 
            new Option("Communicate with nodes", () => program.NavigateTo<CommWithNodePage>()),
            new Option("Log configuration", () => program.NavigateTo<SetDefaultLogLevelPage>()),
            new Option("Quit", () =>
            {
                if(Input.ReadString("Confirm exit ? (Y/N)") != "Y")
                    program.NavigateHome();
            })
            )
        {
            
        }
    }
}