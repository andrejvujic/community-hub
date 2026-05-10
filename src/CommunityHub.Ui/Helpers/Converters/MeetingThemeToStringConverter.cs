using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;
using CommunityHub.Application.Domain.NeighborhoodMeetings;

namespace CommunityHub.Ui.Helpers.Converters
{
    public class MeetingThemeToStringConverter : IValueConverter
    {
        private readonly string _welcomeText = "Dobrodošlica";
        private readonly string _motivationText = "Motivacija";

        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not MeetingTheme theme) return null;

            switch (theme)
            {
                case MeetingTheme.Welcome:
                    return _welcomeText;
                case MeetingTheme.Motivation:
                    return _motivationText;
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
