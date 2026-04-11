using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CommunityHub.Application.Domain;

namespace CommunityHub.Ui.Views.BuildingManager
{
    public partial class BuildingDetailsPage : Page
    {
        private readonly Building _building;
        private readonly long _userId;
        private Grid? _selected = null;

        public BuildingDetailsPage(long userId, Building building)
        {
            InitializeComponent();
            _building = building;
            _userId = userId;
            BuildingFrame.Navigate(new GeneralInfoPage(_userId, _building));
        }
        private void SideBarItem_Click(object sender, MouseButtonEventArgs e)
        {
            var clicked = sender as Grid;
            if (clicked == null)
                return;

            if (!string.IsNullOrEmpty(clicked.Name))
            {
                SetSideBarItemBackground(clicked);
            }

            string? tag = clicked.Tag?.ToString();

            switch (tag)
            {
                case "Info":
                    BuildingFrame.Navigate(new GeneralInfoPage(_userId, _building));
                    break;
                case "ApartmentAccessRequest":
                    break;
                case "ApartmentMembership":
                    break;
                case "CommonRoom":
                    break;
                case "RentingRoom":
                    break;
                case "NoticeBoard":
                    break;
                case "SchedulingAssembly":
                    break;
                case "ProblemOverview":
                    break;
            }
        }
        private void SetSideBarItemBackground(Grid item)
        {
            if (_selected != null)
                _selected.ClearValue(BackgroundProperty);

            _selected = item;
            _selected.Background = Brushes.LightGray;
        }
    }
}
