using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HelloHome.Central.Domain;
using HelloHome.Central.Hub.CmdQrys.Base;
using Microsoft.EntityFrameworkCore;

namespace HelloHome.Central.Hub.CmdQrys
{
    public interface IListRfIdsQuery : IQuery
    {
        Task<IList<int>> ExecuteAsync(CancellationToken cToken);
        Task<IList<int>> ExecuteAsync(byte network, CancellationToken cToken);
        IList<int> Execute();
    }

    public class ListRfIdsQuery : IListRfIdsQuery
    {
        private readonly IUnitOfWork _ctx;

        public ListRfIdsQuery(IUnitOfWork ctx)
        {
            _ctx = ctx;
        }

        public async Task<IList<int>> ExecuteAsync(CancellationToken cToken)
        {
            return await _ctx.Nodes.Select(x => x.RfAddress).ToListAsync(cToken);
        }
        public IList<int> Execute()
        {
            return _ctx.Nodes.Select(x => x.RfAddress).ToList();
        }

        public async Task<IList<int>> ExecuteAsync(byte network, CancellationToken cToken)
        {
            return await _ctx.Nodes
                .Where(x => x.RfAddress == network)
                .Select(x => x.RfAddress)
                .ToListAsync(cToken);
        }
    }
}