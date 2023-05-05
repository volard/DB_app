using DB_app.Behaviors;
using Microsoft.UI.Xaml;
using DB_app.Helpers;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;
using DB_app.Contracts.Services;
using DB_app.ViewModels;

namespace DB_app.Views;


public sealed partial class OrdersGridPage : Page
{
    public OrdersGridViewModel ViewModel { get; } = App.GetService<OrdersGridViewModel>();

    public OrdersGridPage()
    {
        InitializeComponent();
        SetBinding(NavigationViewHeaderBehavior.HeaderContextProperty, new Binding
        {
            Source = ViewModel,
            Mode = BindingMode.OneWay
        });
        
    }

    private void ShowNotificationMessage(object? sender, ListEventArgs e)
    {
        var message = e.Data[0];
        Notification.Content = message;
        Notification.Show(2000);
    }

    #region INavigationAware implementation

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        ViewModel.OperationRejected += ShowNotificationMessage;
        base.OnNavigatedTo(e);
    }

    protected override void OnNavigatedFrom(NavigationEventArgs e)
    {
        ViewModel.OperationRejected-= ShowNotificationMessage;
        base.OnNavigatedFrom(e);
    }

    #endregion



    #region Button handlers


    private void AddButton_Click(object sender, RoutedEventArgs e) =>
        App.GetService<INavigationService>().NavigateTo(typeof(OrderDetailsViewModel).FullName!, new OrderWrapper() { IsInEdit = true });

    private void ViewButton_Click(object sender, RoutedEventArgs e) =>
        Frame.Navigate(typeof(OrderDetailsPage), ViewModel, new DrillInNavigationTransitionInfo());



    private async void DeleteButton_Click(object sender, RoutedEventArgs e) =>
        await ViewModel.DeleteSelected();


    private void EditButton_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.SelectedItem.IsInEdit = true;
        App.GetService<INavigationService>().NavigateTo(typeof(OrderDetailsViewModel).FullName!, ViewModel.SelectedItem);
    }

    #endregion


}
