using System.Windows.Controls;
using System.Windows;
using CommunityHub.Ui.ViewModels.Citizen.TrustRecords;

namespace CommunityHub.Ui.Views.Citizen.TrustRecords
{
    public partial class TrustRecordsPage : Page
    {
        private readonly TrustRecordsViewModel _viewModel;

        public TrustRecordsPage(long userId)
        {
            InitializeComponent();
            _viewModel = new TrustRecordsViewModel(userId);
            DataContext = _viewModel;
        }

        private void ShowOtherMembersTrustButton_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel.NeighborhoodId.HasValue)
            {
                var wnd = new OtherMembersTrustWindow(_viewModel.TrustRecord!.Citizen.Id, _viewModel.NeighborhoodId.Value);
                wnd.Owner = Window.GetWindow(this);
                wnd.ShowDialog();
            }
        }
    }
}