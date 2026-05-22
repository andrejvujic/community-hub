using CommunityHub.Application.Domain.Users;
using CommunityHub.Ui.ViewModels.BuildingManager.Navigation;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace CommunityHub.Ui.Views.BuildingManager
{
    public partial class RegistrationPage : Page
    {
        private readonly RegistrationPageViewModel _viewModel;
        public ObservableCollection<int> ApartmentNumbers { get; }
        private List<(int Number, int Floor)> _apartmentEntries = new();

        public RegistrationPage(User user)
        {
            InitializeComponent();
            _viewModel = new RegistrationPageViewModel(user);
            DataContext = _viewModel;

            ApartmentNumbers = new ObservableCollection<int>();
        }
        private void RegistrationPage_Loaded(object sender, RoutedEventArgs e)
        {
            floorsStackPanel.Children.Add(CreateFloor(1));
        }
        private void FloorsUpDown_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (floorsStackPanel == null) return;

            int newValue = floorsUpDown.Value ?? 1;
            int currentCount = floorsStackPanel.Children.Count;

            if (newValue > currentCount)
                AddFloors(currentCount, newValue);
            else
                RemoveFloors(newValue);

            ValidateApartments();
        }
        private void AddFloors(int from, int to)
        {
            for (int i = from + 1; i <= to; i++)
                floorsStackPanel.Children.Add(CreateFloor(i));
        }
        private void RemoveFloors(int targetCount)
        {
            while (floorsStackPanel.Children.Count > targetCount)
                floorsStackPanel.Children.RemoveAt(floorsStackPanel.Children.Count - 1);
        }
        private GroupBox CreateFloor(int number)
        {
            var apartmentsPanel = CreateApartmentsPanel();
            var contentRow = CreateFloorContentRow(apartmentsPanel);

            var groupBox = new GroupBox
            {
                Header = $"Sprat {number} - stanovi",
                Tag = number,
                FontSize = 14,
                Margin = new Thickness(15, 10, 15, 20),
                Height = 140,
                Content = contentRow
            };

            groupBox.Style = (Style)FindResource("FloorGroupBoxStyle");
            return groupBox;
        }
        private StackPanel CreateApartmentsPanel()
        {
            var panel = new StackPanel { Orientation = Orientation.Horizontal, Tag = 1, Margin = new Thickness(4, 0, 4, 0) };
            panel.Children.Add(CreateApartmentTextBox()); // inicijalno dodajemo jedan stan
            return panel;
        }
        private TextBox CreateApartmentTextBox()
        {
            var textBox = new TextBox
            {
                Width = 45,
                Height = 45,
                VerticalContentAlignment = VerticalAlignment.Center,
                TextAlignment = TextAlignment.Center,
                Margin = new Thickness(0, 0, 7, 0),
            };

            textBox.TextChanged += ApartmentTextBox_TextChanged;
            return textBox;
        }
        private ScrollViewer CreateApartmentsScroll(StackPanel apartmentsPanel)
        {
            return new ScrollViewer
            {
                Content = apartmentsPanel,
                HorizontalScrollBarVisibility = ScrollBarVisibility.Auto,
                VerticalScrollBarVisibility = ScrollBarVisibility.Disabled,
                CanContentScroll = false,
                Padding = new Thickness(8, 4, 8, 4),
                Margin = new Thickness(10, 10, 10, 0)
            };
        }
        private StackPanel CreateApartmentButtonPanel(StackPanel apartmentsPanel)
        {
            var addButton = AddApartmentButton();
            addButton.Click += (s, e) =>
            {
                apartmentsPanel.Children.Add(CreateApartmentTextBox());
                apartmentsPanel.Tag = apartmentsPanel.Children.Count;
                ValidateApartments();
            };

            var removeButton = RemoveApartmentButton();
            removeButton.Click += (s, e) =>
            {
                if (apartmentsPanel.Children.Count > 1)
                {
                    apartmentsPanel.Children.RemoveAt(apartmentsPanel.Children.Count - 1);
                    apartmentsPanel.Tag = apartmentsPanel.Children.Count;
                    ValidateApartments();
                }
            };

            var buttonPanel = new StackPanel { Orientation = Orientation.Horizontal };
            buttonPanel.Children.Add(removeButton);
            buttonPanel.Children.Add(addButton);
            return buttonPanel;
        }
        private Button AddApartmentButton()
        {
            return new Button
            {
                Style = null,
                Content = "+",
                Width = 30,
                Height = 30,
                Margin = new Thickness(4, 0, 0, 0),
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0F5AA6")),
                Foreground = new SolidColorBrush(Colors.White),
                Cursor = Cursors.Hand
            };
        }
        private Button RemoveApartmentButton()
        {
            return new Button
            {
                Style = null,
                Content = "-",
                Width = 30,
                Height = 30,
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0F5AA6")),
                Foreground = new SolidColorBrush(Colors.White),
                Cursor = Cursors.Hand
            };
        }
        private Grid CreateFloorContentRow(StackPanel apartmentsPanel)
        {
            var row = new Grid { Margin = new Thickness(8) };
            row.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            row.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

            var apartmentsScroll = CreateApartmentsScroll(apartmentsPanel);
            var buttonPanel = CreateApartmentButtonPanel(apartmentsPanel);

            Grid.SetColumn(apartmentsScroll, 0);
            Grid.SetColumn(buttonPanel, 1);
            row.Children.Add(apartmentsScroll);
            row.Children.Add(buttonPanel);
            return row;
        }
        private void MarkApartmentTextBox(TextBox textBox, bool isValid)
        {
            textBox.BorderBrush = isValid ? SystemColors.ControlDarkBrush : new SolidColorBrush((Color)ColorConverter.ConvertFromString("#C43D5A")); ;
            textBox.BorderThickness = isValid ? new Thickness(1) : new Thickness(2);
        }
        private bool ValidateApartmentTextBox(TextBox textBox)
        {
            var text = textBox.Text?.Trim() ?? string.Empty;

            if (string.IsNullOrEmpty(text))
            {
                MarkApartmentTextBox(textBox, false);
                return false;
            }

            if (!int.TryParse(text, out var number))
            {
                MarkApartmentTextBox(textBox, false);
                return false;
            }

            if (number < 1)
            {
                MarkApartmentTextBox(textBox, false);
                return false;
            }

            MarkApartmentTextBox(textBox, true);
            return true;
        }
        private bool ValidateApartments()
        {
            ApartmentNumbers.Clear();
            _apartmentEntries.Clear();

            bool allValid = true;

            foreach (GroupBox floorGroup in floorsStackPanel.Children)
            {
                if (floorGroup.Content is Grid grid &&
                    grid.Children.OfType<ScrollViewer>().FirstOrDefault()?.Content is StackPanel apartmentsPanel)
                {
                    int floorNumber = (int)floorGroup.Tag;

                    foreach (TextBox apartmentTextBox in apartmentsPanel.Children.OfType<TextBox>())
                    {
                        if(!ProcessApartmentTextBox(apartmentTextBox, floorNumber))
                            allValid = false;
                    }
                }
            }

            if (!ValidateApartmentNumbersUnique())
                allValid = false;

            _viewModel.AreApartmentsValid = allValid;
            return allValid;
        }
        private bool ProcessApartmentTextBox(TextBox apartmentTextBox, int floorNumber)
        {
            if (!ValidateApartmentTextBox(apartmentTextBox))
                return false;

            int num = int.Parse(apartmentTextBox.Text.Trim());
            ApartmentNumbers.Add(num);
            _apartmentEntries.Add((Number: num, Floor: floorNumber));

            return true;
        }
        private bool ValidateApartmentNumbersUnique()
        {
            bool unique = true;

            foreach (GroupBox floorGroup in floorsStackPanel.Children)
            {
                if (floorGroup.Content is Grid grid &&
                    grid.Children.OfType<ScrollViewer>().FirstOrDefault()?.Content is StackPanel apartmentsPanel)
                {
                    foreach (TextBox apartmentTextBox in apartmentsPanel.Children.OfType<TextBox>())
                    {
                        var text = apartmentTextBox.Text?.Trim();

                        if (!int.TryParse(text, out var current))
                            continue;

                        int count = ApartmentNumbers.Count(n => n == current);
                        if (count > 1)
                        {
                            MarkApartmentTextBox(apartmentTextBox, false);
                            unique = false;
                        }
                    }
                }
            }

            return unique;
        }
        private void ApartmentTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ValidateApartments();
        }
        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            if (!_viewModel.CanConfirm)
                return;

            if (!ValidateApartments())
                return;

            bool success = _viewModel.ConfirmRegistration(
                floorsUpDown.Value ?? 1,
                _apartmentEntries
            );

            if (!success)
                return;

            HomeWindow? homeWindow = Window.GetWindow(this) as HomeWindow;

            if (homeWindow != null)
                homeWindow.HomeFrame.Navigate(new MyBuildingsPage(_viewModel.User));
        }
    }
}