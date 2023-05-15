using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.WinUI;
using DB_app.Contracts.ViewModels;
using DB_app.Core.Contracts.Services;
using DB_app.Helpers;
using DB_app.Models;
using Microsoft.UI.Dispatching;
using System.Collections.ObjectModel;

namespace DB_app.ViewModels;

public partial class HospitalDetailsViewModel : ObservableRecipient, INavigationAware
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
        if (parameter is HospitalWrapper model)
        {
            CurrentHospital = model;
            CurrentHospital.Backup();
            
            if (CurrentHospital.IsInEdit) { Task.Run(LoadAvailableAddressesAsync); }
        }

        if (CurrentHospital.IsNew)
            PageTitle = "New_Hospital".GetLocalizedValue();
        else
            PageTitle = "Hospital/Text".GetLocalizedValue() + " #" + CurrentHospital.Id;
    }

   

    public void OnNavigatedFrom() { /* Not used */ }

    #endregion


    /**************************************/
    #region Properties
    /**************************************/

    private readonly DispatcherQueue _dispatcherQueue = DispatcherQueue.GetForCurrentThread();

    public readonly IRepositoryControllerService _repositoryControllerService = App.GetService<IRepositoryControllerService>();

    public HospitalWrapper CurrentHospital { get; set; } = new HospitalWrapper { IsNew = true, IsInEdit = true };

    public ObservableCollection<Address> AvailableAddresses { get; set; } = new();

    /// <summary>
    /// Location object that binded to hospital and selected by user. 
    /// </summary>
    [ObservableProperty]
    private HospitalLocation? _selectedExistingLocation;

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