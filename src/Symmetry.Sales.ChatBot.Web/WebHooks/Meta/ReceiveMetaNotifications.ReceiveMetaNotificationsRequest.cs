namespace Symmetry.Sales.ChatBot.Web.WebHooks.Meta;

public class ReceiveMetaNotificationsRequest
{
  public static string Route = "/webhook";
  public string Object { get; set; } = string.Empty;
  public IEnumerable<Entry> Entry { get; set; } = [];
}

public class Entry
{
  public string Id { get; set; } = string.Empty;
  public IEnumerable<Change> Changes { get; set; } = [];
}

public class Change
{
  public Value Value { get; set; } = new();
  public string Field { get; set; } = string.Empty;
}

public class Value
{
  public string Messaging_Product { get; set; } = string.Empty;
  public Metadata Metadata { get; set; } = new();
  public IEnumerable<Contact> Contacts { get; set; } = [];
  public IEnumerable<Message> Messages { get; set; } = [];
  public IEnumerable<StatusRequest> Statuses { get; set; } = [];
}

public class Metadata
{
  public string Display_Phone_Number { get; set; } = string.Empty;
  public string Phone_Number_Id { get; set; } = string.Empty;
}

public class Contact
{
  public string Wa_Id { get; set; } = string.Empty;
  public Profile Profile { get; set; } = new();
}

public class Profile
{
  public string Name { get; set; } = string.Empty;
}

public class Message
{
  public string From { get; set; } = string.Empty;
  public string Id { get; set; } = string.Empty;
  public string Timestamp { get; set; } = string.Empty;
  public Text Text { get; set; } = new();
  public string Type { get; set; } = string.Empty;
}

public class Text
{
  public string Body { get; set; } = string.Empty;
}

public class StatusRequest
{
  public string Id { get; set; } = string.Empty;
  public string Status { get; set; } = string.Empty;
  public string Timestamp { get; set; } = string.Empty;
  public string Recipient_Id { get; set; } = string.Empty;
  public Conversation Conversation { get; set; } = new();
  public Pricing Pricing { get; set; } = new();
}

public class Conversation
{
  public string Id { get; set; } = string.Empty;
  public string Expiration_Timestamp { get; set; } = string.Empty;
  public Origin Origin { get; set; } = new();
}

public class Origin
{
  public string Type { get; set; } = string.Empty;
}

public class Pricing
{
  public bool Billable { get; set; }
  public string Pricing_Model { get; set; } = string.Empty;
  public string Category { get; set; } = string.Empty;
}
