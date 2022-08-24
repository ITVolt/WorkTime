using System.Windows;

namespace WorkTime.Properties
{
    public sealed record class WindowSettingsDTO
    {
        public Point LastPosition { get; init; }

        public Size LastSize { get; init; }

        public void Deconstruct(out Point lastPoint, out Size lastSize)
        {
            lastPoint = LastPosition;
            lastSize = LastSize;
        }

    }
}
