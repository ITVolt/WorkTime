using System.Windows;

namespace WorkTime.Converters
{
    public class BooleanToVisibilityConverter : BooleanToTypeConverter<Visibility>
    {
        public BooleanToVisibilityConverter() : base(Visibility.Visible, Visibility.Hidden) { }
    }
}
