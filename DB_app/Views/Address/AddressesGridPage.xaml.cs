using DB_app.Behaviors;
using DB_app.Contracts.Services;
using DB_app.Services;
using DB_app.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;

namespace DB_app.Views;


public sealed partial class AddressesGridPage : Page
{
    public AddressesGridViewModel ViewModel { get; }

    public AddressesGridPage()
    {
        ViewModel = App.GetService<AddressesGridViewModel>();
        InitializeComponent();
        SetBinding(NavigationViewHeaderBehavior.HeaderContextProperty, new Binding
        {
            Source = ViewModel,
            Mode = BindingMode.OneWay
        });
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        ViewModel.D
        base.OnNavigatedTo(e);
    }

    private void Add_Click(object sender, RoutedEventArgs e) =>
        Frame.Navigate(typeof(AddressDetailsPage), new AddressWrapper() { IsInEdit = true }, new DrillInNavigationTransitionInfo());


    private void View_Click(object sender, RoutedEventArgs e) =>
        Frame.Navigate(typeof(AddressDetailsPage), ViewModel.SelectedItem, new DrillInNavigationTransitionInfo());



    private async void Delete_Click(object sender, RoutedEventArgs e)
    {
        await ViewModel.DeleteSelected();
        Notification.Show();
    }


    private void Edit_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.SelectedItem!.IsInEdit = true;
        App.GetService<INavigationService>().NavigateTo(typeof(AddressDetailsViewModel).FullName!, ViewModel.SelectedItem);
    }
}
