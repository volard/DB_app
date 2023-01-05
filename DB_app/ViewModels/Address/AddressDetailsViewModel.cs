using CommunityToolkit.Mvvm.ComponentModel;
using DB_app.Core.Contracts.Services;
using DB_app.Models;
using Microsoft.UI.Xaml;

namespace DB_app.ViewModels;

public partial class AddressDetailsViewModel : ObservableObject
{
    private readonly IRepositoryControllerService _repositoryControllerService
         = App.GetService<IRepositoryControllerService>();

    [ObservableProperty]
    private bool _isEditDisabled;

    [ObservableProperty]
    private bool _isEditEnabled;

    public AddressDetailsViewModel()
    {
        IsEditDisabled = true;
        IsEditEnabled = false;
        city = "";
        street = "";
        building = "";
    }

    public void StartEdit_Click(object sender, RoutedEventArgs e)
    {
        IsEditDisabled = false;
        IsEditEnabled = true;
    }

    private AddressWrapper? _currentAddress;

    public AddressWrapper? CurrentAddress
    {
        get => _currentAddress;
        set
        {
            _currentAddress = value;
            if (_currentAddress != null)
            {
                _isEditEnabled = true;
                _isEditDisabled = false;
                City = _currentAddress.AddressData.City;
                Street = _currentAddress.AddressData.Street;
                Building = _currentAddress.AddressData.Building;
            }
            else
            {
                _isEditEnabled = false;
                _isEditDisabled = true;
                City = "";
                Street = "";
                Building = "";
            }

        }
    }

    [ObservableProperty]
    public string city;

    [ObservableProperty]
    public string street;

    [ObservableProperty]
    public string building;



    /// <summary>
    /// Saves customer data that has been edited.
    /// </summary>
    public async void SaveAsync(object sender, RoutedEventArgs e)
    {

        if (IsEditDisabled) // Create new medicine
        {
            Address newAddress = new()
            {
                City = city,
                Street = street,
                Building = building
            };
            await _repositoryControllerService.Addresses.InsertAsync(newAddress);

        }
        else // Update existing medicine
        {
            if (_currentAddress != null) // TODO WTF is happening here - its redundant but I can't remove it 
            {
                _currentAddress.City = this.city;
                _currentAddress.Street = this.street;
                _currentAddress.Building = this.building;
                _currentAddress.IsModified = true;

                await _repositoryControllerService.Addresses.UpdateAsync(_currentAddress.AddressData);
            }

           
        }

    }
}

