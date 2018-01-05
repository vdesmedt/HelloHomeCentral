using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HelloHome.Central.Domain;
using Microsoft.EntityFrameworkCore;

namespace HelloHome.Central.Hub.Queries
{
    public interface IListRfIdsQuery : IQuery
    {
        Task<IList<byte>> ExecuteAsync();
        Task<IList<byte>> ExecuteAsync(byte network, CancellationToken cToken);
        IList<byte> Execute();
    }

    public class ListRfIdsQuery : IListRfIdsQuery
    {
        private readonly IUnitOfWork _ctx;

        public ListRfIdsQuery(IUnitOfWork ctx)
        {
            _ctx = ctx;
        }

        public async Task<IList<byte>> ExecuteAsync()
        {
            return await _ctx.Nodes.Select(x => (byte)x.RfAddress).ToListAsync();
        }
        public IList<byte> Execute()
        {
            return _ctx.Nodes.Select(x => (byte)x.RfAddress).ToList();
        }

        public async Task<IList<byte>> ExecuteAsync(byte network, CancellationToken cToken)
        {
            return await _ctx.Nodes
                .Where(x => x.RfAddress == network)
                .Select(x => (byte)x.RfAddress)
                .ToListAsync(cToken);
        }
    }
}