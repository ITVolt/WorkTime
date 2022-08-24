using System.Windows;

namespace WorkTime.Properties
{
    public sealed record class WindowSettingsDTO
    {
        public Point LastPosition { get; init; }

        public Size LastSize { get; init; }

        public bool LastWasCollapsed { get; init; }
    }
}
