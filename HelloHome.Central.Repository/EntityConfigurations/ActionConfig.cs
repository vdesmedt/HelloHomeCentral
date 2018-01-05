using HelloHome.Central.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HelloHome.Central.Repository.EntityConfigurations
{
    public class ActionConfig : IEntityTypeConfiguration<Action>
    {
        public void Configure(EntityTypeBuilder<Action> builder)
        {
            builder.ToTable("Action");
            builder.HasDiscriminator<int>("Type")
                .HasValue<ScheduleAction>(1)
                .HasValue<TurnOnAction>(2)
                .HasValue<TurnOffAction>(3);
        }
    }
    
    public class ScheduleActionConfig :  IEntityTypeConfiguration<ScheduleAction>
    {
        public void Configure(EntityTypeBuilder<ScheduleAction> builder)
        {
            builder.HasBaseType<Action>();
            builder.ToTable("Action");            
            builder.HasOne(x => x.ScheduledAction);
        }
    }
    
    public class ActuatorActionConfig : IEntityTypeConfiguration<ActuatorAction>
    {
        public void Configure(EntityTypeBuilder<ActuatorAction> builder)
        {
            builder.HasBaseType<Action>();
            builder.ToTable("Action");
        }
    }
    
    public class RelayActionConfig : IEntityTypeConfiguration<RelayAction>
    {
        public void Configure(EntityTypeBuilder<RelayAction> builder)
        {
            builder.HasBaseType<ActuatorAction>();
            builder.ToTable("Action");
        }
    }

    public class TurnOnActionConfig : IEntityTypeConfiguration<TurnOnAction>
    {
        public void Configure(EntityTypeBuilder<TurnOnAction> builder)
        {
            builder.HasBaseType<RelayAction>();
            builder.ToTable("Action");
        }
    }

    public class TurnOffActionConfig : IEntityTypeConfiguration<TurnOffAction>
    {
        public void Configure(EntityTypeBuilder<TurnOffAction> builder)
        {
            builder.HasBaseType<RelayAction>();
            builder.ToTable("Action");
        }
    }

}