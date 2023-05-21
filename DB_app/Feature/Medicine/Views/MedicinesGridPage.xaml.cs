using DB_app.Behaviors;
using DB_app.ViewModels;
using Microsoft.UI.Xaml;
using DB_app.Helpers;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;
using DB_app.Contracts.Services;
using Windows.Storage.Pickers;
using Windows.Storage.Provider;
using Windows.Storage;
using System.Diagnostics;
using ClosedXML.Excel;
using CommunityToolkit.WinUI.UI.Controls;

namespace DB_app.Views;


public sealed partial class MedicinesGridPage
{
    public MedicinesGridViewModel ViewModel { get; }

    public MedicinesGridPage()
    {
        ViewModel = App.GetService<MedicinesGridViewModel>();
        InitializeComponent();
        SetBinding(NavigationViewHeaderBehavior.HeaderContextProperty, new Binding
        {
            Source = ViewModel,
            Mode = BindingMode.OneWay
        });
    }


   protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        ViewModel.DisplayNotification += ShowNotificationMessage;
        base.OnNavigatedTo(e);
    }

    protected override void OnNavigatedFrom(NavigationEventArgs e)
    {
        ViewModel.DisplayNotification-= ShowNotificationMessage;
        base.OnNavigatedFrom(e);
    }

    private void ShowNotificationMessage(object? sender, NotificationConfigurationEventArgs e)
    {
        Notification.Content = e.Message;
        Notification.Style = e.Style;
        Notification.Show(2000);
    }

    private void Add_Click(object sender, RoutedEventArgs e) =>
        Frame.Navigate(typeof(MedicineDetailsPage), new MedicineWrapper() { IsInEdit = true }, new DrillInNavigationTransitionInfo());


    private void View_Click(object sender, RoutedEventArgs e) =>
        Frame.Navigate(typeof(MedicineDetailsPage), ViewModel.SelectedItem, new DrillInNavigationTransitionInfo());



    private async void Delete_Click(object sender, RoutedEventArgs e) =>
        await ViewModel.DeleteSelected();


    private void Edit_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.SelectedItem!.IsInEdit = true;
        App.GetService<INavigationService>().NavigateTo(typeof(MedicineDetailsViewModel).FullName!, ViewModel.SelectedItem);
    }

    private async void CommandBarExportButton_Click(object sender, RoutedEventArgs e)
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

            XLWorkbook workbook = new XLWorkbook();
            IXLWorksheet? worksheet = workbook.Worksheets.Add("Main");

            List<string> headers = new List<string>();

            foreach (DataGridColumn? col in MedicineGrid.Columns)
            {
                headers.Add(col.Header.ToString() ?? " ");
            }

            // row X column
            worksheet.Cell(1, 1).InsertData(headers);
            List<List<string>> data = new List<List<string>>();

            for (int row = 0; row < ViewModel.Source.Count; row++) // for every row
            {
                List<string> line = new List<string>();
                for (int col = 0; col < MedicineGrid.Columns.Count; col++) // for every col of current row
                {
                    string value = (MedicineGrid.Columns.ElementAt(col).GetCellContent(ViewModel.Source.ElementAt(row)) as TextBlock)?.Text ?? " ";
                    line.Add(value);
                }
                data.Add(line);
            }

            worksheet.Cell(2, 1).InsertData(data);
            workbook.SaveAs(file.Path);

            // Let Windows know that we're finished changing the file so the other app can update the remote version of the file.
            // Completing updates may require Windows to ask for user input.
            FileUpdateStatus status = await CachedFileManager.CompleteUpdatesAsync(file);
            if (status == FileUpdateStatus.Complete)
            {
                Debug.WriteLine("Success - file saved under default name");
            }
            else if (status == FileUpdateStatus.CompleteAndRenamed)
            {
                Debug.WriteLine($"Success - file saved under '{file.Name}' name");
            }
            else
            {
                Debug.WriteLine($"Error - {status}");
            }
        }
        else
        {
            Debug.WriteLine("Operation cancelled.");
        }
    }
}
