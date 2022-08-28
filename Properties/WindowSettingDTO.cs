using System.Windows;

namespace WorkTime.Properties
{
    public sealed record class WindowSettingsDTO(Point LastPosition, Point LastCollapsedPosition, Size LastSize, bool LastWasCollapsed);
}
