using MudBlazor;

namespace AIIncidentInvestigator;

/// <summary>
/// Restrained, enterprise-dashboard styling - subtle palette, modest border radius,
/// low elevation. Intentionally avoids MudBlazor's default playful purple/heavy shadows
/// since this is meant to look like a real Node Watch feature, not a demo.
/// </summary>
public static class AppTheme
{
    public static MudTheme Default { get; } = new()
    {
        PaletteLight = new PaletteLight
        {
            Primary = "#2C5AA0",
            Secondary = "#5C6B7A",
            AppbarBackground = "#1B3A66",
            AppbarText = "#FFFFFF",
            Background = "#F5F6F8",
            Surface = "#FFFFFF",
            TextPrimary = "#1F2933",
            TextSecondary = "#5C6B7A",
            Success = "#2E7D32",
            Warning = "#B7791F",
            Error = "#C62828",
            Info = "#2C5AA0",
            LinesDefault = "#E1E4E8",
            TableLines = "#E1E4E8",
            Divider = "#E1E4E8",
        },
        LayoutProperties = new LayoutProperties
        {
            DefaultBorderRadius = "6px"
        }
    };
}
