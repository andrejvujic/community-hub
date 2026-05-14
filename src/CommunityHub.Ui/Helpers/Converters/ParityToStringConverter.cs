using CommunityHub.Application.Domain.Addresses;
using System;
using System.Globalization;
using System.Windows.Data;

namespace CommunityHub.Ui.Helpers.Converters
{
    public class ParityToStringConverter : IValueConverter
    {
        private const string _evenText = "Parni brojevi";
        private const string _oddText = "Neparni brojevi";
        private const string _bothText = "Svi brojevi";

        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not ParityType parity)
                return null;

            switch (parity)
            {
                case ParityType.Even:
                    return _evenText;
                case ParityType.Odd:
                    return _oddText;
                case ParityType.Both:
                    return _bothText;
            }

            return null;
        }

        public object? ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not string parityText)
                return null;

            if (string.IsNullOrWhiteSpace(parityText))
                return null;

            switch (parityText)
            {
                case _evenText:
                    return ParityType.Even;
                case _oddText:
                    return ParityType.Odd;
                case _bothText:
                    return ParityType.Both;
            }

            return null;
        }
    }
}