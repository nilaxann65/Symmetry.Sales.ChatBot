using Ardalis.SharedKernel;

namespace Symmetry.Sales.ChatBot.Core.BusinessAggregate;

public class PaymentMethod : EntityBase
{
  public string Name { get; set; }
  public bool IsEnabled { get; set; }
  public PaymentMethodType PaymentMethodType { get; set; }
  public string PaymentDetails { get; set; }

  public PaymentMethod(string name, PaymentMethodType paymentMethodType, string paymentDetails)
  {
    Name = name;
    PaymentMethodType = paymentMethodType;
    PaymentDetails = paymentDetails;
    IsEnabled = true;
  }

  private PaymentMethod()
  {
    // required by EF
    Name = string.Empty;
    PaymentDetails = string.Empty;
  }
}

public enum PaymentMethodType
{
  PayPal,
  BankTransfer,
  QRCode,
  //Cash, not supported yet
}
