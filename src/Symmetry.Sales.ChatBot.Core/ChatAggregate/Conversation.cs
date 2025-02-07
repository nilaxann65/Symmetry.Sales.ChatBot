namespace Symmetry.Sales.ChatBot.Core.ChatAggregate;

public class Conversation
{
  public List<Message> Messages { get; private set; } = [];
  public bool IsActive { get; private set; } = true;

  public Conversation(string userMessage)
  {
    string systemInstructions =
      "Actuaras como un agente de ventas, por favor, sigue las instrucciones del sistema. \n se conciso y directo con tus respuestas, si no sabes la respuesta, o si solicitan informacion que no tienes, ponte en contacto con el supervisor. Toda la conversacion sera en español.";

    Messages.AddRange(
      [
        new Message(systemInstructions, MessageSender.System),
        new Message(userMessage, MessageSender.User)
      ]
    );
  }

  private Conversation() { }

  public void AddBotMessage(string content) =>
    Messages.Add(new Message(content, MessageSender.Bot));

  public void AddUserMessage(string content) =>
    Messages.Add(new Message(content, MessageSender.User));

  public void CloseConversation() => IsActive = false;
}
