using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Symmetry.Sales.ChatBot.Core.ChatAggregate;

namespace Symmetry.Sales.ChatBot.Infrastructure.Data.Config;

public class ConversationConfiguration : IEntityTypeConfiguration<Conversation>
{
  public void Configure(EntityTypeBuilder<Conversation> builder)
  {
    builder.Property(p => p.IsActive).IsRequired();

    builder
      .HasMany(p => p.Messages)
      .WithOne()
      .HasForeignKey("ConversationId")
      .OnDelete(DeleteBehavior.Cascade);
  }
}
