using HelloHome.Central.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HelloHome.Central.Repository.EntityConfigurations
{
    public class ConditionConfig : IEntityTypeConfiguration<Condition>
    {
        public void Configure(EntityTypeBuilder<Condition> builder)
        {
            builder.ToTable("Condition");
            builder.HasDiscriminator<string>("Type")
                .HasValue<RelayOnCondition>("RN")
                .HasValue<RelayOffCondition>("RF")
                .HasValue<SwitchOnCondition>("SN")
                .HasValue<SwitchOffCondition>("SF")
                .HasValue<SwitchOffCondition>("TC")
                .HasValue<SwitchOffCondition>("TD");
            builder.Property("Type").HasMaxLength(2).IsUnicode(false);
            builder.Property(c => c.Name).HasMaxLength(50);
        }
    }

    public class RelayOnConditionConfig : IEntityTypeConfiguration<RelayOnCondition>
    {
        public void Configure(EntityTypeBuilder<RelayOnCondition> builder)
        {
            builder.HasBaseType<PortBasedCondition<RelayActuatorPort>>();
            builder.Property(c => c.PortId).HasColumnName("PortId");
            builder.HasOne(c => c.Port)
                .WithMany()
                .HasForeignKey(c => c.PortId)
                .HasConstraintName("FK_Condition_Port_PortId");
        }
    }

    public class RelayOffConditionConfig : IEntityTypeConfiguration<RelayOffCondition>
    {
        public void Configure(EntityTypeBuilder<RelayOffCondition> builder)
        {
            builder.HasBaseType<PortBasedCondition<RelayActuatorPort>>();
            builder.Property(c => c.PortId).HasColumnName("PortId");
            builder.HasOne(c => c.Port)
                .WithMany()
                .HasForeignKey(c => c.PortId)
                .HasConstraintName("FK_Condition_Port_PortId");
        }
    }

    public class SwitchOnConditionConfig : IEntityTypeConfiguration<SwitchOnCondition>
    {
        public void Configure(EntityTypeBuilder<SwitchOnCondition> builder)
        {
            builder.HasBaseType<PortBasedCondition<SwitchSensorPort>>();
            builder.Property(c => c.PortId).HasColumnName("PortId");
            builder.HasOne(c => c.Port)
                .WithMany()
                .HasForeignKey(c => c.PortId)
                .HasConstraintName("FK_Condition_Port_PortId");
        }
    }

    public class SwitchOffConditionConfig : IEntityTypeConfiguration<SwitchOffCondition>
    {
        public void Configure(EntityTypeBuilder<SwitchOffCondition> builder)
        {
            builder.HasBaseType<PortBasedCondition<SwitchSensorPort>>();
            builder.Property(c => c.PortId).HasColumnName("PortId");
            builder.HasOne(c => c.Port)
                .WithMany()
                .HasForeignKey(c => c.PortId)
                .HasConstraintName("FK_Condition_Port_PortId");
        }
    }

    public class TemperatureWithinRangeConditionConfig : IEntityTypeConfiguration<TemperatureWithinRangeCondition>
    {
        public void Configure(EntityTypeBuilder<TemperatureWithinRangeCondition> builder)
        {
            builder.HasBaseType<PortBasedCondition<EnvironmentSensorPort>>();
            builder.Property(c => c.PortId).HasColumnName("PortId");
            builder.HasOne(c => c.Port)
                .WithMany()
                .HasForeignKey(c => c.PortId)
                .HasConstraintName("FK_Condition_Port_PortId");
        }
    }

    public class TimeOfTheDayConditionConfig : IEntityTypeConfiguration<TimeOfTheDayCondition>
    {
        public void Configure(EntityTypeBuilder<TimeOfTheDayCondition> builder)
        {
            builder.HasBaseType<Condition>();
        }
    }
}