using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

namespace CommunityHub.Ui.Helpers.Converters
{
    public class RatingToStarsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var stars = new List<RatingStar>();
            if (value is int rating && rating >= 1 && rating <= 5)
            {
                for (int i = 1; i <= 5; i++)
                {
                    stars.Add(new RatingStar { IsFilled = i <= rating });
                }
            }
            return stars;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }

    public class RatingStar
    {
        public bool IsFilled { get; set; }
    }
}
