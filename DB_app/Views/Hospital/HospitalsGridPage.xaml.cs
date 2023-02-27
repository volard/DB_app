using DB_app.Behaviors;
using DB_app.Contracts.Services;
using DB_app.Helpers;
using DB_app.Services;
using DB_app.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;
using System.Diagnostics;

namespace DB_app.Views;


public sealed partial class HospitalGridPage : Page
{
    public HospitalsGridViewModel ViewModel { get; }

    public HospitalGridPage()
    {
        ViewModel = App.GetService<HospitalsGridViewModel>();
        InitializeComponent();
        SetBinding(NavigationViewHeaderBehavior.HeaderContextProperty, new Binding
        {
            Source = ViewModel,
            Mode = BindingMode.OneWay
        });
    }

    private void InitializeComponent()
    {
        throw new NotImplementedException();
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

    private void ShowNotificationMessage(object? _, ListEventArgs e)
    {
        var message = e.Data[0];
    }

    private void Add_Click(object? _, RoutedEventArgs e) =>
        Frame.Navigate(typeof(AddressDetailsPage), new AddressWrapper() { IsInEdit = true }, new DrillInNavigationTransitionInfo());


    private void View_Click(object? _, RoutedEventArgs e) =>
        Frame.Navigate(typeof(AddressDetailsPage), ViewModel.SelectedItem, new DrillInNavigationTransitionInfo());



    private async void Delete_Click(object? _, RoutedEventArgs e) =>
        await ViewModel.DeleteSelected();


    private void Edit_Click(object? _, RoutedEventArgs e)
    {
        ViewModel.SelectedItem!.IsInEdit = true;
        App.GetService<INavigationService>().NavigateTo(typeof(AddressDetailsViewModel).FullName!, ViewModel.SelectedItem);
    }
}
