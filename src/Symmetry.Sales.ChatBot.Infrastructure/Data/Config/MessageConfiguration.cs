using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Symmetry.Sales.ChatBot.Core.ChatAggregate;

namespace Symmetry.Sales.ChatBot.Infrastructure.Data.Config;

public class MessageConfiguration : IEntityTypeConfiguration<Message>
{
  public void Configure(EntityTypeBuilder<Message> builder)
  {
    builder.HasKey(p => p.Id);
    builder.Property(p => p.Date).IsRequired().HasColumnType("timestamp");
    builder.Property(p => p.Content).IsRequired();
    builder.Property(p => p.Sender).IsRequired();
  }
}
