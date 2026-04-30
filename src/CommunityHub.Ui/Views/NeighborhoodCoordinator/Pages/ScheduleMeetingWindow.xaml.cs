using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using CommunityHub.Application.Domain;

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

        public ScheduleMeetingWindow(Neighborhood neighborhood, MeetingTheme? fixedTheme = null)
        {
            InitializeComponent();
            _neighborhood = neighborhood;
            _fixedTheme = fixedTheme;
            SetupUI();
        }

        private void SetupUI()
        {
            bool hasFixed = _fixedTheme.HasValue;
            FixedThemeTxt.Visibility = hasFixed ? Visibility.Visible : Visibility.Collapsed;
            ThemeComboBox.Visibility = hasFixed ? Visibility.Collapsed : Visibility.Visible;

            if (hasFixed)
                FixedThemeTxt.Text = _fixedTheme == MeetingTheme.Welcome ? "Dobrodošlica" : "Motivacija";

            FromDatePicker.SelectedDate = DateTime.Now.AddDays(1);
            ToDatePicker.SelectedDate = DateTime.Now.AddDays(7);
        }

        private void StartVotingButton_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateInput(out var time)) return;

            CreatedMeeting = new NeighborhoodMeeting(
                _neighborhood,
                GetSelectedTheme(),
                time,
                DateOnly.FromDateTime(FromDatePicker.SelectedDate!.Value),
                DateOnly.FromDateTime(ToDatePicker.SelectedDate!.Value)
            );

            DialogResult = true;
        }

        private bool ValidateInput(out TimeOnly time)
        {
            if (!TryParseTime(out time)) return false;

            if (!AreDatesValid())
            {
                MessageBox.Show("Početni datum mora biti pre završnog datuma.");
                return false;
            }

            return true;
        }

        private bool TryParseTime(out TimeOnly time)
        {
            if (TimeOnly.TryParse($"{HourTxt.Text}:{MinuteTxt.Text}", out time))
                return true;

            MessageBox.Show("Neispravan format vremena (00-23h : 00-59m).");
            return false;
        }

        private bool AreDatesValid()
        {
            var start = FromDatePicker.SelectedDate;
            var end = ToDatePicker.SelectedDate;

            return start.HasValue && end.HasValue && start <= end;
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
