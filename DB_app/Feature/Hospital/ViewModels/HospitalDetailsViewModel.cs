using CommunityToolkit.Mvvm.ComponentModel;
using DB_app.Contracts.Services;
using DB_app.Contracts.ViewModels;
using DB_app.Core.Contracts.Services;
using DB_app.Models;
using Microsoft.Windows.ApplicationModel.Resources;
using System.Collections.ObjectModel;

namespace DB_app.ViewModels;

public partial class HospitalDetailsViewModel : ObservableRecipient, INavigationAware
{

    #region Members

    /// <summary>
    /// Get unlinked addresses
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

    [ObservableProperty]
    public HospitalLocation? selectedExistingLocation;

    public readonly IRepositoryControllerService _repositoryControllerService = App.GetService<IRepositoryControllerService>();

    public ResourceLoader resourceLoader = App.GetService<ILocalizationService>().ResourceLoader;
    
    public void OnNavigatedTo(object? parameter)
    {
        PageTitle = resourceLoader.GetString("New_Hospital");
        if (parameter is HospitalWrapper model)
        {
            CurrentHospital = model;
            CurrentHospital.Backup();
            PageTitle = resourceLoader.GetString("Hospital/Text") + " #" + CurrentHospital.Id;

            if (CurrentHospital.IsInEdit)
                AvailableAddresses = new(GetAvailableAddresses());
        }
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