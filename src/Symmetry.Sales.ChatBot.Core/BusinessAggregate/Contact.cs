namespace Symmetry.Sales.ChatBot.Core.BusinessAggregate;

public class Contact
{
  public string ContactId { get; private set; }
  public string Name { get; private set; }
  public Channel ContactOrigin { get; private set; }

  public Contact(string contactId, string name, Channel contactOrigin)
  {
    ContactId = contactId;
    Name = name;
    ContactOrigin = contactOrigin;
  }

  // Required for EF
  private Contact()
  {
    ContactId = string.Empty;
    Name = string.Empty;
    ContactOrigin = Channel.Whatsapp;
  }
}

public enum Channel
{
  Whatsapp,
  Facebook,
}
