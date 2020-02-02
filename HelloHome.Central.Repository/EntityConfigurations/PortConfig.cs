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
                .HasValue<PulseSensorPort>(1)
                .HasValue<PushSensorPort>(2)
                .HasValue<SwitchSensorPort>(3)
                .HasValue<VarioSensorPort>(4)
                .HasValue<RelayActuatorPort>(5);
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
    
    public class PulseSensorPortConfig : IEntityTypeConfiguration<PulseSensorPort>
    {
        public void Configure(EntityTypeBuilder<PulseSensorPort> builder)
        {
            builder.HasBaseType<SensorPort>();
            builder.HasMany(x => x.PulseHistory).WithOne(x => x.PulseSensorPort).HasForeignKey(x => x.PulseSensorPortId);
        }
    }
    
    public class PushSensorPortConfig : IEntityTypeConfiguration<PushSensorPort>
    {
        public void Configure(EntityTypeBuilder<PushSensorPort> builder)
        {
            builder.HasBaseType<SensorPort>();
        }
    }
    
    public class SwitchSensorPortConfig : IEntityTypeConfiguration<SwitchSensorPort>
    {
        public void Configure(EntityTypeBuilder<SwitchSensorPort> builder)
        {
            builder.HasBaseType<SensorPort>();
        }
    }
    
    public class VarioSensorPortConfig : IEntityTypeConfiguration<VarioSensorPort>
    {
        public void Configure(EntityTypeBuilder<VarioSensorPort> builder)
        {
            builder.HasBaseType<SensorPort>();
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
        }
    }
}