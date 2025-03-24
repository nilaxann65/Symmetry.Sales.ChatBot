using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Symmetry.Sales.ChatBot.Core.BusinessAggregate;

namespace Symmetry.Sales.ChatBot.Infrastructure.Data.Config;

public class PaymentMethodConfiguration : IEntityTypeConfiguration<PaymentMethod>
{
  public void Configure(EntityTypeBuilder<PaymentMethod> builder)
  {
    builder.HasKey(p => p.Id);
    builder.Property(p => p.Name).IsRequired();
    builder.Property(p => p.IsEnabled).IsRequired();
    builder.Property(p => p.PaymentMethodType).IsRequired();
    builder.Property(p => p.PaymentDetails).IsRequired();
  }
}
