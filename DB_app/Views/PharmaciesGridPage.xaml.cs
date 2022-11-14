using DB_app.ViewModels;

using Microsoft.UI.Xaml.Controls;

namespace DB_app.Views;


public sealed partial class PharmaciesGridPage : Page
{
    public PharmaciesGridViewModel ViewModel
    {
        get;
    }

    public PharmaciesGridPage()
    {
        ViewModel = App.GetService<PharmaciesGridViewModel>();
        InitializeComponent();
    }
}
