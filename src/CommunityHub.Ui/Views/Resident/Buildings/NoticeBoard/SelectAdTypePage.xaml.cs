using CommunityHub.Application.Domain.NoticeBoard;
using CommunityHub.Ui.Helpers.Navigation;
using System.Windows;
using System.Windows.Input;

namespace CommunityHub.Ui.Views.Resident.Buildings.NoticeBoard
{
    public partial class SelectAdTypePage : MainNavigatorConsumerPage
    {
        public AdType? SelectedType { get; }
        public SelectAdTypePage(AdType? selectedType = null)
        {
            InitializeComponent();
            DataContext = this;

            SelectedType = selectedType;
        }

        private void TypeItem_Click(object sender, MouseButtonEventArgs e)
        {
            if (sender is not FrameworkElement element) return;

            if (element.DataContext is not AdType type) return;
            if (type == SelectedType) return;

            Pop(type);
        }

        private void BackButton_Click(object sender, MouseButtonEventArgs e) => Pop();
    }
}
