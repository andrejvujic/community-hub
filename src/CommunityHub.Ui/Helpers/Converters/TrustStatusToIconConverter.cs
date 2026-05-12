using System;
using System.Globalization;
using System.Windows.Data;
using System.Collections.Generic;
using CommunityHub.Application.Domain.TrustRecords;

namespace CommunityHub.Ui.Helpers.Converters
{
    public class TrustStatusToIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var stars = new List<TrustLevelIconStar>();
            if (value is TrustStatus status)
            {
                int count = 0;
                TrustStarType type = TrustStarType.Empty;
                switch (status)
                {
                    case TrustStatus.New:
                        count = 1; type = TrustStarType.Empty; break;
                    case TrustStatus.Inactive:
                        count = 1; type = TrustStarType.Black; break;
                    case TrustStatus.Active:
                        count = 1; type = TrustStarType.Yellow; break;
                    case TrustStatus.Prominent:
                        count = 2; type = TrustStarType.Yellow; break;
                    case TrustStatus.Trustworthy:
                        count = 3; type = TrustStarType.Yellow; break;
                }
                for (int i = 0; i < count; i++)
                    stars.Add(new TrustLevelIconStar { StarType = type });
            }
            return stars;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public enum TrustStarType { Empty, Black, Yellow }

    public class TrustLevelIconStar
    {
        public TrustStarType StarType { get; set; }
    }
}
