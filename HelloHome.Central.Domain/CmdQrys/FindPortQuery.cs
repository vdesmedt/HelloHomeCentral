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
        Task<Port> ByNodeRfAndPortNumber(int rfAddress, byte portNumber, PortInclude includes = PortInclude.None);
    }

    public class FindPortQuery : IFindPortQuery
    {
        private readonly IUnitOfWork _ctx;

        public FindPortQuery(IUnitOfWork ctx)
        {
            _ctx = ctx;
        }

        public async Task<Port> ByNodeRfAndPortNumber(int rfAddress, byte portNumber, PortInclude includes)
        {
            return await _ctx.Ports
                .Include(includes)
                .SingleOrDefaultAsync(p => p.Node.RfAddress == rfAddress && p.PortNumber == portNumber);
        }
    }
}