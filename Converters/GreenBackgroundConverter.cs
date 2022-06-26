using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace BarhatnieBrovki.Converters
{
    internal class GreenBackgroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var hasDiscount = (double?)value;
            if (hasDiscount == null || hasDiscount == 0) //нет скидки
            {
                return new SolidColorBrush(Colors.White);
            }
            else
            {               
                return new SolidColorBrush(Colors.LightGreen);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
