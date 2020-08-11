using HelloHome.Central.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HelloHome.Central.Repository.EntityConfigurations
{
    public class NodeConfig : IEntityTypeConfiguration<Domain.Entities.Node>
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.Node> builder)
        {
            builder.ToTable("Node");
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => x.RfAddress).IsUnique();
            builder.OwnsOne(x => x.Metadata, on =>
            {
                on.WithOwner(y => y.Node);
                on.Property(x => x.Name).HasColumnName("Name").HasMaxLength(50);
                on.Property(x => x.Version).HasColumnName("Version").HasMaxLength(10);
            });
            builder.OwnsOne(x => x.AggregatedData, on =>
            {
                on.WithOwner(y => y.Node);
                on.Ignore(x => x.MaxUpTime);
            });
            builder.HasMany(x => x.Ports).WithOne(x => x.Node).HasForeignKey(x => x.NodeId);
            builder.HasMany(x => x.Logs).WithOne().HasForeignKey(x => x.NodeId);
        }
    }
}
