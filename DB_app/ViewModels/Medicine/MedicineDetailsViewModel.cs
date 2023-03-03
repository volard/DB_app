using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using DB_app.Contracts.ViewModels;
using DB_app.Core.Contracts.Services;
using DB_app.Entities;
using DB_app.Services.Messages;
using Microsoft.UI.Xaml;
using System.Diagnostics;

namespace DB_app.ViewModels;

public partial class MedicineDetailsViewModel : ObservableRecipient, INavigationAware
{

    /// <summary>
    /// Current AddressWrapper to edit
    /// </summary>
    public MedicineWrapper CurrentMedicine { get; set; } = new();



    /// <summary>
    /// Represents the page's title
    /// </summary>
    [ObservableProperty]
    private string _pageTitle = "New meidicne";


    /// <summary>
    /// Saves customer data that was edited.
    /// </summary>
    public async void SaveCurrent(object? sender, RoutedEventArgs  e)
    {
        try
        {
            await CurrentMedicine.SaveAsync();
        }
        catch(Exception ex)
        {

        }
    }


    public void OnNavigatedTo(object? parameter)
    {
        if (parameter is MedicineWrapper model)
        {
            CurrentMedicine = model;
            PageTitle = "Edit address";
            CurrentMedicine.Backup();
        }
    }

    public void OnNavigatedFrom()
    {
        // Not used
    }
}

