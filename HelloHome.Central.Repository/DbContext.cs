using HelloHome.Central.Domain;
using HelloHome.Central.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace HelloHome.Central.Repository
{
    public class HelloHomeContext : DbContext, IUnitOfWork
    {
        public DbSet<Node> Nodes { get; set; }
    }
}
