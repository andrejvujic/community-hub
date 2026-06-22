using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using CommunityHub.Ui.Resources.Localization;
using CommunityHub.Ui.ViewModels.NeighborhoodCoordinator.Tabs;

namespace CommunityHub.Ui.Views.NeighborhoodCoordinator.Tabs
{
    public partial class NeighborhoodBudgetTab : UserControl
    {
        private readonly NeighborhoodBudgetViewModel _viewModel;

        public NeighborhoodBudgetTab()
        {
            InitializeComponent();
            Language = XmlLanguage.GetLanguage("sr-Latn-RS");

            _viewModel = new NeighborhoodBudgetViewModel();
            DataContext = _viewModel;

            ExpenseDatePicker.SelectedDate = DateTime.Now;
            this.IsVisibleChanged += NeighborhoodBudgetTab_IsVisibleChanged;

            TranslationSource.Instance.PropertyChanged += (s, e) =>
            {
                if (this.IsVisible)
                {
                    RefreshLanguageAndBindings();
                }
            };
        }

        public void Initialize(long coordinatorId)
        {
            _viewModel.Initialize(coordinatorId);
        }

        private void NeighborhoodBudgetTab_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
            {
                RefreshLanguageAndBindings();
                _viewModel.LoadNeighborhoods();
            }
        }

        private void RefreshLanguageAndBindings()
        {
            var currentCultureName = TranslationSource.Instance.CurrentCulture?.Name ?? "sr-Latn-RS";
            this.Language = XmlLanguage.GetLanguage(currentCultureName);

            BudgetCategoriesDataGrid?.Items.Refresh();
            ExpensesDataGrid?.Items.Refresh();
            DonationsDataGrid?.Items.Refresh();
        }

        private void AmountTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var textBox = (TextBox)sender;

            bool isEnglish = TranslationSource.Instance.CurrentCulture?.Name
                .StartsWith("en", StringComparison.OrdinalIgnoreCase) ?? false;
            char thousandsSeparator = isEnglish ? ',' : '.';
            char decimalSeparator = isEnglish ? '.' : ',';

            var textWithoutSelection = textBox.Text.Remove(textBox.SelectionStart, textBox.SelectionLength);
            var alreadyHasDecimalSeparator = textWithoutSelection.Contains(decimalSeparator);

            foreach (var ch in e.Text)
            {
                if (char.IsDigit(ch)) continue;

                if (ch == decimalSeparator)
                {
                    if (alreadyHasDecimalSeparator)
                    {
                        e.Handled = true;
                        return;
                    }
                    alreadyHasDecimalSeparator = true;
                    continue;
                }

                if (ch == thousandsSeparator)
                {
                    if (alreadyHasDecimalSeparator)
                    {
                        e.Handled = true;
                        return;
                    }
                    continue;
                }

                e.Handled = true;
                return;
            }
        }

        private void AmountTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.V && (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)))
                e.Handled = true;
        }
    }
}