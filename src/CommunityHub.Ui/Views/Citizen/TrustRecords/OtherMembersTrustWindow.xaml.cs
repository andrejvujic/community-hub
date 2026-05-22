using System.Windows;
using System.Windows.Input;
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
            _viewModel.CloseRequested += (_, _) => Close();
            DataContext = _viewModel;
        }

        private void Header_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }
    }
}

