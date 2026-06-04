using CommunityHub.Application.Domain.Users;
using CommunityHub.Application.MappersDto.Buildings;
using CommunityHub.Application.Services.Buildings;
using CommunityHub.Application.Services.Dtos.Buildings;
using CommunityHub.Application.Services.Issues;
using CommunityHub.Application.Services.Issues.Reports;
using CommunityHub.Ui.Helpers.Commands;
using CommunityHub.Ui.Navigation;
using CommunityHub.Ui.ViewModels.BuildingManager.Report;
using CommunityHub.Ui.Views.BuildingManager.Report;
using Microsoft.Win32;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace CommunityHub.Ui.ViewModels.BuildingManager.Navigation
{
    public class MyBuildingsPageViewModel
    {
        private readonly BuildingService _buildingService;
        private readonly IssueService _issueService;
        private readonly BuildingMapper _buildingMapper = new();
        private readonly IAppNavigationService _navigationService;
        public User User { get; }
        public ObservableCollection<BuildingDto> Buildings { get; } = [];

        public ICommand LoadBuildingsCommand { get; }
        public ICommand DetailsCommand { get; }
        public ICommand GenerateReportCommand { get; }
        public MyBuildingsPageViewModel(User user, IAppNavigationService navigationService)
        {
            User = user;
            _navigationService = navigationService;
            _buildingService = new BuildingService();
            _issueService = new IssueService();

            LoadBuildingsCommand = new RelayCommand<object?>(_ => LoadBuildings());
            DetailsCommand = new RelayCommand<BuildingDto?>(OpenDetails);
            GenerateReportCommand = new RelayCommand<object?>(_ => OpenGenerateReportDialog());
        }
        public void LoadBuildings()
        {
            Buildings.Clear();

            var buildings = _buildingService.GetByManagerId(User.Id);

            foreach (var building in buildings)
                Buildings.Add(building);
        }
        private void OpenDetails(BuildingDto? buildingDto)
        {
            if (buildingDto == null) return;

            var building = _buildingMapper.MapToEntity(buildingDto);

            _navigationService.Navigate(new AppNavigationState
            {
                PageTag = "BuildingDetails",
                Building = building,
                SectionTag = "Info"
            });
        }
        private List<BuildingReportOptionViewModel> CreateBuildingReportOptions()
        {
            var options = new List<BuildingReportOptionViewModel>
            {
                new BuildingReportOptionViewModel
                {
                    BuildingId = null,
                    DisplayName = "Sve zgrade"
                }
            };

            foreach (var building in Buildings)
            {
                options.Add(new BuildingReportOptionViewModel
                {
                    BuildingId = building.Id,
                    DisplayName = $"{building.Address.Street.Name} {building.Address.StreetNumber}, {building.Address.Neighborhood}"
                });
            }

            return options;
        }
        private void OpenGenerateReportDialog()
        {
            EnsureBuildingsLoaded();

            if (!TryGetReportFilter(out var selectedBuilding, out var fromDate, out var toDate))
                return;

            var saveFileDialog = CreateSaveFileDialog(selectedBuilding, fromDate, toDate);

            if (saveFileDialog.ShowDialog() != true)
                return;

            GenerateReport(selectedBuilding, fromDate, toDate, saveFileDialog.FileName);
        }
        private void EnsureBuildingsLoaded()
        {
            if (Buildings.Count == 0)
                LoadBuildings();
        }
        private bool TryGetReportFilter(out BuildingReportOptionViewModel selectedBuilding, out DateTime fromDate, out DateTime toDate)
        {
            selectedBuilding = null!;
            fromDate = default;
            toDate = default;

            var options = CreateBuildingReportOptions();
            var dialogViewModel = new ReportFilterDialogViewModel(options);
            var dialog = new ReportFilterDialog(dialogViewModel);

            var result = dialog.ShowDialog();

            if (result != true)
                return false;

            if (dialogViewModel.SelectedBuildingOption == null ||
                dialogViewModel.FromDate == null ||
                dialogViewModel.ToDate == null)
                return false;

            selectedBuilding = dialogViewModel.SelectedBuildingOption;
            fromDate = dialogViewModel.FromDate.Value;
            toDate = dialogViewModel.ToDate.Value;

            return true;
        }
        private SaveFileDialog CreateSaveFileDialog(BuildingReportOptionViewModel selectedBuilding, DateTime fromDate, DateTime toDate)
        {
            return new SaveFileDialog
            {
                FileName = selectedBuilding.IsAll
                    ? $"Izveštaj_SveZgrade_{fromDate:dd-MM-yyyy}_{toDate:dd-MM-yyyy}.pdf"
                    : $"Izveštaj_{fromDate:dd-MM-yyyy}_{toDate:dd-MM-yyyy}.pdf",
                DefaultExt = ".pdf",
                Filter = "PDF dokument (*.pdf)|*.pdf"
            };
        }
        private void GenerateReport(BuildingReportOptionViewModel selectedBuilding, DateTime fromDate, DateTime toDate, string fileName)
        {
            var sections = BuildReportSections(selectedBuilding, fromDate, toDate);

            var reportTitle = selectedBuilding.IsAll
                ? "Sve zgrade"
                : selectedBuilding.DisplayName;

            QuestPDF.Settings.License = LicenseType.Community;

            var document = new IssueReportPdfDocument(
                reportTitle,
                fromDate,
                toDate,
                sections);

            document.GeneratePdf(fileName);
        }
        private List<BuildingIssueReportSection> BuildReportSections(BuildingReportOptionViewModel selectedBuilding, DateTime fromDate, DateTime toDate)
        {
            return selectedBuilding.IsAll
                ? BuildAllBuildingsSections(fromDate, toDate)
                : BuildSingleBuildingSection(selectedBuilding, fromDate, toDate);
        }
        private List<BuildingIssueReportSection> BuildAllBuildingsSections(DateTime fromDate, DateTime toDate)
        {
            var sections = new List<BuildingIssueReportSection>();

            foreach (var building in Buildings)
            {
                var issues = _issueService.GetAllByBuilding(building.Id, fromDate, toDate);

                if (issues == null || issues.Count == 0)
                    continue;

                sections.Add(new BuildingIssueReportSection
                {
                    BuildingName = $"{building.Address.Street.Name} {building.Address.StreetNumber}, {building.Address.Neighborhood}",
                    Items = issues
                });
            }

            return sections;
        }
        private List<BuildingIssueReportSection> BuildSingleBuildingSection(BuildingReportOptionViewModel selectedBuilding, DateTime fromDate, DateTime toDate)
        {
            var sections = new List<BuildingIssueReportSection>();

            var building = Buildings.FirstOrDefault(x => x.Id == selectedBuilding.BuildingId);

            if (building == null)
                return sections;

            var issues = _issueService.GetAllByBuilding(building.Id, fromDate, toDate);

            if (issues != null && issues.Count > 0)
            {
                sections.Add(new BuildingIssueReportSection
                {
                    BuildingName = $"{building.Address.Street.Name} {building.Address.StreetNumber}, {building.Address.Neighborhood}",
                    Items = issues
                });
            }

            return sections;
        }
    }
    public class BuildingReportOptionViewModel
    {
        public long? BuildingId { get; set; }
        public string DisplayName { get; set; } = string.Empty;
        public bool IsAll => BuildingId == null;
    }
}