using DB_app.ViewModels;

using Microsoft.UI.Xaml.Controls;

namespace DB_app.Views;

public sealed partial class GreetingPage : Page
{
    public GreetingViewModel ViewModel
    {
        get;
    }

    public GreetingPage()
    {
        ViewModel = App.GetService<GreetingViewModel>();
        InitializeComponent();
    }
}
