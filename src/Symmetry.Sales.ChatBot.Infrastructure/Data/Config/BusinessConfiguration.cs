using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Symmetry.Sales.ChatBot.Core.BusinessAggregate;

namespace Symmetry.Sales.ChatBot.Infrastructure.Data.Config;

internal class BusinessConfiguration : IEntityTypeConfiguration<Business>
{
  public void Configure(EntityTypeBuilder<Business> builder)
  {
    builder.Property(p => p.Name).HasMaxLength(64).IsRequired();

    builder.Property(p => p.Description).IsRequired();

    builder
      .HasMany(p => p.Contacts)
      .WithOne()
      .HasForeignKey("BusinessId")
      .OnDelete(DeleteBehavior.Cascade);
  }
}
