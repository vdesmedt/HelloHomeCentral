using System;
using EasyConsole;
using HelloHome.Central.Hub.ConsoleApp.CommWithNodes;

namespace HelloHome.Central.Hub.ConsoleApp
{
    public class MainPage : MenuPage
    {
        public MainPage(EasyConsole.Program program) : base("Main", program, 
            new Option("Communicate with nodes", () => program.NavigateTo<CommWithNodePage>()),
            new Option("Quit", () => { Output.WriteLine(ConsoleColor.Red, "[End]");})
            )
        {
            
        }
    }
}