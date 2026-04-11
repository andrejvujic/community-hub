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
    public partial class GeneralInfoPage : Page
    {
        private readonly User _user;
        private readonly Building _building;
        public GeneralInfoPage(User user, Building building)
        {
            InitializeComponent();
            _user = user;
            _building = building;
        }
    }
}
