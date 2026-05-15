using System.Windows;
using CommunityHub.Ui.ViewModels.Citizen.TrustRecords;

namespace CommunityHub.Ui.Views.Citizen.TrustRecords
{
    public partial class OtherMembersTrustWindow : Window
    {
        private readonly OtherMembersTrustViewModel _viewModel;

        public OtherMembersTrustWindow(long userId, long neighborhoodId)
        {
            InitializeComponent();
            _viewModel = new OtherMembersTrustViewModel(userId, neighborhoodId);
            DataContext = _viewModel;
        }
    }
}
