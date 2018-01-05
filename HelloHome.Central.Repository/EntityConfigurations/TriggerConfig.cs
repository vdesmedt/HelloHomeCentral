using HelloHome.Central.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HelloHome.Central.Repository.EntityConfigurations
{
    public class TriggerConfig : IEntityTypeConfiguration<Trigger>
    {
        public void Configure(EntityTypeBuilder<Trigger> builder)
        {
            builder.ToTable("Trigger");
            builder.HasDiscriminator<int>("Type")
                .HasValue<CronTrigger>(1)
                .HasValue<PushTrigger>(2)
                .HasValue<SwitchTrigger>(3)
                .HasValue<VarioTrigger>(4);
        }
    }
    
    public class CronTriggerConfig : IEntityTypeConfiguration<CronTrigger>
    {
        public void Configure(EntityTypeBuilder<CronTrigger> builder)
        {
            builder.ToTable("Trigger");
            builder.Property(x => x.CronExpression).HasMaxLength(20);
        }
    }
    
    public class SensorTriggerConfig : IEntityTypeConfiguration<SensorTrigger>
    {
        public void Configure(EntityTypeBuilder<SensorTrigger> builder)
        {
            builder.HasBaseType<Trigger>();
            builder.ToTable("Trigger");
            builder.HasOne(x => x.SensorPort).WithMany(x => x.Triggers);
        }
    }
    
    public class PushTriggerConfig : IEntityTypeConfiguration<PushTrigger>
    {
        public void Configure(EntityTypeBuilder<PushTrigger> builder)
        {
            builder.HasBaseType<SensorTrigger>();
            builder.ToTable("Trigger");
        }
    }
}