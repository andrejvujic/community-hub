using System.Globalization;
using System.Windows.Markup;
using CommunityHub.Ui.Resources.Localization;

namespace CommunityHub.Ui
{
    public partial class App : System.Windows.Application
    {
        public void ChangeLanguage(string lang)
        {
            var culture = new System.Globalization.CultureInfo(lang);
            TranslationSource.Instance.CurrentCulture = culture;

            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;

            if (MainWindow != null)
                MainWindow.Language = XmlLanguage.GetLanguage(culture.IetfLanguageTag);
        }
    }
}