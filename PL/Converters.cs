using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace PL;
class ConvertIdToContent : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (int)value == 0 ? "Add" : "Update";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }

    internal class ConvertStatusToColor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string status = (string)value;
            switch (status)
            {
                case "UNSCHEDULED":
                    return Brushes.White;
                case "SCHEDULED":
                    return Brushes.Beige;
                case "STARTED":
                    return Brushes.Orange;
                case "DONE":
                    return Brushes.Green;
                default:
                    return Brushes.White;
            }
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    internal class ConvertStatusToForground : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string status = (string)value;
            switch (status)
            {
                case "UNSCHEDULED":
                    return Brushes.White;
                case "SCHEDULED":
                    return Brushes.Beige;
                case "STARTED":
                    return Brushes.Orange;
                case "DONE":
                    return Brushes.Green;
                default:
                    return Brushes.Black;
            }
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
