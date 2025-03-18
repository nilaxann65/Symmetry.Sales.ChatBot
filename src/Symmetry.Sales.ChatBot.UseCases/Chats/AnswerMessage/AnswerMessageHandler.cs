using System.Transactions;
using Ardalis.Result;
using Ardalis.SharedKernel;
using MediatR;
using Symmetry.Sales.ChatBot.Core.BusinessAggregate;
using Symmetry.Sales.ChatBot.Core.BusinessAggregate.Specifications;
using Symmetry.Sales.ChatBot.Core.ChatAggregate;
using Symmetry.Sales.ChatBot.Core.ChatAggregate.Specifications;
using Symmetry.Sales.ChatBot.Core.Interfaces;
using Symmetry.Sales.ChatBot.Core.Utils;
using Symmetry.Sales.ChatBot.UseCases.Chats.StartChat;
using Symmetry.Sales.ChatBot.UseCases.Messages.Generate;

namespace Symmetry.Sales.ChatBot.UseCases.Chats.AnswerMessage;

internal class AnswerMessageHandler(
  IRepository<Chat> chatRepository,
  IRepository<Business> businessRepository,
  IMediator mediator,
  IMessagingService messagingService
) : ICommandHandler<AnswerMessageCommand, Result>
{
  public async Task<Result> Handle(
    AnswerMessageCommand request,
    CancellationToken cancellationToken
  )
  {
    var business = await businessRepository.FirstOrDefaultAsync(
      new GetByContactIdSpec(request.destinataryId, request.channel),
      cancellationToken
    );

    if (business is null)
      return Result.NotFound("Business not found");

    ContextAccesor.CurrentTenantId = business.Id;

    var chat = await chatRepository.FirstOrDefaultAsync(
      new GetChatByContactIdSpec(request.sender, request.channel),
      cancellationToken
    );

    if (chat is null)
    {
      var response = await mediator.Send(
        new StartChatCommand(request.userMessage, request.sender, request.channel, business.Id),
        cancellationToken
      );

      if (!response.IsSuccess)
        return response.Map(result => Result.Error(response.Errors.First()));

      try
      {
        chat = response.Value;
        await chatRepository.AddAsync(chat, cancellationToken);
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
        throw;
      }
    }
    else
    {
      if (chat.HasActiveConversation())
        chat.AddUserMessage(request.userMessage);
      else
        chat.InitConversation(request.userMessage);

      var response = await mediator.Send(new GenerateMessageCommand(chat), cancellationToken);

      if (!response.IsSuccess)
        return response.Map(result => Result.Error(response.Errors.First()));

      chat.AddBotMessage(response.Value);
    }

    var messageSendResult = await messagingService.SendTextMessageAsync(
      business
        .Contacts.Where(s =>
          s.ContactOrigin == request.channel && s.ContactId == request.destinataryId
        )
        .First()
        .ContactId,
      request.sender,
      chat.GetBotGeneratedMessage(),
      false,
      cancellationToken
    );

    if (!messageSendResult.IsSuccess)
      return Result.Error("Error sending generated message");

    if (chat.Id == 0)
      await chatRepository.AddAsync(chat, cancellationToken);
    else
      await chatRepository.SaveChangesAsync(cancellationToken);

    return Result.Success();
  }
}
