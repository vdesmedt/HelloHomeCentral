using HelloHome.Central.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HelloHome.Central.Repository.EntityConfigurations
{
    public class PortConfig : IEntityTypeConfiguration<Port>
    {
        public void Configure(EntityTypeBuilder<Port> builder)
        {
            builder.ToTable("Port");
            builder.Property(x => x.Name).HasMaxLength(50);
            builder.Property(x => x.PortNumber);
            builder.HasDiscriminator<int>("Type")
                .HasValue<NodeHealthSensorPort>(1)
                .HasValue<EnvironmentSensorPort>(2)
                .HasValue<PulseSensorPort>(3)
                .HasValue<PushSensorPort>(4)
                .HasValue<SwitchSensorPort>(5)
                .HasValue<VarioSensorPort>(6)
                .HasValue<RelayActuatorPort>(7)
                .HasValue<FloatDataLogPort>(8)
                .HasValue<IntDataLogPort>(9);
        }
    }
    
    public class SensorPortConfig : IEntityTypeConfiguration<SensorPort>
    {
        public void Configure(EntityTypeBuilder<SensorPort> builder)
        {
            builder.HasBaseType<Port>();
            builder.HasMany(x => x.Triggers).WithOne(x => x.SensorPort);
        }
    }
    
    public class NodeHealthPortConfig : IEntityTypeConfiguration<NodeHealthSensorPort>
    {
        public void Configure(EntityTypeBuilder<NodeHealthSensorPort> builder)
        {
            builder.HasBaseType<SensorPort>();
            builder.HasMany(x => x.History).WithOne(x => x.Port).HasForeignKey(x => x.PortId);
            builder.Property(x => x.UpdateFrequency).HasColumnName("HealtUpdateFreq");
        }
    }
    
    public class EnvironmentPortConfig : IEntityTypeConfiguration<EnvironmentSensorPort>
    {
        public void Configure(EntityTypeBuilder<EnvironmentSensorPort> builder)
        {
            builder.HasBaseType<SensorPort>();
            builder.HasMany(x => x.History).WithOne(x => x.Port).HasForeignKey(x => x.PortId);
            builder.Property(x => x.UpdateFrequency).HasColumnName("EnvUpdateFreq");
        }
    }

    public class PulseSensorPortConfig : IEntityTypeConfiguration<PulseSensorPort>
    {
        public void Configure(EntityTypeBuilder<PulseSensorPort> builder)
        {
            builder.HasBaseType<SensorPort>();
            builder.HasMany(x => x.History).WithOne(x => x.Port).HasForeignKey(x => x.PortId);
        }
    }
    
    public class PushSensorPortConfig : IEntityTypeConfiguration<PushSensorPort>
    {
        public void Configure(EntityTypeBuilder<PushSensorPort> builder)
        {
            builder.HasBaseType<SensorPort>();
            builder.Property(x => x.LastPressStyle).HasColumnName("PortState");

        }
    }
    
    public class SwitchSensorPortConfig : IEntityTypeConfiguration<SwitchSensorPort>
    {
        public void Configure(EntityTypeBuilder<SwitchSensorPort> builder)
        {
            builder.HasBaseType<SensorPort>();
            builder.Property(x => x.SwitchState).HasColumnName("PortState");
        }
    }
    
    public class VarioSensorPortConfig : IEntityTypeConfiguration<VarioSensorPort>
    {
        public void Configure(EntityTypeBuilder<VarioSensorPort> builder)
        {
            builder.HasBaseType<SensorPort>();
            builder.Property(x => x.Level).HasColumnName("PortState");
        }
    }
    
    
   public class ActuatorPortConfig : IEntityTypeConfiguration<ActuatorPort>
   {
       public void Configure(EntityTypeBuilder<ActuatorPort> builder)
       {
           builder.HasBaseType<Port>();
       }
   }
    
    public class RelayActuatorConfig : IEntityTypeConfiguration<RelayActuatorPort>
    {
        public void Configure(EntityTypeBuilder<RelayActuatorPort> builder)
        {
            builder.HasBaseType<ActuatorPort>();
            builder.Property(x => x.RelayState).HasColumnName("PortState");
        }
    }

    public class LoggingPortConfig : IEntityTypeConfiguration<LoggingPort>
    {
        public void Configure(EntityTypeBuilder<LoggingPort> builder)
        {
            builder.HasBaseType<Port>();
        }
    }

    public class FloatDataLogPortConfig : IEntityTypeConfiguration<FloatDataLogPort>
    {
        public void Configure(EntityTypeBuilder<FloatDataLogPort> builder)
        {
            builder.HasBaseType<LoggingPort>();
            builder.Property(x => x.Data).HasColumnName("FloatLogData");
        }
    }
    public class IntDataLogPortConfig : IEntityTypeConfiguration<IntDataLogPort>
    {
        public void Configure(EntityTypeBuilder<IntDataLogPort> builder)
        {
            builder.HasBaseType<LoggingPort>();
            builder.Property(x => x.Data).HasColumnName("IntLogData");
        }
    }
}