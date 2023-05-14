using DB_app.ViewModels;
using Microsoft.UI.Xaml.Controls;

namespace DB_app.Views;


public sealed partial class PharmacyReportGridPage : Page
{
    public PharmacyReportGridViewModel ViewModel { get; }= App.GetService<PharmacyReportGridViewModel>();

    public PharmacyReportGridPage()
    {
        InitializeComponent();
    }
}
