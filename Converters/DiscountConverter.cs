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
    internal class DiscountConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var hasDiscount = (double?)value;
            if (hasDiscount == null || hasDiscount == 0) //нет скидки
            {
                return Visibility.Collapsed;
            }
            else
            {
                return Visibility.Visible;
            }
               
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (Visibility)value == Visibility.Visible;
        }
    }
}
