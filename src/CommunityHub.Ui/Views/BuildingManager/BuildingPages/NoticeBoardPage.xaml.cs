using CommunityHub.Application.Database.Repositories;
using CommunityHub.Application.Domain;
using CommunityHub.Application.Domain.NoticeBoard;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using System.Collections;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace CommunityHub.Ui.Views.BuildingManager.BuildingPages
{
    public partial class NoticeBoardPage : Page
    {
        private readonly Building _building;
        private readonly User _user;
        public ObservableCollection<Ad> Ads { get; set; } = new();
        private readonly Dictionary<string, int> months = new()
        {
            {"Januar",1}, {"Februar",2}, {"Mart",3}, {"April",4},
            {"Maj",5}, {"Jun",6}, {"Jul",7}, {"Avgust",8},
            {"Septembar",9}, {"Oktobar",10}, {"Novembar",11}, {"Decembar",12}
        };

        private readonly Dictionary<AdCategory, string> categoryNames = new()
        {
            { AdCategory.Moving,      "Selidba"       },
            { AdCategory.Cleaning,    "Čišćenje"      },
            { AdCategory.Repairs,     "Popravke"      },
            { AdCategory.Shopping,    "Nabavka"       },
            { AdCategory.TechSupport, "Tehnička podrška" },
            { AdCategory.PetCare,     "Briga o kućnim ljubimcima" },
            { AdCategory.Borrowing,   "Pozajmljivanje" },
            { AdCategory.Tutoring,    "Instrukcije"   },
            { AdCategory.Transport,   "Prevoz"        },
            { AdCategory.Delivery,    "Dostava"       },
            { AdCategory.Gardening,   "Baštovanstvo"  }
        };
        private readonly AdRepository _adRepository = new();
        public NoticeBoardPage(User user, Building building)
        {
            InitializeComponent();
            DataContext = this;
            _user = user;
            _building = building;
            SetStreetInfo();
            RefreshOfferRequestPieChart();
            RefreshCategoryBarChart();
            LoadActiveAdsByCategory();
        }
        private void NoticeBoardPage_Loaded(object sender, RoutedEventArgs e)
        {
            activeAdsTextBlock.Text = _adRepository.GetAdsCountByStatus(_building.Id, AdStatus.Active).ToString();
            archivedAdsTextBlock.Text = _adRepository.GetAdsCountByStatus(_building.Id, AdStatus.Archived).ToString();
            mostHelpfulResidentTextBlock.Text = _adRepository.GetMostHelpfulResident(_building.Id);
            LoadAds();
        }

        private void LeftArrow_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var homeWindow = Window.GetWindow(this) as HomeWindow;

            if (homeWindow != null)
                homeWindow.HomeFrame.Navigate(new MyBuildingsPage(_user));
        }
        private void SetStreetInfo() => streetInfo.Text = $"{_building.Address.Street.Name} {_building.Address.StreetNumber}";
        private void MonthComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RefreshOfferRequestPieChart();
            RefreshCategoryBarChart();
        }
        private void YearComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RefreshOfferRequestPieChart();
            RefreshCategoryBarChart();
        }
        private void RefreshOfferRequestPieChart()
        {
            if (yearComboBox == null || monthComboBox == null || _building == null)
                return;

            string? year = (yearComboBox.SelectedItem as ComboBoxItem)?.Content?.ToString();
            string? month = (monthComboBox.SelectedItem as ComboBoxItem)?.Content?.ToString();

            if (year == null || month == null)
                return;

            var offerCount = 0;
            var requestCount = 0;

            if (month == "Svi")
            {
                offerCount = _adRepository.GetAdsCountByDateAndType(_building.Id, AdType.Offer, int.Parse(year), null);
                requestCount = _adRepository.GetAdsCountByDateAndType(_building.Id, AdType.Request, int.Parse(year), null);
            }
            else
            {
                offerCount = _adRepository.GetAdsCountByDateAndType(_building.Id, AdType.Offer, int.Parse(year), months[month]);
                requestCount = _adRepository.GetAdsCountByDateAndType(_building.Id, AdType.Request, int.Parse(year), months[month]);
            }

            CreatePieChart(offerCount, requestCount);
        }
        private void CreatePieChart(int offerCount, int requestCount)
        {
            if (offerCount == 0 && requestCount == 0)
            {
                offerRequestPieChart.Visibility = Visibility.Collapsed;
                noDataTextBlock.Visibility = Visibility.Visible;
                categoryDataGrid.Visibility = Visibility.Collapsed;
                return;
            }

            offerRequestPieChart.Visibility = Visibility.Visible;
            categoryDataGrid.Visibility = Visibility.Visible;
            noDataTextBlock.Visibility = Visibility.Collapsed;

            offerRequestPieChart.Series = new ISeries[]
            {
                CreateOfferPieSeries(offerCount),
                CreateRequestPieSeries(requestCount),
            };
        }
        private PieSeries<int> CreateOfferPieSeries(int offerCount) => new PieSeries<int>
        {
            Name = "Oglasi koji nude pomoć",
            Values = new[] { offerCount },
            Pushout = 0,
            HoverPushout = 0,
            Fill = new SolidColorPaint(SKColor.Parse("#01696f"))
        };
        private PieSeries<int> CreateRequestPieSeries(int requestCount) => new PieSeries<int>
        {
            Name = "Oglasi koji traže pomoć",
            Values = new[] { requestCount },
            Pushout = 0,
            HoverPushout = 0,
            Fill = new SolidColorPaint(SKColor.Parse("#964219"))
        };
        private void LoadAds()
        {
            List<Ad> ads = _adRepository.GetByBuilding(_building.Id);

            foreach (Ad ad in ads)
                Ads.Add(ad);
        }
        private void RefreshCategoryBarChart()
        {
            if (yearComboBox == null || monthComboBox == null || _building == null)
                return;

            string? year = (yearComboBox.SelectedItem as ComboBoxItem)?.Content?.ToString();
            string? month = (monthComboBox.SelectedItem as ComboBoxItem)?.Content?.ToString();

            if (year == null || month == null)
                return;

            if (month == "Svi")
            {
                var categoryStats = _adRepository.GetAdStatsByCategory(_building.Id, int.Parse(year), null);
                CreateCategoryBarChart(categoryStats);

            }
            else
            {
                var categoryStats = _adRepository.GetAdStatsByCategory(_building.Id, int.Parse(year), months[month]);
                CreateCategoryBarChart(categoryStats);
            }
        }
        private void CreateCategoryBarChart(List<(AdCategory Category, int OfferCount, int RequestCount)> categoryStats)
        {
            var labels = categoryStats.Select(s => categoryNames[s.Category]).ToArray();

            if (categoryStats.Count == 0)
            {
                categoryBarChart.Visibility = Visibility.Collapsed;
                RefreshCategoryDataGrid(categoryStats);
                return;
            }

            categoryBarChart.Visibility = Visibility.Visible;

            categoryBarChart.Series = new ISeries[]
            {
                new ColumnSeries<int>
                {
                    Name   = "Traži pomoć",
                    Values = categoryStats.Select(s => s.RequestCount).ToArray(),
                    Fill   = new SolidColorPaint(SKColor.Parse("#d4e8e6")),
                    MaxBarWidth = 15,
                },

                new ColumnSeries<int>
                {
                    Name   = "Nudi pomoć",
                    Values = categoryStats.Select(s => s.OfferCount).ToArray(),
                    Fill   = new SolidColorPaint(SKColor.Parse("#01696f")),
                    MaxBarWidth = 15,

                }
            };

            categoryBarChart.YAxes = new[] { new Axis { MinLimit = 0, MaxLimit = categoryStats.Max(s => Math.Max(s.OfferCount, s.RequestCount)) } };
            categoryBarChart.XAxes = new[] { new Axis { Labels = labels, TextSize = 10, LabelsRotation = -65, Padding = new LiveChartsCore.Drawing.Padding(4) } };
            categoryBarChart.TooltipPosition = LiveChartsCore.Measure.TooltipPosition.Hidden;
            categoryBarChart.LegendPosition = LiveChartsCore.Measure.LegendPosition.Bottom;

            RefreshCategoryDataGrid(categoryStats);
        }
        private void RefreshCategoryDataGrid(List<(AdCategory Category, int OfferCount, int RequestCount)> categoryStats)
        {
            var data = categoryStats.Select(s => new
            {
                Category = categoryNames[s.Category],
                OfferCount = s.OfferCount,
                RequestCount = s.RequestCount,
                Sum = s.OfferCount + s.RequestCount
            }).ToList();

            categoryDataGrid.ItemsSource = data;
        }
        private void LoadActiveAdsByCategory()
        {
            List<(AdCategory Category, int ActiveAdsCount)> activeAds = _adRepository.GetActiveAdsByCategory(_building.Id);

            var data = activeAds.Select(a => new
            {
                Category = categoryNames[a.Category],
                ActiveAdsCount = a.ActiveAdsCount
            }).ToList();

            categoryActiveAdsDataGrid.ItemsSource = data;
        }
    }
}