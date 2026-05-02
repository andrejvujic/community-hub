using System.Globalization;
using System.Windows.Data;
using CommunityHub.Application.Domain.NeighborhoodBudget;

namespace CommunityHub.Ui.Helpers.Converters
{
    public class BudgetCategoryToStringConverter : IValueConverter
    {
        private const string _streetMaintenanceText = "Uređenje ulica";
        private const string _parkMaintenanceText = "Uređenje parkova";
        private const string _lightingText = "Rasveta";
        private const string _cleanlinessText = "Čistoća";
        private const string _greeneryText = "Zelenilo";
        private const string _emergencyRepairsText = "Hitne popravke";
        private const string _playgroundsText = "Igrališta";
        private const string _sportsFacilitiesText = "Sportski objekti";

        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not BudgetCategory category)
                return null;

            return category switch
            {
                BudgetCategory.StreetMaintenance => _streetMaintenanceText,
                BudgetCategory.ParkMaintenance => _parkMaintenanceText,
                BudgetCategory.Lighting => _lightingText,
                BudgetCategory.Cleanliness => _cleanlinessText,
                BudgetCategory.Greenery => _greeneryText,
                BudgetCategory.EmergencyRepairs => _emergencyRepairsText,
                BudgetCategory.Playgrounds => _playgroundsText,
                BudgetCategory.SportsFacilities => _sportsFacilitiesText,
                _ => null
            };
        }

        public object? ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not string categoryText)
                return null;

            if (string.IsNullOrWhiteSpace(categoryText))
                return null;

            return categoryText switch
            {
                _streetMaintenanceText => BudgetCategory.StreetMaintenance,
                _parkMaintenanceText => BudgetCategory.ParkMaintenance,
                _lightingText => BudgetCategory.Lighting,
                _cleanlinessText => BudgetCategory.Cleanliness,
                _greeneryText => BudgetCategory.Greenery,
                _emergencyRepairsText => BudgetCategory.EmergencyRepairs,
                _playgroundsText => BudgetCategory.Playgrounds,
                _sportsFacilitiesText => BudgetCategory.SportsFacilities,
                _ => null
            };
        }
    }
}
