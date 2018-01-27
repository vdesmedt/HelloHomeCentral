using System;
using System.Linq;
using System.Threading;
using EasyConsole;
using HelloHome.Central.Domain;
using HelloHome.Central.Hub.MessageChannel;
using HelloHome.Central.Hub.MessageChannel.Messages.Commands;
using Microsoft.EntityFrameworkCore;

namespace HelloHome.Central.Hub.ConsoleApp.CommWithNodes
{
    public class SendRestartPage : Page
    {
        private readonly IMessageChannel _msgChannel;
        private readonly IUnitOfWork _unitOfWork;

        public SendRestartPage(EasyConsole.Program program, IMessageChannel msgChannel, IUnitOfWork unitOfWork) : base(
            "Restart node", program)
        {
            _msgChannel = msgChannel;
            _unitOfWork = unitOfWork;
        }

        public override void Display()
        {
            base.Display();

            var nodeId = Input.ReadInt("Enter node Id :", 1, 254);
            var dbNode = _unitOfWork.Nodes.Include(_ => _.Metadata).SingleOrDefault(_ => _.Id == nodeId);
            if (dbNode == null)
            {
                Output.WriteLine(ConsoleColor.Red, $"Node with id {nodeId} not found in database");
            }
            else
            {
                var confirm = Input.ReadString($"Will send restart to node {dbNode.Metadata.Name}. Confirm (Y/N)?");
                if (confirm != "Y")
                {
                    Output.WriteLine(ConsoleColor.Red, "Cancelled");
                }
                else
                {
                    _msgChannel.SendAsync(new RestartCommand {ToRfAddress = dbNode.RfAddress}, CancellationToken.None)
                        .GetAwaiter().GetResult();
                    Output.WriteLine(ConsoleColor.Blue, "Restart sent");
                }
            }

            Output.DisplayPrompt("Press any key");
            Console.ReadKey();
            Program.NavigateBack();
        }
    }
}