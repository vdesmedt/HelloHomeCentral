using HelloHome.Central.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HelloHome.Central.Repository.EntityConfigurations
{
    public class NodeHistoryConfig : IEntityTypeConfiguration<NodeHistory>
    {
        public void Configure(EntityTypeBuilder<NodeHistory> builder)
        {
            builder.ToTable("NodeHistory");
            builder.HasOne(x => x.Node)
                .WithMany(x => x.NodeHistory)
                .HasForeignKey(x => x.NodeId);
            builder.HasDiscriminator<int>("Discr")
                .HasValue<PulseHistory>(1)
                .HasValue<NodeHealthHistory>(2)
                .HasValue<EnvironmentDataHistory>(3);
        }
    }
    
    public class PulseHistoryConfig : IEntityTypeConfiguration<PulseHistory>
    {
        public void Configure(EntityTypeBuilder<PulseHistory> builder)
        {
            builder.HasBaseType<NodeHistory>();
            builder.HasOne(x => x.PulseSensorPort)
                .WithMany(x => x.PulseHistory)
                .HasForeignKey(x => x.PulseSensorPortId);
        }
    }
    
    public class NodeHealthHistoryConfig : IEntityTypeConfiguration<NodeHealthHistory>
    {
        public void Configure(EntityTypeBuilder<NodeHealthHistory> builder)
        {
            builder.HasBaseType<NodeHistory>();
        }
    }
    
    public class EnvironmentDataHistoryConfig : IEntityTypeConfiguration<EnvironmentDataHistory>
    {
        public void Configure(EntityTypeBuilder<EnvironmentDataHistory> builder)
        {
            builder.HasBaseType<NodeHistory>();
        }
    }
}
