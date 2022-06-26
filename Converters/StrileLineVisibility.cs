using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace BarhatnieBrovki.Converters
{
    internal class StrileLineVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var hasDiscount = (double?)value;
            if (hasDiscount != null && hasDiscount != 0)
            {
                return TextDecorations.Strikethrough;
            }
            else return new TextDecoration();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var decoration = value as TextDecoration;
            if (decoration != null)
            {
                return decoration.Location == TextDecorationLocation.Strikethrough;
            }
            else
            {
                throw new ArgumentException();
            }
        }


    }
}
