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

namespace CommunityHub.Ui.Views.BuildingManager
{
    public partial class RegistrationPage : Page
    {
        private readonly long _userId;

        public RegistrationPage(long userId)
        {
            InitializeComponent();
            _userId = userId;

        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            floorsStackPanel.Children.Add(CreateFloor(1));
        }
        private void FloorsUpDown_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (floorsStackPanel == null)
                return;

            int newValue = stepperFloorsUpDown.Value ?? 1;
            int currentCount = floorsStackPanel.Children.Count;

            if (newValue > currentCount)
            {
                for (int i = currentCount + 1; i <= newValue; i++)
                {
                    floorsStackPanel.Children.Add(CreateFloor(i));
                }
            }
            else
            {
                while (floorsStackPanel.Children.Count > newValue)
                {
                    floorsStackPanel.Children.RemoveAt(floorsStackPanel.Children.Count - 1);
                }
            }
        }
        private GroupBox CreateFloor(int number)
        {
            var groupBox = new GroupBox
            {
                Header = $"Sprat {number} - spratovi",
                Margin = new Thickness(15, 10, 15, 20),
                Height = 140
            };

            var apartmentsPanel = new StackPanel { Orientation = Orientation.Horizontal, Tag = 1, Margin = new Thickness(4,0,4,0) };
            apartmentsPanel.Children.Add(ApartmentTextBox()); // inicijalno dodajemo jedan stan

            var apartmentsScroll = new ScrollViewer
            {
                Content = apartmentsPanel,
                HorizontalScrollBarVisibility = ScrollBarVisibility.Auto,
                VerticalScrollBarVisibility = ScrollBarVisibility.Disabled,
                CanContentScroll = false,
                Padding = new Thickness(8, 4, 8, 4),
                Margin = new Thickness(10,10,10,0)
            };
            

            var addButton = AddApartmentsButton();
            addButton.Click += (s, e) =>
            {
                apartmentsPanel.Children.Add(ApartmentTextBox());
                apartmentsPanel.Tag = apartmentsPanel.Children.Count;
            };

            var removeButton = RemoveApartmentButton();
            removeButton.Click += (s, e) =>
            {
                if (apartmentsPanel.Children.Count > 1)
                {
                    apartmentsPanel.Children.RemoveAt(apartmentsPanel.Children.Count - 1);
                    apartmentsPanel.Tag = apartmentsPanel.Children.Count;
                }
            };

            var buttonPanel = new StackPanel { Orientation = Orientation.Horizontal };
            buttonPanel.Children.Add(removeButton);
            buttonPanel.Children.Add(addButton);

            var row = new Grid();
            row.Margin = new Thickness(8);                     
            row.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }); 
            row.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });                      

            
            Grid.SetColumn(apartmentsScroll, 0);
            Grid.SetColumn(buttonPanel, 1);
            row.Children.Add(apartmentsScroll);
            row.Children.Add(buttonPanel);

            groupBox.Content = row;
            return groupBox;
        }
        private TextBox ApartmentTextBox()
        {
            return new TextBox
            {
                Width = 45,
                Height = 45,
                VerticalContentAlignment = VerticalAlignment.Center,
                TextAlignment = TextAlignment.Center,
                Margin = new Thickness(0, 0, 7, 0),
            };
        }
        private Button AddApartmentsButton()
        {
            return new Button
            {
                Content = "+",
                Width = 30,
                Height = 30,
                Margin = new Thickness(4, 0, 0, 0)
            };
        }
        private Button RemoveApartmentButton()
        {
            return new Button
            {
                Content = "-",
                Width = 30,
                Height = 30,
            };
        }

        private void ChooseImage_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Hello");
        }
    }
}