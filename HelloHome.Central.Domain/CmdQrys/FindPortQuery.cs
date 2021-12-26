using System.Linq;
using System.Threading.Tasks;
using HelloHome.Central.Domain.CmdQrys.Base;
using HelloHome.Central.Domain.Entities;
using HelloHome.Central.Domain.Entities.Includes;
using Microsoft.EntityFrameworkCore;

namespace HelloHome.Central.Domain.CmdQrys
{
    public interface IFindPortQuery : IQuery
    {
        Task<Port> ByNodeRfAndPortNumberAsyn(int rfAddress, byte portNumber, PortInclude includes);
        Task<Port> ByNodeIdAndPortNumberAsync(int nodeId, int portNumber, PortInclude includes);
        Task<T> ByNodeIdAndPortNumberAsync<T>(int nodeId, int portNumber, PortInclude includes) where T:Port;
    }

    public class FindPortQuery : IFindPortQuery
    {
        private readonly IUnitOfWork _ctx;

        public FindPortQuery(IUnitOfWork ctx)
        {
            _ctx = ctx;
        }

        public async Task<Port> ByNodeRfAndPortNumberAsyn(int rfAddress, byte portNumber, PortInclude includes)
        {
            return await _ctx.Ports
                .Include(includes)
                .SingleOrDefaultAsync(p => p.Node.RfAddress == rfAddress && p.PortNumber == portNumber);
        }

        public async Task<Port> ByNodeIdAndPortNumberAsync(int nodeId, int portNumber, PortInclude includes)
        {
            return await _ctx.Ports
                .Include(includes)
                .SingleOrDefaultAsync(p => p.NodeId == nodeId && p.PortNumber == portNumber);
        }
        public async Task<T> ByNodeIdAndPortNumberAsync<T>(int nodeId, int portNumber, PortInclude includes) where T:Port
        {
            return await _ctx.Ports
                .Include(includes)
                .OfType<T>()
                .SingleOrDefaultAsync(p => p.NodeId == nodeId && p.PortNumber == portNumber);
        }
    }
}