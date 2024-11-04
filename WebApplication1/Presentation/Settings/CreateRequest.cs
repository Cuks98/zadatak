namespace WebApplication1.Presentation.Settings;

internal sealed record CreateRequest(
    string Value,
    DateTime ValidFrom);