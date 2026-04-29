using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CommunityHub.Application.Database.Repositories;
using CommunityHub.Application.Domain;

namespace CommunityHub.Ui.Views.NeighborhoodCoordinator.Pages
{
    /// <summary>
    /// Interaction logic for StatisticsAndMeetingsTab.xaml
    /// </summary>
    public partial class StatisticsAndMeetingsTab : UserControl
    {
        private long _coordinatorId;
        private Neighborhood? _selectedNeighborhood;

        private readonly NeighborhoodRepository _neighborhoodRepository = new NeighborhoodRepository();
        private readonly NeighborhoodMembershipRepository _membershipRepository = new NeighborhoodMembershipRepository();
        private readonly NeighborhoodMeetingRepository _meetingRepository = new NeighborhoodMeetingRepository();

        public ObservableCollection<Neighborhood> Neighborhoods { get; set; } = new ObservableCollection<Neighborhood>();
        public ObservableCollection<NeighborhoodMeeting> Meetings { get; set; } = new ObservableCollection<NeighborhoodMeeting>();

        public StatisticsAndMeetingsTab()
        {
            InitializeComponent();
            DataContext = this;
        }

        public void Initialize(long coordinatorId)
        {
            _coordinatorId = coordinatorId;
            LoadNeighborhoods();
        }

        private void LoadNeighborhoods()
        {
            Neighborhoods.Clear();
            var neighborhoods = _neighborhoodRepository.GetByCoordinatorId(_coordinatorId);
            foreach (var n in neighborhoods)
            {
                Neighborhoods.Add(n);
            }
        }

        private void NeighborhoodsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedNeighborhood = NeighborhoodsListBox.SelectedItem as Neighborhood;
            if (_selectedNeighborhood == null) return;

            LoadAndDisplayStatistics();
            LoadMeetings();
            StatisticsAndMeetingsPanel.Visibility = Visibility.Visible;
        }

        private void LoadAndDisplayStatistics()
        {
            var trustRecords = _membershipRepository.GetTrustRecordsByNeighborhoodId(_selectedNeighborhood!.Id);

            if (trustRecords.Count == 0) return;

            DisplayTrustStatusCounts(trustRecords);
        }

        private void LoadMeetings()
        {
            Meetings.Clear();
            var data = _meetingRepository.GetByNeighborhoodId(_selectedNeighborhood!.Id);
            foreach (var m in data) Meetings.Add(m);
        }

        private void DisplayTrustStatusCounts(List<CitizenTrustRecord> trustRecords)
        {
            int total = trustRecords.Count;
            TotalCitizensTxt.Text = total.ToString();

            UpdateRow(NewCitizensTxt, NewCitizensPercentTxt, trustRecords, TrustStatus.New, total);
            UpdateRow(InactiveCitizensTxt, InactiveCitizensPercentTxt, trustRecords, TrustStatus.Inactive, total);
            UpdateRow(ActiveCitizensTxt, ActiveCitizensPercentTxt, trustRecords, TrustStatus.Active, total);
            UpdateRow(ProminentCitizensTxt, ProminentCitizensPercentTxt, trustRecords, TrustStatus.Prominent, total);
            UpdateRow(TrustworthyCitizensTxt, TrustworthyCitizensPercentTxt, trustRecords, TrustStatus.Trustworthy, total);

            UpdateRecommendationUI(trustRecords);
        }

        private void UpdateRow(TextBlock countTxt, TextBlock percentTxt, List<CitizenTrustRecord> trustRecords, TrustStatus status, int total)
        {
            int count = trustRecords.Count(r => r.TrustStatus == status);
            double percent = Math.Round((double)count / total * 100, 1);

            countTxt.Text = count.ToString();
            percentTxt.Text = $"({percent}%)";
        }

        private void UpdateRecommendationUI(List<CitizenTrustRecord> trustRecords)
        {
            ResetRecommendationUI();
            if (trustRecords.Count == 0) return;

            int newCount = trustRecords.Count(r => r.TrustStatus == TrustStatus.New);
            int inactiveCount = trustRecords.Count(r => r.TrustStatus == TrustStatus.Inactive);
            int maxCount = GetMaxTrustCount(trustRecords);

            if (maxCount == newCount) ShowWelcomeRecommendation();
            else if (maxCount == inactiveCount) ShowMotivationRecommendation();
        }

        private int GetMaxTrustCount(List<CitizenTrustRecord> records)
        {
            int newCount = records.Count(r => r.TrustStatus == TrustStatus.New);
            int inactiveCount = records.Count(r => r.TrustStatus == TrustStatus.Inactive);
            int activeCount = records.Count(r => r.TrustStatus == TrustStatus.Active);
            int prominentCount = records.Count(r => r.TrustStatus == TrustStatus.Prominent);
            int trustworthyCount = records.Count(r => r.TrustStatus == TrustStatus.Trustworthy);

            int maxNew = Math.Max(newCount, inactiveCount);
            int maxInactive = Math.Max(activeCount, prominentCount);
            return Math.Max(maxNew, Math.Max(maxInactive, trustworthyCount));
        }

        private void ResetRecommendationUI()
        {
            RecommendationContainer.Visibility = Visibility.Collapsed;
            WelcomeRecommendationText.Visibility = Visibility.Collapsed;
            WelcomeMeetingButton.Visibility = Visibility.Collapsed;
            MotivationRecommendationText.Visibility = Visibility.Collapsed;
            MotivationMeetingButton.Visibility = Visibility.Collapsed;
        }

        private void ShowWelcomeRecommendation()
        {
            RecommendationContainer.Visibility = Visibility.Visible;
            WelcomeRecommendationText.Visibility = Visibility.Visible;
            WelcomeMeetingButton.Visibility = Visibility.Visible;
            WelcomeNeighborhoodNameRun.Text = _selectedNeighborhood!.Name;
        }

        private void ShowMotivationRecommendation()
        {
            RecommendationContainer.Visibility = Visibility.Visible;
            MotivationRecommendationText.Visibility = Visibility.Visible;
            MotivationMeetingButton.Visibility = Visibility.Visible;
            MotivationNeighborhoodNameRun.Text = _selectedNeighborhood!.Name;
        }

        private void WelcomeMeetingButton_Click(object sender, RoutedEventArgs e)
        {
            OpenScheduleWindow(MeetingTheme.Welcome);
        }

        private void MotivationMeetingButton_Click(object sender, RoutedEventArgs e)
        {
            OpenScheduleWindow(MeetingTheme.Motivation);
        }

        private void ScheduleNewMeetingButton_Click(object sender, RoutedEventArgs e)
        {
            OpenScheduleWindow(null);
        }

        private void OpenScheduleWindow(MeetingTheme? theme)
        {
            var window = new ScheduleMeetingWindow(theme)
            {
                Owner = Window.GetWindow(this)
            };
            window.ShowDialog();
        }
    }
}
