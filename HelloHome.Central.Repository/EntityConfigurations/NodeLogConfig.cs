using HelloHome.Central.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HelloHome.Central.Repository.EntityConfigurations
{
    public class NodeLogConfig : IEntityTypeConfiguration<NodeLog>
    {
        public void Configure(EntityTypeBuilder<NodeLog> builder)
        {
            builder.ToTable("nodelog");
            builder.Property(x => x.Type).HasMaxLength(10).IsRequired();
            builder.Property(x => x.Data).HasMaxLength(255);
        }
    }
}