using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace ValiantServiceCodeGenerator.Converters {
    public sealed class BooleanToVisibilityConverter : IValueConverter {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            var val = System.Convert.ToBoolean(value, CultureInfo.InvariantCulture);

            if (parameter != null) {
                var p = System.Convert.ToBoolean(parameter, CultureInfo.InvariantCulture);
                if (val && p) {
                    return Visibility.Visible;
                } else {
                    return Visibility.Collapsed;
                }
            } else if (val) {
                return Visibility.Visible;
            } else {
                return Visibility.Collapsed;
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
