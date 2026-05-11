using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using CommunityHub.Application.Database.Repositories.NeighborhoodMeetings;
using CommunityHub.Application.Domain.Neighborhoods;
using CommunityHub.Application.Domain.Neighborhoods.NeighborhoodMeetings;

namespace CommunityHub.Ui.Views.NeighborhoodCoordinator.Pages
{
    /// <summary>
    /// Interaction logic for ScheduleMeetingWindow.xaml
    /// </summary>
    public partial class ScheduleMeetingWindow : Window
    {
        private readonly Neighborhood _neighborhood;
        private readonly MeetingTheme? _fixedTheme;
        public NeighborhoodMeeting? CreatedMeeting { get; private set; }

        private readonly NeighborhoodMeetingRepository _meetingRepository = new NeighborhoodMeetingRepository();

        public ScheduleMeetingWindow(Neighborhood neighborhood, MeetingTheme? fixedTheme = null)
        {
            InitializeComponent();
            this.Language = XmlLanguage.GetLanguage("sr-Latn-RS");
            _neighborhood = neighborhood;
            _fixedTheme = fixedTheme;
            SetupUI();
        }

        private void SetupUI()
        {
            InitializeThemeUI();
            ConfigureDatePickers();
        }

        private void InitializeThemeUI()
        {
            bool hasFixed = _fixedTheme.HasValue;
            FixedThemeTxt.Visibility = hasFixed ? Visibility.Visible : Visibility.Collapsed;
            ThemeComboBox.Visibility = hasFixed ? Visibility.Collapsed : Visibility.Visible;

            if (hasFixed)
                FixedThemeTxt.Text = _fixedTheme == MeetingTheme.Welcome ? "Dobrodošlica" : "Motivacija";
            else
                ThemeComboBox.SelectedIndex = 0;
        }

        private void ConfigureDatePickers()
        {
            DateTime minDate = DateTime.Today.AddDays(2);
            FromDatePicker.DisplayDateStart = minDate;
            ToDatePicker.DisplayDateStart = minDate;

            FromDatePicker.SelectedDate = minDate;
            ToDatePicker.SelectedDate = DateTime.Today.AddDays(8);
        }

        private void StartVotingButton_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateInput(out var time)) return;

            CreatedMeeting = _meetingRepository.Create(new NeighborhoodMeeting(
                _neighborhood,
                GetSelectedTheme(),
                time,
                DateOnly.FromDateTime(FromDatePicker.SelectedDate!.Value),
                DateOnly.FromDateTime(ToDatePicker.SelectedDate!.Value)
            ));

            DialogResult = true;
        }

        private bool ValidateInput(out TimeOnly time)
        {
            time = default;
            return TryParseTime(out time) && AreDatesValid();
        }

        private bool TryParseTime(out TimeOnly time)
        {
            if (TimeOnly.TryParse($"{HourTxt.Text}:{MinuteTxt.Text}", out time)) return true;
            MessageBox.Show("Neispravno vreme (00-23h : 00-59m).", "Greška", MessageBoxButton.OK, MessageBoxImage.Warning);
            return false;
        }

        private bool AreDatesValid()
        {
            if (FromDatePicker.SelectedDate <= ToDatePicker.SelectedDate) return true;
            MessageBox.Show("Početni datum mora biti pre završnog.", "Greška", MessageBoxButton.OK, MessageBoxImage.Warning);
            return false;
        }

        private MeetingTheme GetSelectedTheme()
        {
            if (_fixedTheme.HasValue) return _fixedTheme.Value;
            var item = (ComboBoxItem)ThemeComboBox.SelectedItem;
            return (MeetingTheme)item.Tag;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
