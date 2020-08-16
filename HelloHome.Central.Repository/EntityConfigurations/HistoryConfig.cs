using HelloHome.Central.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HelloHome.Central.Repository.EntityConfigurations
{
    public class PortHistoryConfig : IEntityTypeConfiguration<PortHistory>
    {
        public void Configure(EntityTypeBuilder<PortHistory> builder)
        {
            builder.ToTable("PortHistory");
            builder.HasKey(x => x.Id);
            builder.HasDiscriminator<int>("Discr")
                .HasValue<PulseHistory>(1)
                .HasValue<NodeHealthHistory>(2)
                .HasValue<EnvironmentHistory>(3)
                .HasValue<PushButtonHistory>(4)
                .HasValue<SwitchHistory>(5)
                .HasValue<VarioHistory>(6)
                .HasValue<IntLoggerHistory>(7)
                .HasValue<FloatLoggerHistory>(8)
                .HasValue<RelayHistory>(9);
        }
    }
    
    public class PulseHistoryConfig : IEntityTypeConfiguration<PulseHistory>
    {
        public void Configure(EntityTypeBuilder<PulseHistory> builder)
        {
            builder.HasBaseType<PortHistory>();
        }
    }
    
    public class NodeHealthHistoryConfig : IEntityTypeConfiguration<NodeHealthHistory>
    {
        public void Configure(EntityTypeBuilder<NodeHealthHistory> builder)
        {
            builder.HasBaseType<PortHistory>();
        }
    }
    
    public class EnvironmentHistoryConfig : IEntityTypeConfiguration<EnvironmentHistory>
    {
        public void Configure(EntityTypeBuilder<EnvironmentHistory> builder)
        {
            builder.HasBaseType<PortHistory>();
        }
    }
    public class PushButtonHistoryConfig : IEntityTypeConfiguration<PushButtonHistory>
    {
        public void Configure(EntityTypeBuilder<PushButtonHistory> builder)
        {
            builder.HasBaseType<PortHistory>();
            builder.Property(v => v.PressStyle).HasColumnName("NewSensorState");
        }
    }
    public class SwitchPortHistoryConfig : IEntityTypeConfiguration<SwitchHistory>
    {
        public void Configure(EntityTypeBuilder<SwitchHistory> builder)
        {
            builder.HasBaseType<PortHistory>();
            builder.Property(v => v.NewSensorState).HasColumnName("NewSensorState");
        }
    }

    public class VarioButtonPortHistoryConfig : IEntityTypeConfiguration<VarioHistory>
    {
        public void Configure(EntityTypeBuilder<VarioHistory> builder)
        {
            builder.HasBaseType<PortHistory>();
            builder.Property(v => v.NewLevel).HasColumnName("NewSensorState");
        }

        public class IntDataLogPortHistoryConfig : IEntityTypeConfiguration<IntLoggerHistory>
        {
            public void Configure(EntityTypeBuilder<IntLoggerHistory> builder)
            {
                builder.HasBaseType<PortHistory>();
                builder.Property(x => x.Data).HasColumnName("IntLogData");
            }
        }

        public class FloatDataLogPortHistoryConfig : IEntityTypeConfiguration<FloatLoggerHistory>
        {
            public void Configure(EntityTypeBuilder<FloatLoggerHistory> builder)
            {
                builder.HasBaseType<PortHistory>();
                builder.Property(x => x.Data).HasColumnName("FloatLogData");
            }
        }
        public class RelayPortHistoryConfig : IEntityTypeConfiguration<RelayHistory>
        {
            public void Configure(EntityTypeBuilder<RelayHistory> builder)
            {
                builder.HasBaseType<PortHistory>();
                builder.Property(x => x.NewRelayState).HasColumnName("NewSensorState");
            }
        }
    }
}
