using System.Linq;
using System.Threading.Tasks;
using HelloHome.Central.Domain.CmdQrys.Base;
using HelloHome.Central.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HelloHome.Central.Domain.CmdQrys
{
    public interface IFindPortQuery : IQuery
    {
        Task<Port> ByNodeRfAndPortNumber(int rfAddress, byte portNumber);
    }

    public class FindPortQuery : IFindPortQuery
    {
        private readonly IUnitOfWork _ctx;

        public FindPortQuery(IUnitOfWork ctx)
        {
            _ctx = ctx;
        }

        public async Task<Port> ByNodeRfAndPortNumber(int rfAddress, byte portNumber)
        {
            return await _ctx.Ports
                .SingleOrDefaultAsync(p => p.Node.RfAddress == rfAddress && p.PortNumber == portNumber);
        }
    }
}