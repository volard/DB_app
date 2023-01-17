using AppUIBasics.Helper;
using DB_app.Behaviors;
using DB_app.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;

namespace DB_app.Views;


public sealed partial class OrdersGridPage : Page
{
    public OrdersGridViewModel ViewModel { get; }

    public OrdersGridPage()
    {
        ViewModel = App.GetService<OrdersGridViewModel>();
        InitializeComponent();
        SetBinding(NavigationViewHeaderBehavior.HeaderContextProperty, new Binding
        {
            Source = ViewModel,
            Mode = BindingMode.OneWay
        });
        
    }

    

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        var givenOrder = (OrderWrapper)e.Parameter;
        if (givenOrder != null)
        {
            if (givenOrder.isNew)
            {
                ViewModel.InsertToGridNewWrapper(givenOrder);
            }
            else if (givenOrder.IsModified)
            {
                ViewModel.UpdateGridWithEditedWrapper(givenOrder);
            }
        }
        base.OnNavigatedTo(e);

    }

    private void AddNewOrder_Click(object sender, RoutedEventArgs e) =>
        Frame.Navigate(typeof(OrderDetailsPage), null, new DrillInNavigationTransitionInfo());

    private void EditExistingOrder_Click(object sender, RoutedEventArgs e)
    {
        Frame.Navigate(typeof(OrderDetailsPage), null, new DrillInNavigationTransitionInfo());
        ViewModel.SendPrikol();
    }
}
