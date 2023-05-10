using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.WinUI;
using DB_app.Contracts.Services;
using DB_app.Contracts.ViewModels;
using DB_app.Core.Contracts.Services;
using DB_app.Models;
using Microsoft.UI.Dispatching;
using Microsoft.Windows.ApplicationModel.Resources;
using System.Collections.ObjectModel;

namespace DB_app.ViewModels;

public partial class HospitalDetailsViewModel : ObservableRecipient, INavigationAware
{

    #region Members

    /// <summary>
    /// Gets unlinked addresses.
    /// </summary>
    /// <returns></returns>
    public IEnumerable<Address> GetAvailableAddresses()
    {
        List<Address> _addresses = new();

        foreach (var item in _repositoryControllerService.Hospitals.GetAsync().Result.Select(a => a.Locations))
            _addresses.AddRange(item.Select(a => a.Address));

        return _repositoryControllerService.Addresses.GetAsync().Result.Except(_addresses);
    }


    #endregion


    #region Properties


    /// <summary>
    /// Location object that binded to hospital and selected by user. 
    /// </summary>
    [ObservableProperty]
    private HospitalLocation? selectedExistingLocation;


    /// <summary>
    /// Gets or sets a value that indicates whether to show a progress bar. 
    /// </summary>
    [ObservableProperty]
    private bool _isLoading;

    private readonly DispatcherQueue _dispatcherQueue = DispatcherQueue.GetForCurrentThread();

    public readonly ResourceLoader _resourceLoader = App.GetService<ILocalizationService>().ResourceLoader;

    public readonly IRepositoryControllerService _repositoryControllerService = App.GetService<IRepositoryControllerService>();
    
    public void OnNavigatedTo(object? parameter)
    {
        PageTitle = _resourceLoader.GetString("New_Hospital");
        if (parameter is HospitalWrapper model)
        {
            CurrentHospital = model;
            CurrentHospital.Backup();
            PageTitle = _resourceLoader.GetString("Hospital/Text") + " #" + CurrentHospital.Id;

            if (CurrentHospital.IsInEdit) { Task.Run(LoadAvailableAddressesAsync); }
               
        }
    }

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

    public void OnNavigatedFrom() { /* Not used */ }

    public HospitalWrapper CurrentHospital { get; set; } = new HospitalWrapper{ IsNew = true, IsInEdit = true };

    public ObservableCollection<Address> AvailableAddresses { get; set; } = new();

    [ObservableProperty]
    private Address selectedAddress;

    [ObservableProperty]
    private string _pageTitle;

    #endregion
}