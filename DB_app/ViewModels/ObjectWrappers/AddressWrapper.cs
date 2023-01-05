using CommunityToolkit.Mvvm.ComponentModel;
using DB_app.Core.Contracts.Services;
using DB_app.Models;
using System.ComponentModel;
using System.Diagnostics;

namespace DB_app.ViewModels;
public partial class AddressWrapper : ObservableObject, IEditableObject, IEquatable<AddressWrapper>
{
    private readonly IRepositoryControllerService _repositoryControllerService
        = App.GetService<IRepositoryControllerService>();

    public AddressWrapper(Address address)
    {
        _addressData = address;
    }

    public override string ToString() => $"AddressWrapper : '{_addressData.City}' city, '{_addressData.Street}' streen,  {_addressData.Building} building";

    public AddressWrapper()
    {
        _addressData = new Address();
    }

    private Address _addressData;

    public Address AddressData
    {
        get => _addressData;
        set => _addressData = value;
    }

    public string City
    {
        get => _addressData.City;
        set => _addressData.City = value;
    }

    public string Street
    {
        get => _addressData.Street;
        set => _addressData.Street = value;
    }

    public string Building
    {
        get => _addressData.Building; 
        set => _addressData.Building = value;
    }

    public int Id
    {
        get => _addressData.id_address;
    }


    private bool isModified = false;

    public bool IsModified
    {
        get => isModified;
        set => isModified = value;
    }

    #region IEditable implementation

    public void BeginEdit()
    {
        isModified = true;
        Debug.WriteLine($"BeginEdit - Address : For now the editable addressWrapper = {this._addressData}");
    }

    public void CancelEdit()
    {
        Debug.WriteLine("Look at me! Im soooo lazy to implement CancelEdit");
        isModified = false;
    }

    public async void EndEdit()
    {
        Debug.WriteLine($"EndEdit - Address : For now the editable addressWrapper = {this._addressData}");
        await _repositoryControllerService.Addresses.UpdateAsync(_addressData);
    }

    public bool Equals(AddressWrapper? other) =>
        City == other?.City &&
        Street == other?.Street &&
        Building == other?.Building;

    #endregion
}
