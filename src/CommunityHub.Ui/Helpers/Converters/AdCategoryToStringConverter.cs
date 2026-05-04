using CommunityHub.Application.Modules.Buildings.Core.Domain.Ads;
using System.Globalization;
using System.Windows.Data;

namespace CommunityHub.Ui.Helpers.Converters
{
    public class AdCategoryToStringConverter : IValueConverter
    {
        private readonly Dictionary<AdCategory, string> _categories = new()
        {
            { AdCategory.Moving, "Selidba" },
            { AdCategory.Cleaning, "Čišćenje" },
            { AdCategory.Repairs, "Popravke" },
            { AdCategory.Maintenance, "Održavanje" },
            { AdCategory.Borrowing, "Pozajmljivanje" },
            { AdCategory.Lending, "Ustupanje" },
            { AdCategory.Delivery, "Dostava" },
            { AdCategory.Shopping, "Kupovina" },
            { AdCategory.TechSupport, "Tehnička podrška" },
            { AdCategory.PetCare, "Briga o kućnim ljubimcima" },
            { AdCategory.ChildCare, "Briga o deci" },
            { AdCategory.ElderlyCare, "Briga o starijima" },
            { AdCategory.Gardening, "Baštovanstvo" },
            { AdCategory.Tutoring, "Podučavanje" },
            { AdCategory.Transport, "Prevoz" }
        };

        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string? defaultText = parameter is string parameterValue ? parameterValue : null;

            if (value is not AdCategory category) return defaultText;

            if (_categories.TryGetValue(category, out string? categoryText))
                return categoryText;

            return defaultText;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
