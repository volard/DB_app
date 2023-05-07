using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using DB_app.Contracts.ViewModels;
using DB_app.Core.Contracts.Services;
using DB_app.Models;
using DB_app.Services.Messages;
using System.Collections.ObjectModel;


namespace DB_app.ViewModels;

public partial class PharmacyDetailsViewModel : ObservableRecipient, INavigationAware
{


    #region Members

    public void OnNavigatedTo(object? parameter)
    {
        if (parameter is PharmacyWrapper model)
        {
            CurrentPharmacy = model;
            CurrentPharmacy.Backup();

            if (CurrentPharmacy.IsInEdit)
            {
                PageTitle = "Edit hospital #" + CurrentPharmacy.Id;
            }
            else
                PageTitle = "Hospital #" + CurrentPharmacy.Id;
        }
    }

    public void OnNavigatedFrom()
    {
        // Not used
    }

    public IEnumerable<Address> GetAvailableAddresses()
    {
        List<Address> _addresses = new();

        foreach (var item in _repositoryControllerService.Pharmacies.GetAsync().Result.Select(a => a.Locations))
            _addresses.AddRange(item.Select(el => el.Address));

        foreach (var item in _repositoryControllerService.Hospitals.GetAsync().Result.Select(a => a.Locations))
            _addresses.AddRange(item.Select(el => el.Address));


        return _repositoryControllerService.Addresses.GetAsync().Result.
                 Except(_addresses);
    }


    #endregion



    #region Properties


    [ObservableProperty]
    public PharmacyLocation selectedExistingLocation;

    public readonly IRepositoryControllerService _repositoryControllerService
         = App.GetService<IRepositoryControllerService>();


    /// <summary>
    /// Current_value AddressWrapper to edit
    /// </summary>
    public PharmacyWrapper CurrentPharmacy { get; set; } = new PharmacyWrapper { IsNew = true, IsInEdit = true };

    public ObservableCollection<Address> AvailableAddresses { get; set; }

    [ObservableProperty]
    private Address _selectedAddress;

    [ObservableProperty]
    private string _pageTitle = "New pharmacy";

    #endregion


}