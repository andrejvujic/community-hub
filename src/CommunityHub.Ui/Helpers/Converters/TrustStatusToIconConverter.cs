using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using CommunityHub.Application.Domain;

namespace CommunityHub.Ui.Helpers.Converters
{
    public class TrustStatusToIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is TrustStatus status)
            {
                switch (status)
                {
                    case TrustStatus.New:
                        return new TrustLevelIcon { StarCount = 1, StarType = TrustStarType.Empty };
                    case TrustStatus.Inactive:
                        return new TrustLevelIcon { StarCount = 1, StarType = TrustStarType.Black };
                    case TrustStatus.Active:
                        return new TrustLevelIcon { StarCount = 1, StarType = TrustStarType.Yellow };
                    case TrustStatus.Prominent:
                        return new TrustLevelIcon { StarCount = 2, StarType = TrustStarType.Yellow };
                    case TrustStatus.Trustworthy:
                        return new TrustLevelIcon { StarCount = 3, StarType = TrustStarType.Yellow };
                }
            }
            return new TrustLevelIcon { StarCount = 0, StarType = TrustStarType.Empty };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public enum TrustStarType { Empty, Black, Yellow }

    public class TrustLevelIcon
    {
        public int StarCount { get; set; }
        public TrustStarType StarType { get; set; }
    }
}
