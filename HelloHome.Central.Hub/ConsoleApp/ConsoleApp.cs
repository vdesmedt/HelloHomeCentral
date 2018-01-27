using HelloHome.Central.Domain;
using HelloHome.Central.Hub.ConsoleApp.CommWithNodes;
using HelloHome.Central.Hub.MessageChannel;

namespace HelloHome.Central.Hub.ConsoleApp
{
    public class ConsoleApp : EasyConsole.Program
    {
        public ConsoleApp(IMessageChannel msgChannel, IUnitOfWork unitOfWork) : base("HelloHome Central", breadcrumbHeader : true)
        {
            AddPage(new MainPage(this));
            AddPage(new CommWithNodePage(this));
            AddPage(new SendRestartPage(this, msgChannel, unitOfWork));
            SetPage<MainPage>();
        }
    }
}