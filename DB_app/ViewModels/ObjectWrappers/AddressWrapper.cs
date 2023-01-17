using CommunityToolkit.Mvvm.ComponentModel;
using DB_app.Core.Contracts.Services;
using DB_app.Entities;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace DB_app.ViewModels;

/// <summary>
/// Provides wrapper for the Order model class, encapsulating various services for access by the UI.
/// </summary>
public partial class AddressWrapper : ObservableValidator, IEditableObject, IEquatable<AddressWrapper>
{

    public AddressWrapper(Address? address = null)
    {
        if (address == null)
        {
            isNew = true;
            AddressData = new();
        }
        else { AddressData = address; }
        ErrorsChanged += Suspect_ErrorsChanged;
        NotifyAboutProperties();
    }

    private void Suspect_ErrorsChanged(object sender, DataErrorsChangedEventArgs e)
    {
        NotifyAboutProperties();
    }

    public override string ToString() => $"AddressWrapper with addressData '{City}' city '{Street}' street '{Building}' building";

    #region Properties

    private readonly IRepositoryControllerService _repositoryControllerService
        = App.GetService<IRepositoryControllerService>();

    private Address _addressData;

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
    private string _city;

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required(ErrorMessage = "Street is Required")]
    private string _street;

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required(ErrorMessage = "Building is Required")]
    private string _building;

    public void NotifyAboutProperties()
    {
        OnPropertyChanged(string.Empty);
    }
        
    public string GetPropertyErrors(string type)
        => string.Join(Environment.NewLine, from ValidationResult e in GetErrors(type) select e.ErrorMessage);

    public string Errors            
        => string.Join(Environment.NewLine, from ValidationResult e in GetErrors(null) select e.ErrorMessage);
    
    public bool HasCityErrors =>
        GetPropertyErrors(nameof(City)).Any();

    public bool HasStreetErrors =>
       GetPropertyErrors(nameof(Street)).Any();

    public bool HasBuildingErrors =>
       GetPropertyErrors(nameof(Building)).Any();


    public Microsoft.UI.Xaml.Visibility HasPropertyErrors(string type)
        => Converters.CollapsedIfNull(GetErrors(type).Any());
    
    public bool AreNoErrors 
        => !HasErrors;

    public int Id { get => _addressData.Id; }


    /// <summary>
    /// Indicates about changes that is not synced with UI DataGrid
    /// </summary>
    [ObservableProperty]
    private bool isModified = false;

    /// <summary>
    /// indicates whether its a new object
    /// </summary>
    public bool isNew = false;

    #endregion


    #region Modification methods


    public async Task ApplyChanges()
    {
        _addressData.City = City;
        _addressData.Street = Street;
        _addressData.Building = Building;
    }

    /// <summary>
    /// Undo current changes
    /// </summary>
    public void UndoChanges()
    {
        City = _addressData.City;
        Street = _addressData.Street;
        Building = _addressData.Building;
        IsModified = false;
    }

    /// <summary>
    /// Get back to prevoius data after updating
    /// </summary>
    public async Task Revert()
    {
        if (BackupData != null)
        {
            AddressData = BackupData;
            await _repositoryControllerService.Addresses.UpdateAsync(_addressData);
        }
    }

    private Address? BackupData;

    public void Backup() =>
        BackupData = _addressData;

    #endregion


    #region IEditable implementation
    // TODO figure out how to use this interface correctly...
    public void BeginEdit()
    {
        Backup();
        isModified = true;
    }

    public void CancelEdit()
    {
        isModified = false;
    }

    public async void EndEdit()
    {
        await _repositoryControllerService.Addresses.UpdateAsync(AddressData);
    }

    public bool Equals(AddressWrapper? other) =>
        City == other?.City &&
        Street == other?.Street &&
        Building == other?.Building;

    #endregion
}