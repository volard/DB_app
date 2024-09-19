using DB_app.ViewModels;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using Windows.Storage;
using Windows.Storage.Pickers;

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

    private async void CommandBarExportButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        FileSavePicker savePicker = new FileSavePicker();

        // Retrieve the window handle (HWND) of the current WinUI 3 window.
        //var window = WindowHelper.GetWindowForElement(this);

        IntPtr hWnd = WinRT.Interop.WindowNative.GetWindowHandle(App.MainWindow);

        // Initialize the file picker with the window handle (HWND).
        WinRT.Interop.InitializeWithWindow.Initialize(savePicker, hWnd);

        // Set options
        savePicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
        // Dropdown of file types the user can save the file as
        savePicker.FileTypeChoices.Add("Excel document", new List<string>() { ".xlsx" });
        // Default file name
        savePicker.SuggestedFileName = "Report";

        // Open the picker for the user to pick a file
        StorageFile file = await savePicker.PickSaveFileAsync();
        if (file != null)
        {
            // Prevent updates to the remote version of the file until we finish making changes and call CompleteUpdatesAsync.
            CachedFileManager.DeferUpdates(file);
        }
    }
}
