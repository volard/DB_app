using DB_app.Behaviors;
using DB_app.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;
using DB_app.ViewModels.ObjectWrappers;

namespace DB_app.Views;


public sealed partial class MedicinesGridPage : Page
{
    public MedicinesGridViewModel ViewModel{ get; }

    public MedicinesGridPage()
    {
        ViewModel = App.GetService<MedicinesGridViewModel>();
        InitializeComponent();
        SetBinding(NavigationViewHeaderBehavior.HeaderContextProperty, new Binding
        {
            Source = ViewModel,
            Mode = BindingMode.OneWay
        });
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        var modifiedMedicine = (MedicineWrapper)e.Parameter;
        if (modifiedMedicine != null)
        {
            ViewModel.SyncDataGridWithModified(modifiedMedicine);
        }
        base.OnNavigatedTo(e);
    }

    private void AddNewMedicine_Click(object sender, RoutedEventArgs e) =>
        Frame.Navigate(typeof(MedicineDetailsPage), null, new DrillInNavigationTransitionInfo());

    private void EditExistingMedicine_Click(object sender, RoutedEventArgs e) =>
        Frame.Navigate(typeof(MedicineDetailsPage), ViewModel.SelectedMedicine, new DrillInNavigationTransitionInfo());

}
