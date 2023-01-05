using DB_app.ViewModels;
using Microsoft.UI.Xaml.Controls;

namespace DB_app.Views;


public sealed partial class ProductsGridPage : Page
{
    public ProductsGridViewModel ViewModel
    {
        get;
    }

    public ProductsGridPage()
    {
        ViewModel = App.GetService<ProductsGridViewModel>();
        InitializeComponent();
    }
}
