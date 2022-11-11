using DB_app.ViewModels;

using Microsoft.UI.Xaml.Controls;

namespace DB_app.Views;

// TODO: Change the grid as appropriate for your app. Adjust the column definitions on DataGridPage.xaml.
// For more details, see the documentation at https://docs.microsoft.com/windows/communitytoolkit/controls/datagrid.
public sealed partial class HospitalReportGridPage : Page
{
    public HospitalReportGridViewModel ViewModel
    {
        get;
    }

    public HospitalReportGridPage()
    {
        ViewModel = App.GetService<HospitalReportGridViewModel>();
        InitializeComponent();
    }
}
