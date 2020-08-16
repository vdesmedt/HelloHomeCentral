using HelloHome.Central.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HelloHome.Central.Repository.EntityConfigurations
{
    public class ScriptConfig : IEntityTypeConfiguration<Script>
    {
        public void Configure(EntityTypeBuilder<Script> builder)
        {
            builder.ToTable("Script");
            builder.Property(s => s.Name).HasMaxLength(50);
            builder.HasOne(x => x.Trigger).WithMany(x => x.Scripts).HasForeignKey(x => x.TriggerId);
        }
    }
    
    public class ScriptActionConfig : IEntityTypeConfiguration<ScriptAction>
    {
        public void Configure(EntityTypeBuilder<ScriptAction> builder)
        {
            builder.ToTable("ScriptAction");
            builder.HasKey(x => new {x.ScriptId, x.ActionId});
            builder.HasOne(x => x.Action).WithMany().HasForeignKey(x => x.ActionId);
            builder.HasOne(x => x.Script).WithMany(x => x.Actions).HasForeignKey(x => x.ScriptId);
        }
    }
    
    public class ScriptConditionConfig : IEntityTypeConfiguration<ScriptCondition>
    {
        public void Configure(EntityTypeBuilder<ScriptCondition> builder)
        {
            builder.ToTable("ScriptCondition");
            builder.HasKey(x => new {x.ScriptId, x.ConditionId});
            builder.HasOne(x => x.Condition).WithMany().HasForeignKey(x => x.ConditionId);
            builder.HasOne(x => x.Script).WithMany(x => x.Conditions).HasForeignKey(x => x.ScriptId);
        }
    }
}