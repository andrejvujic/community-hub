using CommunityHub.Application.Domain.Ads;
using CommunityHub.Ui.Helpers.Navigation;
using System.Windows;
using System.Windows.Input;

namespace CommunityHub.Ui.Views.Resident.Buildings.Ads.Create
{
    public partial class SelectAdCategoryPage : MainNavigationConsumerPage
    {
        public AdCategory? SelectedCategory { get; }

        public SelectAdCategoryPage(AdCategory? selectedCategory = null)
        {
            InitializeComponent();
            DataContext = this;

            SelectedCategory = selectedCategory;
        }

        private void CategoryItem_Click(object sender, MouseButtonEventArgs e)
        {
            if (sender is not FrameworkElement element) return;

            if (element.DataContext is not AdCategory category) return;
            if (category == SelectedCategory) return;

            Pop(category);
        }

        private void BackButton_Click(object sender, MouseButtonEventArgs e) => Pop();
    }
}
