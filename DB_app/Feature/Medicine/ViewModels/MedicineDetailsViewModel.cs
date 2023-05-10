using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using DB_app.Contracts.Services;
using DB_app.Contracts.ViewModels;
using DB_app.Core.Contracts.Services;
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

    public ResourceLoader resourceLoader = App.GetService<ILocalizationService>().ResourceLoader;


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

            if (CurrentMedicine.IsInEdit)
            {
                PageTitle = "Edit medicine #" + CurrentMedicine.Id;
            }
            else
                PageTitle = "Medicine #" + CurrentMedicine.Id;
        }
    }

    public void OnNavigatedFrom()
    {
        // Not used
    }
}

