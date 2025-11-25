using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HelloHome.Central.Domain.CmdQrys.Base;
using HelloHome.Central.Domain.Entities;
using HelloHome.Central.Domain.Entities.Includes;
using Microsoft.EntityFrameworkCore;

namespace HelloHome.Central.Domain.CmdQrys;

public interface IGetNodeWithLastValuesQuery
{
    Task<Node> ExecuteAsync(int nodeId, NodeInclude includes = NodeInclude.None);
}

public class GetNodeWithLastValuesQuery(IUnitOfWork ctx) : IQuery, IGetNodeWithLastValuesQuery
{
    public async Task<Node> ExecuteAsync(int nodeId, NodeInclude includes = NodeInclude.None)
    {
        var node = await ctx.Nodes
            .Where(n => n.Id == nodeId)
            .Include(NodeInclude.Ports)
            .SingleOrDefaultAsync();
        foreach (var port in node.Ports)
        {
            switch (port)
            {
                case EnvironmentSensor envSensor:
                    envSensor.History = new List<EnvironmentHistory>
                    {
                        await ctx.PortHistory.OfType<EnvironmentHistory>().OrderByDescending(h => h.Id)
                            .FirstOrDefaultAsync(h => h.PortId == port.Id)
                    };
                    break;
                case NodeHealthSensor healthSensor:
                    healthSensor.History = new List<NodeHealthHistory>
                    {
                        await ctx.PortHistory.OfType<NodeHealthHistory>().OrderByDescending(h => h.Id)
                            .FirstOrDefaultAsync(h => h.PortId == port.Id)
                    };
                    break;
                case PulseSensor pulseSensor:
                    pulseSensor.History = new List<PulseHistory>
                    {
                        await ctx.PortHistory.OfType<PulseHistory>().OrderByDescending(h => h.Id)
                            .FirstOrDefaultAsync(h => h.PortId == port.Id)
                    };
                    break;
                case PushButtonSensor pushButtonSensor:
                    pushButtonSensor.History = new List<PushButtonHistory>
                    {
                        await ctx.PortHistory.OfType<PushButtonHistory>().OrderByDescending(h => h.Id)
                            .FirstOrDefaultAsync(h => h.PortId == port.Id)
                    };
                    break;
                case RelayActuator relayActuator:
                    relayActuator.History = new List<RelayHistory>
                    {
                        await ctx.PortHistory.OfType<RelayHistory>().OrderByDescending(h => h.Id)
                            .FirstOrDefaultAsync(h => h.PortId == port.Id)
                    };
                    break;
                    
                default:
                    throw new NotImplementedException(
                        $"Port type {port.GetType()} not supported yet by : GetNodeWithLastValuesQuery");
            }
        }

        return node;
    }    
}