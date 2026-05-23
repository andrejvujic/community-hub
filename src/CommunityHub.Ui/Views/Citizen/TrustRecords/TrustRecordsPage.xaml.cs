using System.Windows;
using System.Windows.Controls;
using CommunityHub.Ui.ViewModels.Citizen.TrustRecords;

namespace CommunityHub.Ui.Views.Citizen.TrustRecords
{
    public partial class TrustRecordsPage : Page
    {
        private TrustRecordsViewModel? _viewModel;

        public TrustRecordsPage(long userId)
        {
            InitializeComponent();

            Loaded += (_, _) =>
            {
                _viewModel = new TrustRecordsViewModel(userId);
                _viewModel.ShowOtherMembersTrustRequested += (_, _) => HandleShowOtherMembersTrust();
                DataContext = _viewModel;
            };
        }

        private void HandleShowOtherMembersTrust()
        {
            if (_viewModel?.NeighborhoodId.HasValue != true || _viewModel?.TrustRecord?.Citizen == null)
                return;

            var wnd = new OtherMembersTrustWindow(_viewModel.TrustRecord.Citizen.Id, _viewModel.NeighborhoodId.Value);
            wnd.Owner = Window.GetWindow(this);
            wnd.ShowDialog();
        }
    }
}
