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


public sealed partial class HospitalsGridPage : Page
{
    public HospitalsGridViewModel ViewModel { get; }


    public HospitalsGridPage()
    {
        ViewModel = App.GetService<HospitalsGridViewModel>();
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


    private void ShowNotificationMessage(object? _, ListEventArgs e)
    {
        var message = e.Data[0];
    }


    private void Add_Click(object? _, RoutedEventArgs e) =>
        App.GetService<INavigationService>().NavigateTo(typeof(HospitalDetailsViewModel).FullName!, new AddressWrapper() { IsNew = true, IsInEdit = true });


    private void View_Click(object? _, RoutedEventArgs e) =>
        App.GetService<INavigationService>().NavigateTo(typeof(HospitalDetailsViewModel).FullName!, ViewModel.SelectedItem);


    private async void Delete_Click(object? _, RoutedEventArgs e) =>
        await ViewModel.DeleteSelected();


    private void Edit_Click(object? _, RoutedEventArgs e)
    {
        if (ViewModel.SelectedItem == null) return;

        ViewModel.SelectedItem.IsInEdit = true;
        App.GetService<INavigationService>().NavigateTo(typeof(HospitalDetailsViewModel).FullName!, ViewModel.SelectedItem);
    }

    private async void Button_Click(object sender, RoutedEventArgs e)
        => await ViewModel.ToggleInactive();
}
