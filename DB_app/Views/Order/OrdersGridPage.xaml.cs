using DB_app.ViewModels;
using Microsoft.UI.Xaml.Controls;

namespace DB_app.Views;


public sealed partial class OrdersGridPage : Page
{
    public OrdersGridViewModel ViewModel
    {
        get;
    }

    public OrdersGridPage()
    {
        ViewModel = App.GetService<OrdersGridViewModel>();
        InitializeComponent();
    }
}
