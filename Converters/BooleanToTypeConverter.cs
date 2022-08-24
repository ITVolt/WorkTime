using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

namespace WorkTime.Converters
{
    public abstract class BooleanToTypeConverter<T> : IValueConverter
    {
        public T TrueValue { get; set; }
        public T FalseValue { get; set; }

        protected BooleanToTypeConverter(T trueValue, T falseValue){
            this.TrueValue = trueValue;
            this.FalseValue = falseValue;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is bool boolean && boolean ? TrueValue : FalseValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is T valueOfT && EqualityComparer<T>.Default.Equals(valueOfT, TrueValue);
        }
    }
}
