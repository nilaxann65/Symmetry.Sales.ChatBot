using Ardalis.Result;
using Ardalis.SharedKernel;
using MediatR;
using Symmetry.Sales.ChatBot.Core.BusinessAggregate;
using Symmetry.Sales.ChatBot.Core.BusinessAggregate.Specifications;
using Symmetry.Sales.ChatBot.Core.ChatAggregate;
using Symmetry.Sales.ChatBot.Core.ChatAggregate.Specifications;
using Symmetry.Sales.ChatBot.Core.Interfaces;
using Symmetry.Sales.ChatBot.UseCases.Chats.StartChat;
using Symmetry.Sales.ChatBot.UseCases.Messages.Generate;

namespace Symmetry.Sales.ChatBot.UseCases.Chats.AnswerMessage;

internal class AnswerMessageHandler(
  IRepository<Chat> chatRepository,
  IRepository<Business> businessRepository,
  IMediator mediator,
  IMessagingService messagingService
) : ICommandHandler<AnswerMessageCommand, Result<string>>
{
  public async Task<Result<string>> Handle(
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

    var chatExists = await chatRepository.AnyAsync(
      new GetChatByContactIdSpec(request.sender, request.channel, business.Id),
      cancellationToken
    );

    string generatedMessage = string.Empty;
    if (!chatExists)
    {
      var response = await mediator.Send(
        new StartChatCommand(request.userMessage, request.sender, request.channel, business.Id),
        cancellationToken
      );

      if (!response.IsSuccess)
        return response.Map(result => result);

      generatedMessage = response.Value;
    }
    else
    {
      var response = await mediator.Send(
        new GenerateMessageCommand(
          request.userMessage,
          request.sender,
          request.channel,
          business.Id
        ),
        cancellationToken
      );

      if (!response.IsSuccess)
        return response.Map(result => result);

      generatedMessage = response.Value;
    }

    var messageSendResult = await messagingService.SendTextMessageAsync(
      business
        .Contacts.Where(s =>
          s.ContactOrigin == request.channel && s.ContactId == request.destinataryId
        )
        .First()
        .ContactId,
      request.sender,
      generatedMessage,
      false,
      cancellationToken
    );

    if (!messageSendResult.IsSuccess)
      return Result.Error("Error sending generated message");

    return generatedMessage;
  }
}
