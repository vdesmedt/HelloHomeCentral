﻿using HelloHome.Central.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HelloHome.Central.Repository.EntityConfigurations
{
    public class ActionConfig : IEntityTypeConfiguration<Action>
    {
        public void Configure(EntityTypeBuilder<Action> builder)
        {
            builder.ToTable("Action");
            builder.HasDiscriminator<string>("Type")
                .HasValue<ScheduleAction>("SC")
                .HasValue<ActuatorAction>("AA")
                .HasValue<RelayAction>("RA")
                .HasValue<TurnOnAction>("TN")
                .HasValue<TurnOffAction>("TF");
            builder.Property("Type").HasMaxLength(2).IsUnicode(false);
            builder.HasOne(x => x.Trigger).WithMany(x=>x.Actions).HasForeignKey(x => x.TriggerId).IsRequired(false);
        }
    }
    
    public class ScheduleActionConfig :  IEntityTypeConfiguration<ScheduleAction>
    {
        public void Configure(EntityTypeBuilder<ScheduleAction> builder)
        {
            builder.HasBaseType<Action>();
            builder.HasOne(x => x.ScheduledAction);
        }
    }
    
    public class ActuatorActionConfig : IEntityTypeConfiguration<ActuatorAction>
    {
        public void Configure(EntityTypeBuilder<ActuatorAction> builder)
        {
            builder.HasBaseType<Action>();
        }
    }
    
    public class RelayActionConfig : IEntityTypeConfiguration<RelayAction>
    {
        public void Configure(EntityTypeBuilder<RelayAction> builder)
        {
            builder.HasBaseType<ActuatorAction>();
        }
    }

    public class TurnOnActionConfig : IEntityTypeConfiguration<TurnOnAction>
    {
        public void Configure(EntityTypeBuilder<TurnOnAction> builder)
        {
            builder.HasBaseType<RelayAction>();
        }
    }

    public class TurnOffActionConfig : IEntityTypeConfiguration<TurnOffAction>
    {
        public void Configure(EntityTypeBuilder<TurnOffAction> builder)
        {
            builder.HasBaseType<RelayAction>();
        }
    }

}