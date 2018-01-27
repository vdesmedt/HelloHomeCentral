using EasyConsole;

namespace HelloHome.Central.Hub.ConsoleApp.CommWithNodes
{
    public class CommWithNodePage : MenuPage
    {
        public CommWithNodePage(EasyConsole.Program program) : base("Communicate with node", program,
            new Option("Restart node", () => program.NavigateTo<SendRestartPage>()))
        {
        }
    }
}