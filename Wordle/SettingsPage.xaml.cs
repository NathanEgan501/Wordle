using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;

namespace Wordle
{
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            InitializeComponent();

            var savedTheme = Preferences.Get("app_theme", "Light Mode");
            ThemePicker.SelectedItem = savedTheme;
        }

        private void OnThemeChanged(object sender, EventArgs e)
        {
            var selectedTheme = ThemePicker.SelectedItem.ToString();
            ApplyTheme(selectedTheme);
            Preferences.Set("app_theme", selectedTheme); 
        }

        private void ApplyTheme(string theme)
        {
            if (theme == "Dark Mode")
            {
                Application.Current.UserAppTheme = AppTheme.Dark;
            }
            else
            {
                Application.Current.UserAppTheme = AppTheme.Light;
            }
        }
    }
}