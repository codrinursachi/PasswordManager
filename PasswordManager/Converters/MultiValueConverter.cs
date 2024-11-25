using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace PasswordManager.Converters
{

    public class MultiValueConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool isSelected = (bool)values[0];
            bool isExpanded = (bool)values[1];
            return isSelected && isExpanded;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            bool isChecked = (bool)value;
            return [isChecked, isChecked];
        }
    }
}
