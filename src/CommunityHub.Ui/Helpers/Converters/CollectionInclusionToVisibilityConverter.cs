using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace CommunityHub.Ui.Helpers.Converters
{
    public class CollectionInclusionToVisibilityConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null || values.Length < 2) return Visibility.Collapsed;
            if (values[0] == null) return Visibility.Collapsed;

            long id;
            try
            {
                id = System.Convert.ToInt64(values[0]);
            }
            catch
            {
                return Visibility.Collapsed;
            }

            if (values[1] is System.Collections.IEnumerable enumerable)
            {
                foreach (var item in enumerable)
                {
                    try
                    {
                        var v = System.Convert.ToInt64(item);
                        if (v == id) return Visibility.Visible;
                    }
                    catch { }
                }
            }

            return Visibility.Collapsed;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
