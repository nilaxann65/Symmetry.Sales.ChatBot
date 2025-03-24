using Ardalis.Result;
using Ardalis.SharedKernel;
using Symmetry.Sales.ChatBot.Core.BusinessAggregate;

namespace Symmetry.Sales.ChatBot.UseCases.Companies.Payments.AddPaymentMethod;

public record AddPaymentMethodCommand(string name, PaymentMethodType type, string paymentDetails)
  : ICommand<Result>;
