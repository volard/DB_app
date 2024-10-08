﻿using CommunityToolkit.Mvvm.ComponentModel;
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
    

    public void OnNavigatedTo(object? parameter)
    {
        if (parameter is not PharmacyWrapper model) return;
        CurrentPharmacy = model;
        CurrentPharmacy.Backup();

        if (CurrentPharmacy.IsInEdit)
        {
            LoadAvailableAddresses();
        }
    }

    public void LoadAvailableAddresses()
    {
        CollectionsHelper.LoadCollectionAsync(
            AvailableAddresses, _dispatcherQueue, _repositoryControllerService.Addresses.GetFreeAddressesAsync
        );
    }

    public void OnNavigatedFrom() { /* Not used */ }




    #endregion
    /**************************************/



    /**************************************/
    #region Properties

    

    public readonly IRepositoryControllerService _repositoryControllerService = App.GetService<IRepositoryControllerService>();


    /// <summary>
    /// Current_value AddressWrapper to edit
    /// </summary>
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(PageTitle))]
    private PharmacyWrapper _currentPharmacy = new PharmacyWrapper { IsNew = true, IsInEdit = true };

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

    public string PageTitle
    {
        get
        {
            if (CurrentPharmacy.IsNew)
                return "New_Pharmacy".GetLocalizedValue();
            return "Pharmacy/Text".GetLocalizedValue() + " #" + CurrentPharmacy.Id;
        }
    }

    #endregion
    /**************************************/

}