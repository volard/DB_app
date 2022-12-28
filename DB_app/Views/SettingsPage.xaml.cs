using DB_app.ViewModels;

using Microsoft.UI.Xaml.Controls;

namespace DB_app.Views;


public sealed partial class SettingsPage : Page
{
    // TODO Add settigns panes to enhance UI with modern stuff
    public SettingsViewModel ViewModel
    {
        get;
    }

    public SettingsPage()
    {
        ViewModel = App.GetService<SettingsViewModel>();
        InitializeComponent();
    }
}
