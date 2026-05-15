using CommunityHub.Application.Domain.Ads;
using CommunityHub.Application.Domain.Buildings;
using CommunityHub.Application.Domain.Users;
using LiveChartsCore;
using CommunityHub.Application.Services.Dtos.Ads;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using System.Collections;
using CommunityHub.Application.Services.Dtos.Buildings;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using CommunityHub.Application.Database.Repositories.Ads;
using CommunityHub.Application.Database.Repositories.Ads.TimeSlots;
using System.Windows;
using CommunityHub.Application.Modules.Buildings.Core.Services.Ads;
using CommunityHub.Application.MappersDto.Buildings;

namespace CommunityHub.Ui.ViewModels.BuildingManager.BuildingPages
{
    public class NoticeBoardPageViewModel : INotifyPropertyChanged
    {
        private readonly BuildingMapper _buildingMapper = new();
        private readonly AdService _adService = new(
            new AdRepository(),
            new AdTimeSlotRepository()
        );

        private readonly Dictionary<string, int> _monthsMap = new()
        {
            {"Januar",1}, {"Februar",2}, {"Mart",3}, {"April",4},
            {"Maj",5}, {"Jun",6}, {"Jul",7}, {"Avgust",8},
            {"Septembar",9}, {"Oktobar",10}, {"Novembar",11}, {"Decembar",12}
        };

        private readonly Dictionary<AdCategory, string> _categoryNames = new()
        {
            { AdCategory.Moving, "Selidba" },
            { AdCategory.Cleaning, "Čišćenje" },
            { AdCategory.Repairs, "Popravke" },
            { AdCategory.Shopping, "Nabavka" },
            { AdCategory.TechSupport, "Tehnička podrška" },
            { AdCategory.PetCare, "Briga o kućnim ljubimcima" },
            { AdCategory.Borrowing, "Pozajmljivanje" },
            { AdCategory.Tutoring, "Instrukcije" },
            { AdCategory.Transport, "Prevoz" },
            { AdCategory.Delivery, "Dostava" },
            { AdCategory.Gardening, "Baštovanstvo" }
        };

        public User User { get; }
        public Building Building { get; }
        public BuildingDto BuildingDto { get; }
        public ObservableCollection<AdDto> Ads { get; } = new();

        public List<string> Months { get; } = new()
        {
            "Svi", "Januar", "Februar", "Mart", "April", "Maj", "Jun",
            "Jul", "Avgust", "Septembar", "Oktobar", "Novembar", "Decembar"
        };

        public List<string> Years { get; } = new();

        private string? _selectedMonth;
        public string? SelectedMonth
        {
            get => _selectedMonth;
            set
            {
                if (_selectedMonth != value)
                {
                    _selectedMonth = value;
                    OnPropertyChanged();
                    RefreshCharts();
                }
            }
        }

        private string? _selectedYear;
        public string? SelectedYear
        {
            get => _selectedYear;
            set
            {
                if (_selectedYear != value)
                {
                    _selectedYear = value;
                    OnPropertyChanged();
                    RefreshCharts();
                }
            }
        }

        private int _activeAdsCount;
        public int ActiveAdsCount
        {
            get => _activeAdsCount;
            set { _activeAdsCount = value; OnPropertyChanged(); }
        }

        private int _archivedAdsCount;
        public int ArchivedAdsCount
        {
            get => _archivedAdsCount;
            set { _archivedAdsCount = value; OnPropertyChanged(); }
        }

        private string _mostHelpfulResident = string.Empty;
        public string MostHelpfulResident
        {
            get => _mostHelpfulResident;
            set { _mostHelpfulResident = value; OnPropertyChanged(); }
        }

        public string StreetInfo => $"{BuildingDto.Address.Street.Name} {BuildingDto.Address.StreetNumber}";

        private ISeries[] _offerRequestPieSeries = [];
        public ISeries[] OfferRequestPieSeries
        {
            get => _offerRequestPieSeries;
            set { _offerRequestPieSeries = value; OnPropertyChanged(); }
        }

        private ISeries[] _categoryBarSeries = [];
        public ISeries[] CategoryBarSeries
        {
            get => _categoryBarSeries;
            set { _categoryBarSeries = value; OnPropertyChanged(); }
        }

        private Axis[] _categoryXAxes = [];
        public Axis[] CategoryXAxes
        {
            get => _categoryXAxes;
            set { _categoryXAxes = value; OnPropertyChanged(); }
        }

        private Axis[] _categoryYAxes = [];
        public Axis[] CategoryYAxes
        {
            get => _categoryYAxes;
            set { _categoryYAxes = value; OnPropertyChanged(); }
        }

        private Visibility _offerRequestPieChartVisibility = Visibility.Visible;
        public Visibility OfferRequestPieChartVisibility
        {
            get => _offerRequestPieChartVisibility;
            set { _offerRequestPieChartVisibility = value; OnPropertyChanged(); }
        }

        private Visibility _categoryBarChartVisibility = Visibility.Visible;
        public Visibility CategoryBarChartVisibility
        {
            get => _categoryBarChartVisibility;
            set { _categoryBarChartVisibility = value; OnPropertyChanged(); }
        }

        private Visibility _noDataVisibility = Visibility.Collapsed;
        public Visibility NoDataVisibility
        {
            get => _noDataVisibility;
            set { _noDataVisibility = value; OnPropertyChanged(); }
        }

        private Visibility _categoryDataGridVisibility = Visibility.Visible;
        public Visibility CategoryDataGridVisibility
        {
            get => _categoryDataGridVisibility;
            set { _categoryDataGridVisibility = value; OnPropertyChanged(); }
        }

        private IEnumerable? _categoryData;
        public IEnumerable? CategoryData
        {
            get => _categoryData;
            set { _categoryData = value; OnPropertyChanged(); }
        }

        private IEnumerable? _categoryActiveAdsData;
        public IEnumerable? CategoryActiveAdsData
        {
            get => _categoryActiveAdsData;
            set { _categoryActiveAdsData = value; OnPropertyChanged(); }
        }

        public NoticeBoardPageViewModel(User user, Building building)
        {
            User = user;
            Building = building;
            BuildingDto = _buildingMapper.MapToDto(building);

            LoadYears();

            SelectedYear = Years.Last();
            SelectedMonth = "Svi";

            LoadStaticData();
            OnPropertyChanged(nameof(SelectedYear));
            OnPropertyChanged(nameof(SelectedMonth));
        }
        public void LoadPageData()
        {
            ActiveAdsCount = _adService.GetAdsCountByStatus(Building.Id, AdStatus.Active);
            ArchivedAdsCount = _adService.GetAdsCountByStatus(Building.Id, AdStatus.Archived);
            MostHelpfulResident = _adService.GetMostHelpfulResident(Building.Id);
            LoadAds();
        }
        private void LoadYears()
        {
            Years.Clear();
            
            var years = _adService.GetDistinctYears();
            
            foreach (var year in years)
                Years.Add(year);
        }
        private void LoadAds()
        {
            Ads.Clear();

            var ads = _adService.GetAllByBuildingId(Building.Id);
            foreach (var ad in ads)
                Ads.Add(ad);
        }
        private void LoadStaticData()
        {
            RefreshCharts();
            LoadActiveAdsByCategory();
        }
        private void RefreshCharts()
        {
            if (string.IsNullOrWhiteSpace(SelectedYear) || string.IsNullOrWhiteSpace(SelectedMonth))
                return;

            RefreshOfferRequestPieChart();
            RefreshCategoryBarChart();
        }
        private void RefreshOfferRequestPieChart()
        {
            int offerCount;
            int requestCount;

            if (SelectedMonth == "Svi")
            {
                offerCount = _adService.GetAdsCountByDateAndType(Building.Id, AdType.Offer, int.Parse(SelectedYear!), null);
                requestCount = _adService.GetAdsCountByDateAndType(Building.Id, AdType.Request, int.Parse(SelectedYear!), null);
            }
            else
            {
                offerCount = _adService.GetAdsCountByDateAndType(Building.Id, AdType.Offer, int.Parse(SelectedYear!), _monthsMap[SelectedMonth!]);
                requestCount = _adService.GetAdsCountByDateAndType(Building.Id, AdType.Request, int.Parse(SelectedYear!), _monthsMap[SelectedMonth!]);
            }

            if (offerCount == 0 && requestCount == 0)
            {
                OfferRequestPieChartVisibility = Visibility.Collapsed;
                NoDataVisibility = Visibility.Visible;
                CategoryDataGridVisibility = Visibility.Collapsed;
                OfferRequestPieSeries = [];
                return;
            }

            OfferRequestPieChartVisibility = Visibility.Visible;
            NoDataVisibility = Visibility.Collapsed;
            CategoryDataGridVisibility = Visibility.Visible;
            CreatePieChart(offerCount, requestCount);

        }
        private void CreatePieChart(int offerCount, int requestCount)
        {
            OfferRequestPieSeries = new ISeries[]
            {
                new PieSeries<int>
                {
                    Name = "Oglasi koji nude pomoć",
                    Values = new[] { offerCount },
                    Pushout = 0,
                    HoverPushout = 0,
                    Fill = new SolidColorPaint(SKColor.Parse("#01696f"))
                },
                new PieSeries<int>
                {
                    Name = "Oglasi koji traže pomoć",
                    Values = new[] { requestCount },
                    Pushout = 0,
                    HoverPushout = 0,
                    Fill = new SolidColorPaint(SKColor.Parse("#964219"))
                }
            };
        }
        private void RefreshCategoryBarChart()
        {
            List<(AdCategory Category, int OfferCount, int RequestCount)> categoryStats;

            if (SelectedMonth == "Svi")
                categoryStats = _adService.GetAdStatsByCategory(Building.Id, int.Parse(SelectedYear!), null);
            else
                categoryStats = _adService.GetAdStatsByCategory(Building.Id, int.Parse(SelectedYear!), _monthsMap[SelectedMonth!]);

            var labels = categoryStats.Select(s => _categoryNames[s.Category]).ToArray();

            if (categoryStats.Count == 0)
            {
                CategoryBarChartVisibility = Visibility.Collapsed;
                CategoryData = new List<object>();
                return;
            }

            CategoryBarChartVisibility = Visibility.Visible;

           CreateBarChart(categoryStats);

            CategoryYAxes = new[]
            {
                new Axis
                {
                    MinLimit = 0,
                    MaxLimit = categoryStats.Max(s => Math.Max(s.OfferCount, s.RequestCount))
                }
            };

            CategoryXAxes = new[]
            {
                new Axis
                {
                    Labels = labels,
                    TextSize = 10,
                    LabelsRotation = -65,
                    Padding = new LiveChartsCore.Drawing.Padding(4)
                }
            };

            LoadCategoryDataGrid(categoryStats);
        }
        private void LoadCategoryDataGrid(List<(AdCategory Category, int OfferCount, int RequestCount)> categoryStats)
        {
            CategoryData = categoryStats.Select(s => new
            {
                Category = _categoryNames[s.Category],
                OfferCount = s.OfferCount,
                RequestCount = s.RequestCount,
                Sum = s.OfferCount + s.RequestCount
            }).ToList();
        }
        private void CreateBarChart(List<(AdCategory Category, int OfferCount, int RequestCount)> categoryStats)
        {
            CategoryBarSeries = new ISeries[]
           {
                new ColumnSeries<int>
                {
                    Name = "Traži pomoć",
                    Values = categoryStats.Select(s => s.RequestCount).ToArray(),
                    Fill = new SolidColorPaint(SKColor.Parse("#d4e8e6")),
                    MaxBarWidth = 15
                },
                new ColumnSeries<int>
                {
                    Name = "Nudi pomoć",
                    Values = categoryStats.Select(s => s.OfferCount).ToArray(),
                    Fill = new SolidColorPaint(SKColor.Parse("#01696f")),
                    MaxBarWidth = 15
                }
           };
        }
        private void LoadActiveAdsByCategory()
        {
            var activeAds = _adService.GetActiveAdsByCategory(Building.Id);

            CategoryActiveAdsData = activeAds.Select(a => new
            {
                Category = _categoryNames[a.Category],
                ActiveAdsCount = a.ActiveAdsCount
            }).ToList();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}