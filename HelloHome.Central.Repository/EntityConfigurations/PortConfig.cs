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
                .HasValue<NodeHealthSensor>(1)
                .HasValue<EnvironmentSensor>(2)
                .HasValue<PulseSensor>(3)
                .HasValue<PushButtonSensor>(4)
                .HasValue<SwitchSensor>(5)
                .HasValue<VarioSensor>(6)
                .HasValue<RelayActuator>(7)
                .HasValue<FloatLogger>(8)
                .HasValue<IntLogger>(9);
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
    
    public class NodeHealthPortConfig : IEntityTypeConfiguration<NodeHealthSensor>
    {
        public void Configure(EntityTypeBuilder<NodeHealthSensor> builder)
        {
            builder.HasBaseType<SensorPort>();
            builder.HasMany(x => x.History)
                .WithOne(x => x.Port)
                .HasForeignKey(x => x.PortId)
                .HasConstraintName("FK_Port_PortHistory_PortId");
            builder.Property(x => x.UpdateFrequency).HasColumnName("HealtUpdateFreq");
        }
    }
    
    public class EnvironmentPortConfig : IEntityTypeConfiguration<EnvironmentSensor>
    {
        public void Configure(EntityTypeBuilder<EnvironmentSensor> builder)
        {
            builder.HasBaseType<SensorPort>();
            builder.HasMany(x => x.History)
                .WithOne(x => x.Port)
                .HasForeignKey(x => x.PortId)
                .HasConstraintName("FK_Port_PortHistory_PortId");
            builder.Property(x => x.UpdateFrequency).HasColumnName("EnvUpdateFreq");
        }
    }

    public class PulseSensorPortConfig : IEntityTypeConfiguration<PulseSensor>
    {
        public void Configure(EntityTypeBuilder<PulseSensor> builder)
        {
            builder.HasBaseType<SensorPort>();
            builder.HasMany(x => x.History)
                .WithOne(x => x.Port)
                .HasForeignKey(x => x.PortId)
                .HasConstraintName("FK_Port_PortHistory_PortId");
        }
    }
    
    public class PushSensorPortConfig : IEntityTypeConfiguration<PushButtonSensor>
    {
        public void Configure(EntityTypeBuilder<PushButtonSensor> builder)
        {
            builder.HasBaseType<SensorPort>();
            builder.HasMany(x => x.History)
                .WithOne(x => x.Port)
                .HasForeignKey(x => x.PortId)
                .HasConstraintName("FK_Port_PortHistory_PortId");
            builder.Property(x => x.LastPressStyle).HasColumnName("PortState");

        }
    }
    
    public class SwitchSensorPortConfig : IEntityTypeConfiguration<SwitchSensor>
    {
        public void Configure(EntityTypeBuilder<SwitchSensor> builder)
        {
            builder.HasBaseType<SensorPort>();
            builder.HasMany(x => x.History)
                .WithOne(x => x.Port)
                .HasForeignKey(x => x.PortId)
                .HasConstraintName("FK_Port_PortHistory_PortId");
            builder.Property(x => x.SwitchState).HasColumnName("PortState");
        }
    }
    
    public class VarioSensorPortConfig : IEntityTypeConfiguration<VarioSensor>
    {
        public void Configure(EntityTypeBuilder<VarioSensor> builder)
        {
            builder.HasBaseType<SensorPort>();
            builder.HasMany(x => x.History)
                .WithOne(x => x.Port)
                .HasForeignKey(x => x.PortId)
                .HasConstraintName("FK_Port_PortHistory_PortId");
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
    
    public class RelayActuatorConfig : IEntityTypeConfiguration<RelayActuator>
    {
        public void Configure(EntityTypeBuilder<RelayActuator> builder)
        {
            builder.HasBaseType<ActuatorPort>();
            builder.HasMany(x => x.History)
                .WithOne(x => x.Port)
                .HasForeignKey(x => x.PortId)
                .HasConstraintName("FK_Port_PortHistory_PortId");
            builder.Property(x => x.RelayState).HasColumnName("PortState");
        }
    }

    public class LoggingPortConfig : IEntityTypeConfiguration<LoggerPort>
    {
        public void Configure(EntityTypeBuilder<LoggerPort> builder)
        {
            builder.HasBaseType<Port>();
        }
    }

    public class FloatDataLogPortConfig : IEntityTypeConfiguration<FloatLogger>
    {
        public void Configure(EntityTypeBuilder<FloatLogger> builder)
        {
            builder.HasBaseType<LoggerPort>();
            builder.HasMany(x => x.History)
                .WithOne(x => x.Port)
                .HasForeignKey(x => x.PortId)
                .HasConstraintName("FK_Port_PortHistory_PortId");
            builder.Property(x => x.Data).HasColumnName("FloatLogData");
        }
    }
    public class IntDataLogPortConfig : IEntityTypeConfiguration<IntLogger>
    {
        public void Configure(EntityTypeBuilder<IntLogger> builder)
        {
            builder.HasBaseType<LoggerPort>();
            builder.HasMany(x => x.History)
                .WithOne(x => x.Port)
                .HasForeignKey(x => x.PortId)
                .HasConstraintName("FK_Port_PortHistory_PortId");
            builder.Property(x => x.Data).HasColumnName("IntLogData");
        }
    }
}