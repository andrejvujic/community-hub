using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Globalization;
using CommunityHub.Application.Database.Repositories;
using CommunityHub.Application.Domain.Events;
using CommunityHub.Application.Database.Repositories.Events;

namespace CommunityHub.Ui.Views.Citizen.Events
{
    public partial class OrganizeEventWindow : Window
    {   
        private readonly long _userId;
        private readonly CitizenRepository _citizenRepository = new CitizenRepository();
        private readonly CommunityEventRepository _communityEventRepository = new CommunityEventRepository();

        private DateTime? _cachedEventStart;
        private int? _cachedDurationMinutes;
        private int? _cachedMinVolunteers;
        private List<string>? _cachedItemNames;

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
            if (TryGetEventStart(out var es)) _cachedEventStart = es; else _cachedEventStart = null;
            if (TryGetDuration(out var dm)) _cachedDurationMinutes = dm; else _cachedDurationMinutes = null;
            if (TryGetMinVolunteers(out var mv)) _cachedMinVolunteers = mv; else _cachedMinVolunteers = null;
            var items = ParseItems(ItemsBox.Text);
            _cachedItemNames = items.Count > 0 ? items : null;

            SaveButton.IsEnabled = hasName && hasDesc && _cachedEventStart.HasValue && _cachedDurationMinutes.HasValue && _cachedMinVolunteers.HasValue && _cachedItemNames != null;
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
            var name = NameBox.Text.Trim();
            var description = DescriptionBox.Text.Trim();
            var eventStart = _cachedEventStart!.Value;
            var duration = _cachedDurationMinutes!.Value;
            var minVolunteers = _cachedMinVolunteers!.Value;
            var items = _cachedItemNames!.Select(n => new EventItem(0, n, 0)).ToList();

            var organizer = _citizenRepository.GetById(_userId);
            var neighborhood = organizer!.Membership!.Neighborhood;

            CommunityEvent ev = new CommunityEvent(
                id: 0,
                name: name,
                description: description,
                eventStart: eventStart,
                durationMinutes: duration,
                minVolunteers: minVolunteers,
                status: EventStatus.Preparation,
                neighborhood: neighborhood,
                organizer: organizer,
                items: items
            );

            CommunityEvent createdEvent = _communityEventRepository.Create(ev);
            this.DialogResult = true;
            this.Close();
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
