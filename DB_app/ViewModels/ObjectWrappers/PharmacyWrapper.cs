using CommunityToolkit.Mvvm.ComponentModel;
using DB_app.Core.Contracts.Services;
using DB_app.Entities;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Diagnostics;

namespace DB_app.ViewModels;

public partial class PharmacyWrapper : ObservableValidator, IEditableObject, IEquatable<PharmacyWrapper>
{

    #region Constructors

    public PharmacyWrapper(Pharmacy Pharmacy)
    {
        PharmacyData = Pharmacy;
        ErrorsChanged += Suspect_ErrorsChanged;
        NotifyAboutProperties();
    }

    public PharmacyWrapper()
    {
        PharmacyData = new();
        ErrorsChanged += Suspect_ErrorsChanged;
        NotifyAboutProperties();
    }

    #endregion


    #region Properties

    private readonly IRepositoryControllerService _repositoryControllerService
        = App.GetService<IRepositoryControllerService>();


    public Pharmacy PharmacyData { get; set; }


    [Required(ErrorMessage = "Maindoctor's name is Required")]
    public string Name
    {
        get => PharmacyData.Name;
        set
        {
            ValidateProperty(value);
            if (!GetErrors(nameof(Name)).Any())
            {
                PharmacyData.Name = value;
                OnPropertyChanged();
            }
        }
    }

    public bool IsActive
    {
        get => PharmacyData.IsActive;
        set
        {
            PharmacyData.IsActive = value;
            OnPropertyChanged();
        }
    }

   

    public ObservableCollection<Address> ObservableAddresses
    {
        get
        {   
            if (PharmacyData.Addresses == null) { return new(); }
            else
            {
                return new(PharmacyData.Addresses);
            }
        }
        set
        {
            PharmacyData.Addresses = value.ToList();
            IsModified = true;
            OnPropertyChanged();
        }
    }


    public int Id { get => PharmacyData.Id; }


    // TODO that looks disgusting. I wonder if functions in xaml bindings works properly for me
    public string Errors
        => string.Join(Environment.NewLine, from ValidationResult e in GetErrors(null) select e.ErrorMessage);
    public string NameErrors
        => string.Join(Environment.NewLine, from ValidationResult e in GetErrors(nameof(Name)) select e.ErrorMessage);


    public bool HasNameErrors
        => GetErrors(nameof(Name)).Any();

    public bool AreNoErrors
        => !HasErrors;


    // TODO implement cancel button on notification popup
    // TODO maybe it will be better to create another Pharmacy object instead of 
    // keeping the bunch of backuped properties
    public string? BackupedName;
    public string? BackupedINN;
    public string? BackupedOGRN;


    /// <summary>
    /// Indicates about changes that is not synced with UI DataGrid
    /// </summary>
    [ObservableProperty]
    private bool isModified = false;

    /// <summary>
    /// Indicates whether its a new object
    /// </summary>
    public bool isNew = false;

    #endregion


    #region Modification methods


    public void NotifyAboutProperties()
    {
        OnPropertyChanged(nameof(Name));
        OnPropertyChanged(nameof(ObservableAddresses));
    }

    public void NotifyAboutAddressesChanged() =>
        OnPropertyChanged(nameof(ObservableAddresses));

    private void Suspect_ErrorsChanged(object sender, DataErrorsChangedEventArgs e)
    {
        OnPropertyChanged(nameof(Errors));
        OnPropertyChanged(nameof(NameErrors));

        OnPropertyChanged(nameof(Name));


        OnPropertyChanged(nameof(HasNameErrors));


        OnPropertyChanged(nameof(AreNoErrors));
    }

    public override string ToString()
        => $"PharmacyWrapper with PharmacyData - [ {PharmacyData} ]";


    public void BuckupData()
    {
        BackupedName = Name;
    }

    public void ApplyChanges() => isModified = true;

    public void UndoChanges()
    {
        if (
                BackupedName != null &&
                BackupedINN != null &&
                BackupedOGRN != null
           )
        {
            Name = BackupedName;

            isModified = true;
        }
        
    }


    #endregion


    #region IEditable implementation
    // TODO figure out how to use this interface correctly...
    public void BeginEdit()
    {
        isModified = true;
        BuckupData();
    }

    public void CancelEdit()
    {
        
        isModified = false;
    }

    public async void EndEdit()
    {
        await _repositoryControllerService.Pharmacies.UpdateAsync(PharmacyData);
    }

    public bool Equals(PharmacyWrapper? other) =>
        Name == other?.Name;

    #endregion
}