using HelloHome.Central.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HelloHome.Central.Repository.EntityConfigurations
{
    public class NodeConfiguration : IEntityTypeConfiguration<Domain.Entities.Node>
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.Node> builder)
        {
            builder.ToTable("Node");
            builder.HasIndex(x => x.RfAddress).IsUnique();
            builder.OwnsOne(x => x.Metadata, nm =>
            {
                nm.Property(mx => mx.Name)
                    .HasColumnName("Name")
                    .HasMaxLength(50);
                nm.Property(mx => mx.Version)
                    .HasColumnName("Version")
                    .HasMaxLength(10);
            });
        }
    }
}
