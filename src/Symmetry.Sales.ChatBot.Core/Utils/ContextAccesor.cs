using System;

namespace Symmetry.Sales.ChatBot.Core.Utils;

public static class ContextAccesor
{
  private static readonly AsyncLocal<string> _currentDestinataryId = new();
  private static readonly AsyncLocal<int> _currentTenantId = new();

  public static int CurrentTenantId
  {
    get => _currentTenantId.Value;
    set => _currentTenantId.Value = value;
  }

  public static string CurrentDestinataryId
  {
    get => _currentDestinataryId.Value ?? string.Empty;
    set => _currentDestinataryId.Value = value;
  }
}
