using CommunityToolkit.Mvvm.ComponentModel;
using DB_app.Core.Contracts.Services;
using DB_app.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace DB_app.ViewModels;

/// <summary>
/// Provides wrapper for the Order model class, encapsulating various services for access by the UI.
/// </summary>
public partial class AddressWrapper : ObservableValidator, IEditableObject, IEquatable<AddressWrapper>
{
    public AddressWrapper(Address address)
    {
        addressData = address;
        ErrorsChanged += Suspect_ErrorsChanged;
        NotifyAboutProperties();
    }

    public AddressWrapper()
    {
        addressData = new();
        ErrorsChanged += Suspect_ErrorsChanged;
        NotifyAboutProperties();
    }

    private void Suspect_ErrorsChanged(object sender, DataErrorsChangedEventArgs e)
    {
        // TODO this looks weird imo
        OnPropertyChanged(nameof(Errors));

        OnPropertyChanged(nameof(CityErrors));
        OnPropertyChanged(nameof(StreetErrors));
        OnPropertyChanged(nameof(BuildingErrors));

        OnPropertyChanged(nameof(HasCityErrors));
        OnPropertyChanged(nameof(HasStreetErrors));
        OnPropertyChanged(nameof(HasBuildingErrors));

        OnPropertyChanged(nameof(AreNoErrors));
    }

    public override string ToString() => $"AddressWrapper with addressData '{City}' city '{Street}' street '{Building}' building";

    #region Properties

    private readonly IRepositoryControllerService _repositoryControllerService
        = App.GetService<IRepositoryControllerService>();


    public Address addressData { get; set; }

    [Required(ErrorMessage = "City is Required")]
    public string City
    {
        get => addressData.City;
        set
        {
            ValidateProperty(value);
            Debug.WriteLine($"I've just validated the name and got this errors: {Errors}");
            if (!GetErrors(nameof(City)).Any())
            {
                addressData.City = value;
                OnPropertyChanged();
            }
            Debug.WriteLine($"\nfor name property especially: {GetErrors(nameof(City))}");
        }
    }


    [Required(ErrorMessage = "Street is Required")]
    public string Street
    {
        get => addressData.Street;
        set
        {
            ValidateProperty(value);
            Debug.WriteLine($"I've just validated the type and got this errors: {Errors}");
            if (!GetErrors(nameof(Street)).Any())
            {
                addressData.Street = value;
                OnPropertyChanged();
            }
            Debug.WriteLine($"\nfor type property especially: {GetErrors(nameof(Street))}");
        }
    }

    [Required(ErrorMessage = "Building is Required")]
    public string Building
    {
        get => addressData.Building;
        set
        {
            ValidateProperty(value);
            Debug.WriteLine($"I've just validated the type and got this errors: {Errors}");
            if (!GetErrors(nameof(Building)).Any())
            {
                addressData.Building = value;
                OnPropertyChanged();
            }
            Debug.WriteLine($"\nfor type property especially: {GetErrors(nameof(Building))}");
        }
    }

    public void NotifyAboutProperties()
    {
        OnPropertyChanged(nameof(City));
        OnPropertyChanged(nameof(Street));
        OnPropertyChanged(nameof(Building));
    }



    public string Errors            
        => string.Join(Environment.NewLine, from ValidationResult e in GetErrors(null) select e.ErrorMessage);
    public string CityErrors        
        => string.Join(Environment.NewLine, from ValidationResult e in GetErrors(nameof(City)) select e.ErrorMessage);
    public string StreetErrors      
        => string.Join(Environment.NewLine, from ValidationResult e in GetErrors(nameof(Street)) select e.ErrorMessage);
    public string BuildingErrors    
        => string.Join(Environment.NewLine, from ValidationResult e in GetErrors(nameof(Building)) select e.ErrorMessage);
    public bool HasCityErrors 
        => GetErrors(nameof(City)).Any();
    public bool HasStreetErrors 
        => GetErrors(nameof(Street)).Any();
    public bool HasBuildingErrors
        => GetErrors(nameof(Building)).Any();
    public bool AreNoErrors 
        => !HasErrors;

    public int Id { get => addressData.id_address; }

    // TODO implement cancel button on notification popup
    public string? BackupedCity;
    public string? BackupedStreet;
    public string? BackupedBuilding;


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

    public void BuckupData()
    {
        BackupedCity = addressData.City;
        BackupedStreet = addressData.Street;
        BackupedBuilding = addressData.Building;
    }

    public void ApplyChanges() => isModified = true;

    public void UndoChanges()
    {
        if (BackupedCity != null && 
            BackupedStreet != null &&
            BackupedBuilding != null
            )
        {
            City = BackupedCity;
            Street = BackupedStreet;
            Building  = BackupedBuilding;
            isModified = true;
        }
        Debug.WriteLine("Impossible to undo changes - backuped data is empty");
    }

    #endregion


    #region IEditable implementation
    // TODO figure out how to use this interface correctly...
    public void BeginEdit()
    {
        isModified = true;
        BuckupData();
        Debug.WriteLine($"BeginEdit : For now the editable addressWrapper = {addressData}");
    }

    public void CancelEdit()
    {
        Debug.WriteLine("Look at me! Im soooo lazy to implement CancelEdit");
        isModified = false;
    }

    public async void EndEdit()
    {
        Debug.WriteLine($"EndEdit : For now the editable addressWrapper = {addressData}");
        await _repositoryControllerService.Addresses.UpdateAsync(addressData);
    }

    public bool Equals(AddressWrapper? other) =>
        City == other?.City &&
        Street == other?.Street &&
        Building == other?.Building;

    #endregion
}