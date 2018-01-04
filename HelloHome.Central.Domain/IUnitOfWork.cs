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
        DbSet<Trigger> Triggers { get; set; }

        int SaveChanges();
        Task CommitAsync();
    }
}
