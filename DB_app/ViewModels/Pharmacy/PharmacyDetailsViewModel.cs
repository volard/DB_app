using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using DB_app.Contracts.ViewModels;
using DB_app.Core.Contracts.Services;
using DB_app.Entities;
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
            PageTitle = "Edit " + CurrentPharmacy.Name;
            CurrentPharmacy.Backup();
        }
    }

    public void OnNavigatedFrom()
    {
        // Not used
    }

    /// <summary>
    /// Saves productPharmacy that was edited or created
    /// </summary>
    public async Task SaveAsync()
    {
        if (CurrentPharmacy.IsNew) // Create new productMedicine
        {
            await _repositoryControllerService.Pharmacies.InsertAsync(CurrentPharmacy.PharmacyData);
        }
        else // Update existing productMedicine
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


    #endregion



    #region Properties

    public readonly IRepositoryControllerService _repositoryControllerService
         = App.GetService<IRepositoryControllerService>();


    /// <summary>
    /// Current AddressWrapper to edit
    /// </summary>
    public PharmacyWrapper CurrentPharmacy { get; set; } = new();

    public ObservableCollection<Address> AvailableAddresses { get; set; }

    [ObservableProperty]
    private Address _selectedAddress;

    [ObservableProperty]
    private string _pageTitle = "New pharmacy";

    #endregion


}