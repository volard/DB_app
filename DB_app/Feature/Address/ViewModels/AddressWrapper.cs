using CommunityToolkit.Mvvm.ComponentModel;
using DB_app.Core.Contracts.Services;
using DB_app.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DB_app.ViewModels;

/// <summary>
/// Provides wrapper for the <see cref="Address"/> model class, encapsulating various services for access by the UI.
/// </summary>
public sealed partial class AddressWrapper : ObservableValidator, IEditableObject
{

#region Constructors

    /// <summary>
    /// Initialize new AddressWrapper object
    /// </summary>
    /// <param name="address">Address model representing by the wrapper</param>
    public AddressWrapper(Address? address = null)
    {
        if (address == null)
        {
            IsNew = true;
        }
        else { AddressData = address; }
    }

#endregion






#region Properties

    /// <summary>
    /// Underlying <see cref="Address"/> data
    /// </summary>
    private Address _addressData = new();

    /// <summary>
    /// Underlying <see cref="Address"/> data
    /// </summary>
    public Address AddressData 
    {
        get => _addressData;
        set 
        {   
            _addressData = value;
            City = _addressData.City;
            Street = _addressData.Street;
            Building = _addressData.Building;
        } 
    }


    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required(ErrorMessage = "City is Required")]
    private string? _city;

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required(ErrorMessage = "Street is Required")]
    private string? _street;

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required(ErrorMessage = "Building is Required")]
    private string? _building;
    

    public int Id { get => _addressData.Id; }

    private Address? _backupData;


    /// <summary>
    /// Indicates about changes that is not synced with UI DataGrid
    /// </summary>
    [ObservableProperty]
    private bool _isModified = false;


    /// <summary>
    /// Indicate edit mode
    /// </summary>
    [ObservableProperty]
    private bool _isInEdit = false;


    /// <summary>
    /// Indicates whether its a new object
    /// </summary>
    [ObservableProperty]
    private bool _isNew = false;

#endregion






    #region Members

    public override string ToString() => 
        $"AddressWrapper with addressData {AddressData}";

    #endregion








#region Modification methods


    /// <summary>
    /// Go back to prevoius data after updating
    /// </summary>
    public async Task Revert()
    {
        if (_backupData != null)
        {
            AddressData = _backupData;
            await App.GetService<IRepositoryControllerService>().Addresses.UpdateAsync(_addressData);
        }
    }

    public async Task<bool> SaveAsync()
    {
        ValidateAllProperties();
        if (HasErrors) return false;
        EndEdit();
        if (IsNew)
        {
            await App.GetService<IRepositoryControllerService>().Addresses.InsertAsync(AddressData);
        }
        else
        {
            await App.GetService<IRepositoryControllerService>().Addresses.UpdateAsync(AddressData);
        }
        IsNew = false;
        return true;
    }




    public void Backup() =>
        _backupData = _addressData;


#endregion






#region IEditable implementation


    public void BeginEdit()
    {
        IsInEdit = true;
        Backup();
    }

    public void CancelEdit()
    {
        City = _addressData.City;
        Street = _addressData.Street;
        Building = _addressData.Building;
        IsModified = false;
        IsInEdit= false;
    }

    public void EndEdit()
    {
        IsInEdit= false;
        _addressData.City = City;
        _addressData.Street = Street;
        _addressData.Building = Building;
    }


    public override bool Equals(object? obj)
    {
        if (obj is not AddressWrapper other) return false;
        return
            City == other?.City &&
            Street == other?.Street &&
            Building == other?.Building;
    }

    
#endregion


}