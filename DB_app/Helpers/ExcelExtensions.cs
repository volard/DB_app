using ClosedXML.Excel;
using CommunityToolkit.WinUI.UI.Controls;
using System.Diagnostics;
using Windows.Storage.Pickers;
using Windows.Storage.Provider;
using Windows.Storage;

namespace DB_app.Helpers;

public class ExcelExtensions
{

    public static async Task ExportAsExcel(
        DataGrid dataGrid,
        Dictionary<string, List<string>> collection,
        string fileName = "Report")
    {
        FileSavePicker savePicker = new();

        // Retrieve the window handle (HWND) of the current WinUI 3 window.
        //var window = WindowHelper.GetWindowForElement(this);

        IntPtr hWnd = WinRT.Interop.WindowNative.GetWindowHandle(App.MainWindow);

        // Initialize the file picker with the window handle (HWND)
        WinRT.Interop.InitializeWithWindow.Initialize(savePicker, hWnd);

        // Set options
        savePicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
        // Dropdown of file types the user can save the file as
        savePicker.FileTypeChoices.Add("Excel document", new List<string>() { ".xlsx" });
        // Default file name
        savePicker.SuggestedFileName = fileName;

        // Open the picker for the user to pick a file
        StorageFile file = await savePicker.PickSaveFileAsync();
        if (file != null)
        {
            // Prevent updates to the remote version of the file until we finish making changes and call CompleteUpdatesAsync.
            CachedFileManager.DeferUpdates(file);

            XLWorkbook workbook = new();
            IXLWorksheet? worksheet = workbook.Worksheets.Add("Main");

            // row X column

            // Create header
            int column = 1;
            foreach (string header in dataGrid.Columns.Select(header => header.Header.ToString() ?? " ").ToList())
            {
                worksheet.Cell(1, column).Value = header;
                worksheet.Cell(1, column).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                worksheet.Cell(1, column).Style.Font.Bold = true;
                ++column;
            }

            // Add data content
            int row = 2;
            foreach(string key in collection.Keys)
            {
                worksheet.Cell(row, 1).Value = key;
                worksheet.Cell(row, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                worksheet.Cell(row, 1).Style.Font.Italic = true;
                worksheet.Range(row, 1, row, dataGrid.Columns.Count).Merge();

                ++row;

                worksheet.Cell(row, 1).InsertData(collection[key]);

                ++row;
            }

            worksheet.Columns().AdjustToContents();

            // Save file
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


        public static async Task ExportAsExcel(
        DataGrid dataGrid, 
        List<List<string>> collection, 
        string fileName = "Report")
    {
        FileSavePicker savePicker = new();

        // Retrieve the window handle (HWND) of the current WinUI 3 window.
        //var window = WindowHelper.GetWindowForElement(this);

        IntPtr hWnd = WinRT.Interop.WindowNative.GetWindowHandle(App.MainWindow);

        // Initialize the file picker with the window handle (HWND)
        WinRT.Interop.InitializeWithWindow.Initialize(savePicker, hWnd);

        // Set options
        savePicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
        // Dropdown of file types the user can save the file as
        savePicker.FileTypeChoices.Add("Excel document", new List<string>() { ".xlsx" });
        // Default file name
        savePicker.SuggestedFileName = fileName;

        // Open the picker for the user to pick a file
        StorageFile file = await savePicker.PickSaveFileAsync();
        if (file != null)
        {
            // Prevent updates to the remote version of the file until we finish making changes and call CompleteUpdatesAsync.
            CachedFileManager.DeferUpdates(file);

            XLWorkbook workbook = new();
            IXLWorksheet? worksheet = workbook.Worksheets.Add("Main");

            // row X column

            // Create header
            int i = 1;
            foreach (string header in dataGrid.Columns.Select(header => header.Header.ToString() ?? " ").ToList())
            {
                worksheet.Cell(1, i).Value = header;
                worksheet.Cell(1, i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                worksheet.Cell(1, i).Style.Font.Bold = true;
                ++i;
            }

            // Add data content
            worksheet.Cell(2, 1).InsertData(collection);

            worksheet.Columns().AdjustToContents();

            // Save file
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
