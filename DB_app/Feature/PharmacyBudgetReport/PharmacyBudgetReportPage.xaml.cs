using DB_app.Behaviors;
using DB_app.ViewModels;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Navigation;


namespace DB_app.Views;

public sealed partial class PharmacyBudgetReportPage : Page
{
    public PharmacyBudgetReportViewModel ViewModel { get; } = App.GetService<PharmacyBudgetReportViewModel>();

    public PharmacyBudgetReportPage()
    {
        InitializeComponent();
        //SetBinding(NavigationViewHeaderBehavior.HeaderContextProperty, new Binding
        //{
        //    Source = ViewModel,
        //    Mode = BindingMode.OneWay
        //});
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        if (ViewModel.Source.Count >= 1) { return; }

        ViewModel.LoadReport();

        base.OnNavigatedTo(e);
    }
}
