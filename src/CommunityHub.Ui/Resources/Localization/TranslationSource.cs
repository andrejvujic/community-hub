using System.ComponentModel;
using System.Globalization;
using System.Resources;
using System.Windows.Data;

namespace CommunityHub.Ui.Resources.Localization
{
    public class TranslationSource : INotifyPropertyChanged
    {
        private static readonly TranslationSource instance = new TranslationSource();
        public static TranslationSource Instance
        {
            get { return instance; }
        }

        private readonly ResourceManager resourceManager = Resources.ResourceManager;
        private CultureInfo currentCulture = null;

        public string this[string key]
        {
            get
            {
                string retVal = this.resourceManager.GetString(key, this.currentCulture);
                return retVal ?? $"[{key}]";
            }
        }

        public string Format(string key, params object[] args)
        {
            var template = this[key].Replace("\\n", "\n");
            return string.Format(template, args);
        }

        public string FormatDate(DateTime date)
        {
            return currentCulture?.Name == "en-US"
                ? date.ToString("MM/dd/yyyy", currentCulture)
                : date.ToString("dd.MM.yyyy.", currentCulture);
        }

        public string FormatDate(DateOnly date)
        {
            return currentCulture?.Name == "en-US"
                ? date.ToString("MM/dd/yyyy", currentCulture)
                : date.ToString("dd.MM.yyyy.", currentCulture);
        }

        public string FormatDate(DateOnly? date)
        {
            return date.HasValue ? FormatDate(date.Value) : string.Empty;
        }

        public string FormatDateTime(DateTime date)
        {
            return currentCulture?.Name == "en-US"
                ? date.ToString("MM/dd/yyyy hh:mm tt", currentCulture)
                : date.ToString("dd.MM.yyyy. HH:mm", currentCulture);
        }

        public string FormatTime(TimeOnly time)
        {
            return currentCulture?.Name == "en-US"
                ? time.ToString("hh:mm tt", currentCulture)
                : time.ToString("HH:mm", currentCulture);
        }

        public CultureInfo CurrentCulture
        {
            get { return this.currentCulture; }
            set
            {
                if (this.currentCulture != value)
                {
                    this.currentCulture = value;
                    var @event = this.PropertyChanged;
                    if (@event != null)
                        @event.Invoke(this, new PropertyChangedEventArgs(string.Empty));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class LocExtension : Binding
    {
        public LocExtension(string name) : base("[" + name + "]")
        {
            this.Mode = BindingMode.OneWay;
            this.Source = TranslationSource.Instance;
        }
    }
}