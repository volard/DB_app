﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using DB_app.Core.Contracts.Services;
using DB_app.Entities;
using DB_app.Services.Messages;
using System.Collections.ObjectModel;


namespace DB_app.ViewModels;

public partial class PharmacyDetailsViewModel : ObservableRecipient, IRecipient<ShowRecordDetailsMessage<PharmacyWrapper>>
{

    #region Constructors


    public PharmacyDetailsViewModel()
    {
        CurrentPharmacy = new()
        {
            IsNew = true
        };
        WeakReferenceMessenger.Default.Register(this);
        AvailableAddresses = new(getAvailableAddresses());
        pageTitle = "New pharmacy";
    }



    public PharmacyDetailsViewModel(PharmacyWrapper PharmacyWrapper)
    {
        CurrentPharmacy = PharmacyWrapper;
        WeakReferenceMessenger.Default.Register(this);
        AvailableAddresses = new(getAvailableAddresses());
    }


    #endregion



    #region Members

    public void Receive(ShowRecordDetailsMessage<PharmacyWrapper> message)
    {
        CurrentPharmacy = message.Value;
    }

    /// <summary>
    /// Saves pharmacy that was edited or created
    /// </summary>
    public async Task SaveAsync()
    {
        if (CurrentPharmacy.IsNew) // Create new medicine
        {
            await _repositoryControllerService.Pharmacies.InsertAsync(CurrentPharmacy.PharmacyData);
        }
        else // Update existing medicine
        {
            await _repositoryControllerService.Pharmacies.UpdateAsync(CurrentPharmacy.PharmacyData);
        }
    }


    [ObservableProperty]
    public Address selectedExistingAddress;


    public IEnumerable<Address> getAvailableAddresses()
    {
        List<Address> _addresses = new();

        foreach (var item in _repositoryControllerService.Pharmacies.GetAsync().Result.Select(a => a.Addresses))
            _addresses.AddRange(item);

        foreach (var item in _repositoryControllerService.Hospitals.GetAsync().Result.Select(a => a.Addresses))
            _addresses.AddRange(item);


        return _repositoryControllerService.Addresses.GetAsync().Result.
                 Except(_addresses);
    }

    public void NotifyGridAboutChange() => WeakReferenceMessenger.Default.Send(new AddRecordMessage<PharmacyWrapper>(CurrentPharmacy));

    #endregion



    #region Properties

    public readonly IRepositoryControllerService _repositoryControllerService
         = App.GetService<IRepositoryControllerService>();


    private PharmacyWrapper currentPharmacy;

    /// <summary>
    /// Current PharmacyWrapper to edit
    /// </summary>
    public PharmacyWrapper CurrentPharmacy
    {
        get
        {
            return currentPharmacy;
        }
        set
        {
            currentPharmacy = value;
            pageTitle = "Hospital #" + currentPharmacy.Id;
            AvailableAddresses = new(getAvailableAddresses());
        }
    }

    public ObservableCollection<Address> AvailableAddresses { get; set; }

    [ObservableProperty]
    public Address selectedAddress;

    [ObservableProperty]
    public string pageTitle;

    #endregion


}