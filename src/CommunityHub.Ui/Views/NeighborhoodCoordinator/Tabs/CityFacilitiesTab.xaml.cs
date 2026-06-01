using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using CommunityHub.Application.Services.Dtos.Neighborhoods;
using CommunityHub.Application.Services.Dtos.Neighborhoods.CityFacilities;
using CommunityHub.Ui.ViewModels.NeighborhoodCoordinator.Tabs;
using CommunityHub.Ui.Views.NeighborhoodCoordinator.Windows;

namespace CommunityHub.Ui.Views.NeighborhoodCoordinator.Tabs
{
    public partial class CityFacilitiesTab : UserControl
    {
        private CityFacilitiesTabViewModel _viewModel = null!;

        public CityFacilitiesTab()
        {
            InitializeComponent();
            Language = XmlLanguage.GetLanguage("sr-Latn-RS");
        }

        public void Initialize(long coordinatorId)
        {
            _viewModel = new CityFacilitiesTabViewModel(coordinatorId);
            DataContext = _viewModel;
            _viewModel.LoadNeighborhoods();
            StartDatePicker.DisplayDateStart = DateTime.Today.AddDays(1);
            EndDatePicker.DisplayDateStart = DateTime.Today.AddDays(1);
        }

        private void NeighborhoodsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
            => _viewModel.SelectedNeighborhood = NeighborhoodsListBox.SelectedItem as NeighborhoodDto;

        private void FacilitiesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
            => _viewModel.SelectedFacility = FacilitiesComboBox.SelectedItem as CityFacilityDto;

        private void ReserveButton_Click(object sender, RoutedEventArgs e)
        {
            if (!TryGetBookingInputs(out var durationHours, out var rangeStart, out var rangeEnd)) return;

            var result = _viewModel.FindSlots(durationHours, rangeStart, rangeEnd);
            if (result is null || (result.Value.SlotInRange is null && result.Value.SlotsOutsideRange.Count == 0))
            {
                new InfoDialog("Nema slobodnih termina", "Nije pronađen nijedan slobodan termin u narednih 90 dana.", Window.GetWindow(this)).ShowDialog();
                return;
            }

            new FacilityBookingWindow(
                _viewModel.SelectedFacility!, _viewModel.SelectedNeighborhood!,
                result.Value.SlotInRange, result.Value.SlotsOutsideRange, _viewModel)
            { 
                Owner = Window.GetWindow(this) 
            }.ShowDialog();
        }

        private void NumberTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !e.Text.All(char.IsDigit);
        }

        private void NumberTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.V &&
                (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)))
                e.Handled = true;
        }

        private bool TryGetBookingInputs(out int durationHours, out DateTime rangeStart, out DateTime rangeEnd)
        {
            durationHours = 0; rangeStart = DateTime.MinValue; rangeEnd = DateTime.MinValue;

            if (_viewModel.SelectedFacility is null)
                return ShowError("Izaberite objekat.");

            if (!int.TryParse(DurationTextBox.Text.Trim(), out durationHours) || durationHours <= 0)
                return ShowError("Unesite ispravno trajanje posete u satima.");

            if (StartDatePicker.SelectedDate is null || EndDatePicker.SelectedDate is null)
                return ShowError("Izaberite opseg datuma.");

            rangeStart = StartDatePicker.SelectedDate.Value;
            rangeEnd = EndDatePicker.SelectedDate.Value.AddDays(1).AddSeconds(-1);

            if (rangeStart >= rangeEnd)
                return ShowError("Početni datum mora biti pre krajnjeg.");

            return true;
        }

        private bool ShowError(string message)
        {
            new InfoDialog("Greška", message, Window.GetWindow(this)).ShowDialog();
            return false;
        }
    }
}