using DB_app.Behaviors;
using DB_app.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;

namespace DB_app.Views;


public sealed partial class ProductsGridPage : Page
{
    public ProductsGridViewModel ViewModel { get; }

    public ProductsGridPage()
    {
        ViewModel = App.GetService<ProductsGridViewModel>();
        InitializeComponent();
        SetBinding(NavigationViewHeaderBehavior.HeaderContextProperty, new Binding
        {
            Source = ViewModel,
            Mode = BindingMode.OneWay
        });
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        var givenProduct = (ProductWrapper)e.Parameter;
        if (givenProduct != null)
        {
            if (givenProduct.isNew)
            {
                ViewModel.InsertToGridNewWrapper(givenProduct);
            }
            else if (givenProduct.IsModified)
            {
                ViewModel.UpdateGridWithEditedWrapper(givenProduct);
            }
        }
        base.OnNavigatedTo(e);

    }

    private void AddNewProduct_Click(object sender, RoutedEventArgs e) =>
        Frame.Navigate(typeof(ProductDetailsPage), null, new DrillInNavigationTransitionInfo());

    private void EditExistingProduct_Click(object sender, RoutedEventArgs e)
    {
        Frame.Navigate(typeof(ProductDetailsPage), null, new DrillInNavigationTransitionInfo());
        ViewModel.SendPrikol();
    }
}
