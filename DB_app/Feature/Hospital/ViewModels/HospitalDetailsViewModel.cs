using CommunityToolkit.Mvvm.ComponentModel;
using DB_app.Contracts.ViewModels;
using DB_app.Core.Contracts.Services;
using DB_app.Models;
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

    public void OnNavigatedTo(object? parameter)
    {
        if (parameter is HospitalWrapper model)
        {
            CurrentHospital = model;
            CurrentHospital.Backup();
            PageTitle = "Hospital #" + CurrentHospital.Id;

            if (CurrentHospital.IsInEdit)
                AvailableAddresses = new(GetAvailableAddresses());
        }
    }

    public void OnNavigatedFrom() { /* Not used */ }

    public HospitalWrapper CurrentHospital { get; set; } = new HospitalWrapper{ IsNew = true, IsInEdit = true };


    public ObservableCollection<Address> AvailableAddresses { get; set; } = new();

    [ObservableProperty]
    public Address selectedAddress;

    [ObservableProperty]
    private string pageTitle = "sadfasdf";

    #endregion
}