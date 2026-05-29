using System.Windows;
using System.Windows.Input;
using CommunityHub.Ui.ViewModels.Citizen.LocalFacilities;

namespace CommunityHub.Ui.Views.Citizen.LocalFacilities
{
    public partial class ReservationStatisticsWindow : Window
    {
        public ReservationStatisticsWindow()
        {
            InitializeComponent();
            var viewModel = new ReservationStatisticsViewModel();
            DataContext = viewModel;

            if (viewModel != null)
            {
                viewModel.CloseRequested += (_, _) =>
                {
                    Close();
                };
            }
        }

        private void Header_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
    }
}
