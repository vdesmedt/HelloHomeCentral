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
            builder.HasIndex(x => x.RfAddress).IsUnique();
            builder.HasOne(x => x.Metadata).WithOne(x => x.Node).HasForeignKey<NodeMetadata>(x => x.Id);
        }
    }

    public class NodeMetaConfig : IEntityTypeConfiguration<NodeMetadata>
    {
        public void Configure(EntityTypeBuilder<NodeMetadata> builder)
        {
            builder.ToTable("Node");
            builder.Property(x => x.Name).HasColumnName("Name").HasMaxLength(50);
            builder.Property(x => x.Version).HasColumnName("Version").HasMaxLength(10);
        }
    }
}
