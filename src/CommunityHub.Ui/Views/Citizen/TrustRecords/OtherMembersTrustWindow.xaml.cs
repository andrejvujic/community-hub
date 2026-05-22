using System.Windows;
using System.Windows.Input;
using CommunityHub.Ui.Helpers.Commands;
using CommunityHub.Ui.ViewModels.Citizen.TrustRecords;

namespace CommunityHub.Ui.Views.Citizen.TrustRecords
{
    public partial class OtherMembersTrustWindow : Window
    {
        private readonly OtherMembersTrustViewModel _viewModel;

        public ICommand CloseCommand { get; }

        public OtherMembersTrustWindow(long userId, long neighborhoodId)
        {
            InitializeComponent();
            CloseCommand = new RelayCommand<object>(_ => Close());
            _viewModel = new OtherMembersTrustViewModel(userId, neighborhoodId);
            DataContext = _viewModel;
        }

        private void Header_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }
    }
}

