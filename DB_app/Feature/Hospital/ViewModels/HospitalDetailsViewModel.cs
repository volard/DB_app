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
    /// Saves hospital that was edited or created
    /// </summary>
    public async Task SaveAsync()
    {
        if (CurrentHospital.IsNew)
            await _repositoryControllerService.Hospitals.InsertAsync(CurrentHospital.HospitalData);
        else
            await _repositoryControllerService.Hospitals.UpdateAsync(CurrentHospital.HospitalData);
    }


    public IEnumerable<Address> GetAvailableAddresses()
    {
        List<Address> _addresses = new();

        foreach (var item in _repositoryControllerService.Hospitals.GetAsync().Result.Select(a => a.Addresses))
            _addresses.AddRange(item);

        return _repositoryControllerService.Addresses.GetAsync().Result.Except(_addresses);
    }


    #endregion


    #region Properties

    [ObservableProperty]
    public Address? selectedExistingAddress;

    private readonly IRepositoryControllerService _repositoryControllerService = App.GetService<IRepositoryControllerService>();

    public void OnNavigatedTo(object? parameter)
    {
        if (parameter is HospitalWrapper model)
        {
            CurrentHospital = model;
            CurrentHospital.Backup();

            if (CurrentHospital.IsInEdit)
            {
                AvailableAddresses = new(GetAvailableAddresses());
                pageTitle = "Edig hospital #" + CurrentHospital.Id;
            }
            else
                pageTitle = "Hospital #" + CurrentHospital.Id;
        }
    }

    public void OnNavigatedFrom() { /* Not used */ }

    public HospitalWrapper CurrentHospital { get; set; } = new(){ IsNew = true };

    public ObservableCollection<Address> AvailableAddresses { get; set; } = new();

    [ObservableProperty]
    public Address selectedAddress;

    [ObservableProperty]
    public string pageTitle = "New hospital";

    #endregion
}