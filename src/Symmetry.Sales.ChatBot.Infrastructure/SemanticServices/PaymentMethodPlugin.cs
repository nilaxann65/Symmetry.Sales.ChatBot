using System;
using System.ComponentModel;
using Microsoft.SemanticKernel;
using Symmetry.Sales.ChatBot.Core.BusinessAggregate;

namespace Symmetry.Sales.ChatBot.Infrastructure.SemanticServices;

[Description("Representa los métodos de pago")]
public class PaymentMethodPlugin
{
  [KernelFunction("get_payment_methods")]
  [Description("Get the available payment methods")]
  public PaymentMethod GetPaymentMethods()
  {
    return new PaymentMethod(
      "PayPal",
      PaymentMethodType.PayPal,
      "Cuenta: 77272888373 Destinatario: JOSE ARMANDO VARGAS AGUANTA Banco: Bancosol"
    );
    
  }
}
