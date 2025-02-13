using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Symmetry.Sales.ChatBot.Core.BusinessAggregate;

namespace Symmetry.Sales.ChatBot.Infrastructure.Data.Config;

public class ContactConfiguration : IEntityTypeConfiguration<Contact>
{
  public void Configure(EntityTypeBuilder<Contact> builder)
  {
    builder.Property(p => p.ContactId).IsRequired();

    builder.Property(p => p.ContactOrigin).IsRequired();
  }
}
