using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace BarhatnieBrovki.Converters
{
    internal class MaloVremeniConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var whatTimes = (TimeSpan?)value;
            TimeSpan ts = new TimeSpan(1, 0, 0);
            
            if (whatTimes.Value.TotalDays < 1 && whatTimes <= ts) //меньше часа
            {
                return new SolidColorBrush(Colors.Red);
            }
            else
            {
                return new SolidColorBrush(Colors.Black);
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}