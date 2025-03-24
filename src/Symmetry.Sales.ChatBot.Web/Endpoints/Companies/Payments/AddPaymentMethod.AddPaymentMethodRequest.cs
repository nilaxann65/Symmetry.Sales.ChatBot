using System;
using Symmetry.Sales.ChatBot.Core.BusinessAggregate;

namespace Symmetry.Sales.ChatBot.Web.Endpoints.Companies.Payments;

public class AddPaymentMethodRequest
{
  public static string Route = "Business/Payments/Execute";
  public string Name { get; set; } = string.Empty;
  public PaymentMethodType Type { get; set; }
  public string PaymentDetails { get; set; } = string.Empty;
}
