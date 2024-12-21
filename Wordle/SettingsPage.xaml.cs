using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;
using Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;
using Microsoft.Maui.Storage;

namespace Wordle
{
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            InitializeComponent(); var savedTheme = Preferences.Get("app_theme", "Light Mode");
            ThemePicker.SelectedItem = savedTheme;

            var savedFontSize = Preferences.Get("font_size", 14.0);
            FontSizeSlider.Value = savedFontSize;
            FontSizeLabel.Text = $"Font Size: {savedFontSize}";
        }

        private void OnThemeChanged(object sender, EventArgs e)
        {
            var selectedTheme = ThemePicker.SelectedItem.ToString();
            ApplyTheme(selectedTheme);
            Preferences.Set("app_theme", selectedTheme);
        }

        private void OnFontSizeChanged(object sender, ValueChangedEventArgs e)
        {
            FontSizeLabel.Text = $"Font Size: {e.NewValue}";
            Preferences.Set("font_size", e.NewValue);
        }

        private void ApplyTheme(string theme)
        {
            Microsoft.Maui.Controls.Application.Current.UserAppTheme = theme == "Dark Mode" ? AppTheme.Dark : AppTheme.Light;
        }
    }
}