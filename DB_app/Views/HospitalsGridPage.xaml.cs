using DB_app.ViewModels;

using Microsoft.UI.Xaml.Controls;

namespace DB_app.Views;


public sealed partial class HospitalsGridPage : Page
{
    public HospitalsGridViewModel ViewModel
    {
        get;
    }

    public HospitalsGridPage()
    {
        ViewModel = App.GetService<HospitalsGridViewModel>();
        InitializeComponent();
    }
}
