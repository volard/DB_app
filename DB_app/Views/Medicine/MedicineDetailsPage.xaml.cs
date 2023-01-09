using DB_app.Behaviors;
using DB_app.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Navigation;
using System.Diagnostics;
using WinUIEx.Messaging;

namespace DB_app.Views;

// TODO change "New medicine" page header to dynamic one
public sealed partial class MedicineDetailsPage : Page
{
    public MedicineDetailsViewModel ViewModel { get; }

    public MedicineDetailsPage()
    {
        ViewModel = App.GetService<MedicineDetailsViewModel>();
        InitializeComponent();
        SetBinding(NavigationViewHeaderBehavior.HeaderContextProperty, new Binding
        {
            Source = ViewModel,
            Mode = BindingMode.OneWay
        });

    }

    private async void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        await ViewModel.SaveAsync();
        ViewModel.NotifyGridAboutChange();
        Debug.WriteLine($"So boiii the ViewModel.CurrentMedicine now is {ViewModel.CurrentMedicine}");
        
        Frame.Navigate(typeof(MedicinesGridPage), null);
    }

    /// <summary>
    /// Navigate to the previous page when the user cancels the creation of a new record.
    /// </summary>
    private void CancelEdit_Click(object sender, RoutedEventArgs e) => Frame.GoBack();

    /// <summary>
    /// Check whether there are unsaved changes and warn the user.
    /// </summary>
    protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
    {
        // TODO implement this
    }


    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        ViewModel.CurrentMedicine.BuckupData();
        ViewModel.CurrentMedicine.NotifyAboutProperties();
        base.OnNavigatedTo(e);
    }

    private void NameText_TextChanged(object sender, TextChangedEventArgs e)
    {
         ViewModel.CurrentMedicine.Name = Name.Text;
    }

    private void TypeText_TextChanged(object sender, TextChangedEventArgs e)
    {
          ViewModel.CurrentMedicine.Type = Type.Text;
    }
}
