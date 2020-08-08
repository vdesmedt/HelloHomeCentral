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
}