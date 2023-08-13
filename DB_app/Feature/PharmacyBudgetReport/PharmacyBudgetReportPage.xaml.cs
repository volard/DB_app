using DB_app.ViewModels;
using Microsoft.UI.Xaml.Controls;
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

    protected override async void OnNavigatedTo(NavigationEventArgs e)
    {
        if (ViewModel.Source.Count >= 1) { return; }

        await ViewModel.LoadReport();

        if (ViewModel.Source.Count == 0)
        {
            SourceDataGrid.Visibility = Microsoft.UI.Xaml.Visibility.Collapsed;
            NotFoundBlock.Visibility = Microsoft.UI.Xaml.Visibility.Visible;
        }
        else
        {
            SourceDataGrid.Visibility = Microsoft.UI.Xaml.Visibility.Visible;
            NotFoundBlock.Visibility = Microsoft.UI.Xaml.Visibility.Collapsed;
        }

        base.OnNavigatedTo(e);
    }
}
