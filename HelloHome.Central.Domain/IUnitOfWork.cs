using HelloHome.Central.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HelloHome.Central.Domain
{
    public interface IUnitOfWork
    {
        DbSet<Node> Nodes { get; set; }
        DbSet<PortHistory> PortHistory { get; set; }
        DbSet<Port> Ports { get; set; }
        DbSet<Trigger> Triggers { get; set; }
        DbSet<NodeLog> NodeLogs { get; set; }
        DbSet<Script> Scripts { get; set; }
        DbSet<Condition> Conditions { get; set; }
        int Commit();
        Task<int> CommitAsync();
    }
}
