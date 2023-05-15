using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using DB_app.Core.Contracts.Services;
using DB_app.Models;
using DB_app.Services.Messages;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DB_app.ViewModels;

/// <summary>
/// Provides wrapper for the <see cref="Address"/> model class, encapsulating various services for access by the UI.
/// </summary>
public sealed partial class AddressWrapper : ObservableValidator, IEditableObject
{
    /**************************************/
    #region Constructors
    /**************************************/


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
        else 
        { 
            AddressData = address; 
        }

        InitFields();
    }

    #endregion



    /**************************************/
    #region Properties
    /**************************************/

    /// <summary>
    /// Underlying <see cref="Address"/> data
    /// </summary>
    public Address AddressData { get; set; } = new();


    [ObservableProperty]
    [NotifyDataErrorInfo]
    [NotifyPropertyChangedFor(nameof(IsModified))]
    [Required(ErrorMessage = "City is Required")]
    private string? _city;


    [ObservableProperty]
    [NotifyDataErrorInfo]
    [NotifyPropertyChangedFor(nameof(IsModified))]
    [Required(ErrorMessage = "Street is Required")]
    private string? _street;


    [ObservableProperty]
    [NotifyDataErrorInfo]
    [NotifyPropertyChangedFor(nameof(IsModified))]
    [Required(ErrorMessage = "Building is Required")]
    private string? _building;
    

    public int Id { get => AddressData.Id; }


    private Address? _backupData;


    /// <summary>
    /// Indicates about changes that is not synced with UI DataGrid
    /// </summary>
    public bool IsModified
    {
        get
        {
            return
                City != AddressData.City ||
                Street != AddressData.Street ||
                Building != AddressData.Building;
        }
    }


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




    /**************************************/
    #region Members
    /**************************************/

    public override string ToString() => 
        $"AddressWrapper with addressData {AddressData}";


    public override bool Equals(object? obj)
    {
        if (obj is not AddressWrapper other) return false;
        return
            City == other?.City &&
            Street == other?.Street &&
            Building == other?.Building;
    }


    private void InitFields()
    {
        City = AddressData.City;
        Street = AddressData.Street;
        Building = AddressData.Building;
    }


    #endregion




    /**************************************/
    #region Modification methods
    /**************************************/


    /// <summary>
    /// Go back to prevoius data after updating
    /// </summary>
    public async Task Revert()
    {
        if (_backupData != null)
        {
            AddressData = _backupData;
            await App.GetService<IRepositoryControllerService>().Addresses.UpdateAsync(AddressData);
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

        // Sync with grid
        WeakReferenceMessenger.Default.Send(new AddRecordMessage<AddressWrapper>(this));
        return true;
    }




    public void Backup() =>
        _backupData = AddressData;


    #endregion




    /**************************************/
    #region IEditable implementation
    /**************************************/

    public void BeginEdit()
    {
        IsInEdit = true;
        OnPropertyChanged(nameof(IsModified));
        Backup();
    }


    public void CancelEdit()
    {
        InitFields();
        OnPropertyChanged(nameof(IsModified));
        IsInEdit = false;
    }


    public void EndEdit()
    {
        IsInEdit= false;
        OnPropertyChanged(nameof(IsModified));

        // NOTE the underlying code relays on preliminary data validation
        AddressData.City = City;
        AddressData.Street = Street;
        AddressData.Building = Building;
    }


    #endregion
}