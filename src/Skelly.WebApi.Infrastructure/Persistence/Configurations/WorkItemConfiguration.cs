using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Skelly.WebApi.Domain.WorkItemAggregate;

namespace Skelly.WebApi.Infrastructure.Persistence.Configurations;

public class WorkItemConfiguration : IEntityTypeConfiguration<WorkItem>
{
    public void Configure(EntityTypeBuilder<WorkItem> builder)
    {
        builder.ToTable(nameof(WorkItem));

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Title)
            .IsRequired();
    }
}
