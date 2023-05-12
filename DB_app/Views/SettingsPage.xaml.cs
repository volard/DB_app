using DB_app.ViewModels;

using Microsoft.UI.Xaml.Controls;

namespace DB_app.Views;


public sealed partial class SettingsPage : Page
{
    public SettingsViewModel ViewModel { get; } = App.GetService<SettingsViewModel>();

    public SettingsPage()
    {
        InitializeComponent();
    }
}
