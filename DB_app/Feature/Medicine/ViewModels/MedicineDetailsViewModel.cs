using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using DB_app.Contracts.Services;
using DB_app.Contracts.ViewModels;
using DB_app.Core.Contracts.Services;
using DB_app.Helpers;
using DB_app.Models;
using DB_app.Services.Messages;
using Microsoft.UI.Xaml;
using Microsoft.Windows.ApplicationModel.Resources;
using System.Diagnostics;

namespace DB_app.ViewModels;

public partial class MedicineDetailsViewModel : ObservableRecipient, INavigationAware
{

    /// <summary>
    /// Current_value AddressWrapper to edit
    /// </summary>
    public MedicineWrapper CurrentMedicine { get; set; } = new MedicineWrapper { IsNew = true, IsInEdit = true };


    /// <summary>
    /// Represents the page's title
    /// </summary>
    [ObservableProperty]
    private string _pageTitle;


    public void OnNavigatedTo(object? parameter)
    {
        if (parameter is MedicineWrapper model)
        {
            CurrentMedicine = model;
            CurrentMedicine.Backup();
        }

        if (CurrentMedicine.IsNew)
            PageTitle = "New_Medicine".GetLocalizedValue();
        else
            PageTitle = "Medicine/Text".GetLocalizedValue() + " #" + CurrentMedicine.Id;
    }

    public void OnNavigatedFrom() { /* Not used */ }
}

