using Ardalis.Result;
using Ardalis.SharedKernel;

namespace Symmetry.Sales.ChatBot.Core.BusinessAggregate;

public class Business : EntityBase, IAggregateRoot
{
  public string Name { get; set; }
  public string Description { get; set; }
  public List<Contact> Contacts { get; private set; } = [];
  public List<IProduct> Products { get; private set; } = [];

  public Business(string name, string? description)
  {
    Name = name;
    Description = description ?? string.Empty;
  }

  public Result AddContact(string contactId, string name, Channel contactOrigin)
  {
    if (Contacts.Any(c => c.ContactId == contactId))
      return Result.Conflict("Contact already exists.");

    if (Contacts.Any(c => c.ContactOrigin == contactOrigin))
      return Result.Conflict($"Contact from {nameof(contactOrigin)}  already exists.");

    Contacts.Add(new Contact(contactId, name, contactOrigin));

    return Result.Success();
  }

  // Required for EF
  private Business()
  {
    Name = string.Empty;
    Description = string.Empty;
  }
}
