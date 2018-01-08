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
            builder.HasOne(x => x.Metadata).WithOne(x => x.Node).HasForeignKey<NodeMetadata>(x => x.Id);
            builder.HasOne(x => x.AggregatedData).WithOne(x => x.Node).HasForeignKey<NodeAggregatedData>(x => x.Id);
            builder.HasMany(x => x.Ports).WithOne(x => x.Node).HasForeignKey(x => x.NodeId);
            builder.HasMany(x => x.Logs).WithOne().HasForeignKey(x => x.NodeId);
            builder.HasMany(x => x.NodeHistory).WithOne(x=>x.Node).HasForeignKey(x => x.NodeId);
        }
    }

    public class NodeMetaConfig : IEntityTypeConfiguration<Domain.Entities.NodeMetadata>
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.NodeMetadata> builder)
        {
            builder.ToTable("Node");
            builder.Property(x => x.Name).HasColumnName("Name").HasMaxLength(50);
            builder.Property(x => x.Version).HasColumnName("Version").HasMaxLength(10);
        }
    }
    
    public class NodeAggregatedDataConfig : IEntityTypeConfiguration<NodeAggregatedData>
    {
        public void Configure(EntityTypeBuilder<NodeAggregatedData> builder)
        {
            builder.ToTable("Node");
            builder.Ignore(x => x.MaxUpTime);
        }
    }
}
