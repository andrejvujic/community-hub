using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Globalization;
using CommunityHub.Application.Domain;

namespace CommunityHub.Ui.Views.Citizen.Events
{
    public partial class OrganizeEventWindow : Window
    {
        private readonly long _userId;
        public string? CreatedName { get; private set; }
        public string? CreatedDescription { get; private set; }
        public DateTime? CreatedEventStart { get; private set; }
        public int? CreatedDurationMinutes { get; private set; }
        public int? CreatedMinVolunteers { get; private set; }
        public List<EventItem>? CreatedItems { get; private set; }

        public OrganizeEventWindow(long userId)
        {
            InitializeComponent();
            _userId = userId;
        }

        private void Input_Changed(object sender, System.EventArgs e)
        {
            ValidateInputs();
        }

        private void ValidateInputs()
        {
            bool hasName = !string.IsNullOrWhiteSpace(NameBox.Text);
            bool hasDesc = !string.IsNullOrWhiteSpace(DescriptionBox.Text);
            bool hasValidStart = TryGetEventStart(out _);
            bool hasValidDuration = TryGetDuration(out _);
            bool hasValidVolunteers = TryGetMinVolunteers(out _);
            bool hasItems = ParseItems(ItemsBox.Text).Count > 0;

            SaveButton.IsEnabled = hasName && hasDesc && hasValidStart && hasValidDuration && hasValidVolunteers && hasItems;
        }

        private bool TryGetEventStart(out DateTime eventStart)
        {
            eventStart = default;

            if (!DatePicker.SelectedDate.HasValue)
                return false;

            if (!int.TryParse(HourBox.Text, out var h) || h < 0 || h > 23)
                return false;

            if (!int.TryParse(MinuteBox.Text, out var m) || m < 0 || m > 59)
                return false;

            var date = DatePicker.SelectedDate.Value.Date;
            eventStart = DateTime.SpecifyKind(date + new TimeSpan(h, m, 0), DateTimeKind.Unspecified);
            return eventStart > DateTime.Now.AddHours(12);
        }

        private bool TryGetDuration(out int totalMinutes)
        {
            totalMinutes = 0;
            if (!int.TryParse(DurationHourBox.Text, out var dh) || dh < 0) return false;
            if (!int.TryParse(DurationMinuteBox.Text, out var dm) || dm < 0 || dm > 59) return false;

            totalMinutes = dh * 60 + dm;
            return totalMinutes > 0;
        }

        private bool TryGetMinVolunteers(out int minVol)
        {
            minVol = 0;
            if (!int.TryParse(VolunteersBox.Text, out var v)) return false;
            if (v < 0) return false;
            minVol = v;
            return true;
        }

        private void DiscardButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // TODO: implement save
        }

        private List<string> ParseItems(string? raw)
        {
            if (string.IsNullOrWhiteSpace(raw)) return new List<string>();

            var separators = new[] { '\n', '\r', ',' };
            var parts = raw.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            var result = new List<string>();
            foreach (var p in parts)
            {
                var s = p.Trim();
                if (string.IsNullOrEmpty(s)) continue;
                s = string.Join(" ", s.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));
                s = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(s.ToLower());
                if (!result.Contains(s)) result.Add(s);
            }
            return result;
        }
    }
}
