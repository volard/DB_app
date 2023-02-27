using DB_app.Behaviors;
using DB_app.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using DB_app.Helpers;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;
using DB_app.Contracts.Services;

namespace DB_app.Views;


public sealed partial class PharmaciesGridPage : Page
{
    public PharmaciesGridViewModel ViewModel { get; }

    public PharmaciesGridPage()
    {
        ViewModel = App.GetService<PharmaciesGridViewModel>();
        InitializeComponent();
        SetBinding(NavigationViewHeaderBehavior.HeaderContextProperty, new Binding
        {
            Source = ViewModel,
            Mode = BindingMode.OneWay
        });
    }

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

    private void ShowNotificationMessage(object? sender, ListEventArgs e)
    {
        var message = e.Data[0];
        Notification.Content = message;
        Notification.Show(2000);
    }

    private void Add_Click(object sender, RoutedEventArgs e) =>
        Frame.Navigate(typeof(AddressDetailsPage), new AddressWrapper() { IsInEdit = true }, new DrillInNavigationTransitionInfo());


    private void View_Click(object sender, RoutedEventArgs e) =>
        Frame.Navigate(typeof(AddressDetailsPage), ViewModel.SelectedItem, new DrillInNavigationTransitionInfo());



    private async void Delete_Click(object sender, RoutedEventArgs e) =>
        await ViewModel.DeleteSelected();


    private void Edit_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.SelectedItem!.IsInEdit = true;
        App.GetService<INavigationService>().NavigateTo(typeof(AddressDetailsViewModel).FullName!, ViewModel.SelectedItem);
    }
}
