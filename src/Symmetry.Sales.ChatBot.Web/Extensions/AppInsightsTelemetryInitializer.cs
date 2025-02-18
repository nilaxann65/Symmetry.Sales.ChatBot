using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;

namespace Symmetry.Sales.ChatBot.Web.Extensions;

public class AppInsightsTelemetryInitializer : ITelemetryInitializer
{
  public void Initialize(ITelemetry telemetry)
  {
    telemetry.Context.Cloud.RoleName = "Symmetry-Sales";
  }
}
