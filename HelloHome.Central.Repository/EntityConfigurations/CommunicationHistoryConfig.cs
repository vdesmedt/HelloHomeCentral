using HelloHome.Central.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HelloHome.Central.Repository.EntityConfigurations
{
    public class CommunicationHistoryConfig : IEntityTypeConfiguration<CommunicationHistory>
    {
        public void Configure(EntityTypeBuilder<CommunicationHistory> builder)
        {
            builder.ToTable("CommunicationHistory");
        }
    }
}