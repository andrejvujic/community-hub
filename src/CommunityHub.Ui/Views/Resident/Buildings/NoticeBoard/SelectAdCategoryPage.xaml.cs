using CommunityHub.Application.Domain.NoticeBoard;
using CommunityHub.Ui.Helpers.Navigation;
using System.Windows;
using System.Windows.Input;

namespace CommunityHub.Ui.Views.Resident.Buildings.NoticeBoard
{
    public partial class SelectAdCategoryPage : MainNavigatorConsumerPage
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

            if (element.DataContext is not AdCategory selectedCategory) return;

            Pop(selectedCategory);
        }

        private void BackButton_Click(object sender, MouseButtonEventArgs e) => Pop();
    }
}
