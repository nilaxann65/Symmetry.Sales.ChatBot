using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Symmetry.Sales.ChatBot.Core.ChatAggregate;

namespace Symmetry.Sales.ChatBot.Infrastructure.Data.Config;

public class ChatConfiguration : IEntityTypeConfiguration<Chat>
{
  public void Configure(EntityTypeBuilder<Chat> builder)
  {
    builder.HasKey(p => p.Id);

    builder.Property(p => p.TenantId).IsRequired();

    builder.Property(p => p.Origin).IsRequired();

    builder.Property(p => p.ContactId).HasMaxLength(64).IsRequired();

    builder
      .HasMany(p => p.Conversations)
      .WithOne()
      .HasForeignKey("ChatId")
      .OnDelete(DeleteBehavior.Cascade);
  }
}
