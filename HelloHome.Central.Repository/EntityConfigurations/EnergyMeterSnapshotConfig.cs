using HelloHome.Central.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HelloHome.Central.Repository.EntityConfigurations;

public class EnergyMeterSnapshotConfig : IEntityTypeConfiguration<EnergyMeterSnapshot>
{
    public void Configure(EntityTypeBuilder<EnergyMeterSnapshot> builder)
    {
        builder.ToTable("EnergyMeterSnapshot");
        builder.HasKey(x => x.Id);
        builder.HasDiscriminator<int>("Discr")
            .HasValue<PulseEnergyMeterSnapshot>(1);
    }
}

public class PulseEnergyMeterSnapshotConfig : IEntityTypeConfiguration<PulseEnergyMeterSnapshot>
{
    public void Configure(EntityTypeBuilder<PulseEnergyMeterSnapshot> builder)
    {
        builder.HasBaseType<EnergyMeterSnapshot>();
    }
}
