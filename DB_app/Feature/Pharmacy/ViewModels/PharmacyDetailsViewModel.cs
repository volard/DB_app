using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.WinUI;
using DB_app.Contracts.ViewModels;
using DB_app.Core.Contracts.Services;
using DB_app.Helpers;
using DB_app.Models;
using DB_app.Services.Messages;
using Microsoft.UI.Dispatching;
using System.Collections.ObjectModel;


namespace DB_app.ViewModels;

public partial class PharmacyDetailsViewModel : ObservableRecipient, INavigationAware
{
    /**************************************/
    #region Members
    /**************************************/

    /// <summary>
    /// Loads the addresses data.
    /// </summary>
    public async Task LoadAvailableAddressesAsync()
    {
        await _dispatcherQueue.EnqueueAsync(() =>
        {
            IsLoading = true;
        });

        var addresses = await _repositoryControllerService.Addresses.GetFreeAddressesAsync();

        await _dispatcherQueue.EnqueueAsync(() =>
        {
            AvailableAddresses.Clear();
            foreach (var address in addresses)
            {
                AvailableAddresses.Add(address);
            }

            IsLoading = false;
        });
    }


    public void OnNavigatedTo(object? parameter)
    {
        if (parameter is PharmacyWrapper model)
        {
            CurrentPharmacy = model;
            CurrentPharmacy.Backup();

            if (CurrentPharmacy.IsInEdit) { Task.Run(LoadAvailableAddressesAsync); }
        }

        if (CurrentPharmacy.IsNew)
            PageTitle = "New_Pharmacy".GetLocalizedValue();
        else
            PageTitle = "Pharmacy/Text".GetLocalizedValue() + " #" + CurrentPharmacy.Id;
    }

    public void OnNavigatedFrom() { /* Not used */ }

    


    #endregion


    /**************************************/
    #region Properties
    /**************************************/

    public readonly IRepositoryControllerService _repositoryControllerService = App.GetService<IRepositoryControllerService>();


    /// <summary>
    /// Current_value AddressWrapper to edit
    /// </summary>
    public PharmacyWrapper CurrentPharmacy { get; set; } = new PharmacyWrapper { IsNew = true, IsInEdit = true };

    private readonly DispatcherQueue _dispatcherQueue = DispatcherQueue.GetForCurrentThread();

    public ObservableCollection<Address> AvailableAddresses { get; set; } = new();

    /// <summary>
    /// Location object that binded to hospital and selected by user. 
    /// </summary>
    [ObservableProperty]
    private PharmacyLocation? _selectedExistingLocation;

    /// <summary>
    /// Gets or sets a value that indicates whether to show a progress bar. 
    /// </summary>
    [ObservableProperty]
    private bool _isLoading;

    [ObservableProperty]
    private Address? _selectedAddress;

    [ObservableProperty]
    private string? _pageTitle;

    #endregion


}