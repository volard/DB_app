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
        Debug.WriteLine($"So boiii the ViewModel.CurrentMedicine now is {ViewModel.CurrentMedicine}");
        
        //Frame.Navigate(typeof(MedicinesGridPage), ViewModel.CurrentMedicine);
    }


    /// <summary>
    /// Navigate to the previous page when the user cancels the creation of a new record.
    /// </summary>
    private void CancelEdit_Click(object sender, RoutedEventArgs e)
        => Frame.GoBack();

    /// <summary>
    /// Check whether there are unsaved changes and warn the user.
    /// </summary>
    protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
    {
        // TODO implement this
    }

    /// <summary>
    /// Loads selected MedicineWrapper object.
    /// </summary>
    /// <param newName="e">Info about the event.</param>
    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        var selectedMedicine = (MedicineWrapper)e.Parameter;
        if (selectedMedicine != null)
        {
            ViewModel.CurrentMedicine = selectedMedicine;
            ViewModel.CurrentMedicine.Name = selectedMedicine.Name;
            ViewModel.CurrentMedicine.Type = selectedMedicine.Type;
        }
        else
        {
            ViewModel.CurrentMedicine.Name = "";
            ViewModel.CurrentMedicine.Type = "";
        }
        ViewModel.CurrentMedicine.BuckupData();
        base.OnNavigatedTo(e);
    }

    private bool DetermineModifiedStatus()
    {
        bool isNameTextModified =
            Name.Text != ViewModel.CurrentMedicine.Name;

        bool isTypeTextModified =
            Type.Text != ViewModel.CurrentMedicine.Type;

        bool isEveryTextBoxNotEmpty =
            Name.Text != "" &&
            Type.Text != "";

        return (isNameTextModified || isTypeTextModified) && isEveryTextBoxNotEmpty;
    }

    private void NameText_TextChanged(object sender, TextChangedEventArgs e)
    {
        //ViewModel.CurrentMedicine.IsModified = DetermineModifiedStatus();
        //if (ViewModel.CurrentMedicine.IsModified || ViewModel.CurrentMedicine.isNew)
        //{
            ViewModel.CurrentMedicine.Name = Name.Text;
        //}
    }

    private void TypeText_TextChanged(object sender, TextChangedEventArgs e)
    {
        //ViewModel.CurrentMedicine.IsModified = DetermineModifiedStatus();
        //if (ViewModel.CurrentMedicine.IsModified || ViewModel.CurrentMedicine.isNew)
        //{
            ViewModel.CurrentMedicine.Type = Type.Text;
        //}
    }
}
