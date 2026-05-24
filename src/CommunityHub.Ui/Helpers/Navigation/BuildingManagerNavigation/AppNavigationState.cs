using CommunityHub.Application.Domain.Buildings;

namespace CommunityHub.Ui.Navigation
{
    public class AppNavigationState
    {
        public string PageTag { get; set; } = "";
        public Building? Building { get; set; }
        public string? SectionTag { get; set; }
    }
}