using CommunityHub.Application.Domain.Users;
using CommunityHub.Application.Services.Apartments;
using CommunityHub.Application.Services.Buildings;
using CommunityHub.Application.Services.Dtos.Issues;
using CommunityHub.Application.Services.Dtos.ResidentAssemblies;
using CommunityHub.Application.Services.Issues;
using CommunityHub.Application.Services.ResidentAssemblies;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using System.Globalization;

namespace CommunityHub.Ui.ViewModels.BuildingManager.Navigation
{
    public class HomePageViewModel
    {
        private readonly BuildingService _buildingService = new();
        private readonly ApartmentMembershipService _apartmentMembershipService = new();
        private readonly AssemblyService _assemblyService = new();
        private readonly IssueService _issueService = new();

        public User User { get; }
        public int BuildingsCount { get; private set; }
        public int ResidentsCount { get; private set; }
        public int OpenIssuesCount { get; private set; }
        public AssemblyDto? NextAssembly { get; private set; }
        public ISeries[] OpenIssuesSeries { get; private set; } = Array.Empty<ISeries>();
        public Axis[] XAxes { get; private set; } = Array.Empty<Axis>();
        public Axis[] YAxes { get; private set; } = Array.Empty<Axis>();
        public string NextAssemblyDisplay =>
           NextAssembly != null
         ? NextAssembly.StartsAt.ToString("dd. MMMM yyyy.", new CultureInfo("sr-Latn-RS"))
           + Environment.NewLine +
           NextAssembly.StartsAt.ToString("HH:mm'h'", new CultureInfo("sr-Latn-RS"))
         : "Nema skupštine";

        public HomePageViewModel(User user)
        {
            User = user;
            LoadBuildingCount();
            LoadResidentsCount();
            LoadNextAssembly();
            LoadOpenIssuesCount();
            LoadOpenIssuesChart();
        }

        public void LoadBuildingCount() => BuildingsCount = _buildingService.GetBuildingsCountForManager(User.Id);
        public void LoadResidentsCount() => ResidentsCount = _apartmentMembershipService.CountResidentsForManagerBuildings(User.Id);
        public void LoadNextAssembly() => NextAssembly = _assemblyService.GetNearestAssembly(User.Id);
        public void LoadOpenIssuesCount() => OpenIssuesCount = _issueService.CountOpenIssuesForManagerBuildings(User.Id);
        public void LoadOpenIssuesChart()
        {
            var chartData = _issueService.GetOpenIssuesChartDataForManager(User.Id);

            CreateOpenIssuesChart(chartData);
            ConfigureOpenIssuesAxes(chartData);
        }
        private void CreateOpenIssuesChart(List<BuildingOpenIssuesChartDto> chartData)
        {
            OpenIssuesSeries = new ISeries[]
            {
                new RowSeries<int>
                {
                    Name = "Broj nerešenih problema",
                    Values = chartData.Select(x => x.OpenIssuesCount).ToArray(),
                    Fill = new SolidColorPaint(SKColor.Parse("#BAD7F5")),
                    MaxBarWidth = 25,
                    DataLabelsSize = 14
                }
            };
        }
        private void ConfigureOpenIssuesAxes(List<BuildingOpenIssuesChartDto> chartData)
        {
           ConfigureXAxes();
           ConfigureYAxes(chartData);
        }
        private void ConfigureXAxes()
        {
            XAxes = new[]
           {
                new Axis
                {
                    Name = "Broj nerešenih problema",
                    NameTextSize = 20,
                    NamePadding = new LiveChartsCore.Drawing.Padding(0, 15, 0, 20),
                    MinLimit = 0,
                    MinStep = 1
                }
            };
        }
        private void ConfigureYAxes(List<BuildingOpenIssuesChartDto> chartData)
        {
            YAxes = new[]
            {
                new Axis
                {
                    Labels = chartData.Select(x => x.BuildingInfo).ToArray(),
                    TextSize = 14,
                    LabelsRotation = -15,
                    ForceStepToMin = true,
                    MinStep = 1,
                    Padding = new LiveChartsCore.Drawing.Padding(20, 0, 28, 0)
                }
            };
        }
    }
}