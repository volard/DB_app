using CommunityToolkit.Mvvm.ComponentModel;
using DB_app.Core.Contracts.Services;
using DB_app.Models;
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


   
    [Required(ErrorMessage = "INN is Required")]
    [RegularExpression("([0-9]+)", ErrorMessage = "Please enter a Number")]
    public string INN
    {
        get => PharmacyData.INN;
        set
        {
            ValidateProperty(value);
            if (!GetErrors(nameof(INN)).Any())
            {
                PharmacyData.INN = value;
                OnPropertyChanged();
            }
        }
    }


    [Required(ErrorMessage = "OGRN is Required")]
    [RegularExpression("([0-9]+)", ErrorMessage = "Please enter a Number")]
    public string OGRN
    {
        get => PharmacyData.OGRN;
        set
        {
            ValidateProperty(value);
            if (!GetErrors(nameof(OGRN)).Any())
            {
                PharmacyData.OGRN = value;
                OnPropertyChanged();
            }
        }
    }


    public ObservableCollection<Address> ObservableAddresses
    {
        get => new(PharmacyData.Addresses);
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
    public string INNErrors
        => string.Join(Environment.NewLine, from ValidationResult e in GetErrors(nameof(INN)) select e.ErrorMessage);
    public string OGRNErrors
        => string.Join(Environment.NewLine, from ValidationResult e in GetErrors(nameof(OGRN)) select e.ErrorMessage);


    public bool HasNameErrors
        => GetErrors(nameof(Name)).Any();
    public bool HasINNErrors
        => GetErrors(nameof(INN)).Any();
    public bool HasOGRNErrors
        => GetErrors(nameof(OGRN)).Any();
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
        OnPropertyChanged(nameof(INN));
        OnPropertyChanged(nameof(OGRN));
        OnPropertyChanged(nameof(ObservableAddresses));
    }

    public void NotifyAboutAddressesChanged() =>
        OnPropertyChanged(nameof(ObservableAddresses));

    private void Suspect_ErrorsChanged(object sender, DataErrorsChangedEventArgs e)
    {
        OnPropertyChanged(nameof(Errors));
        OnPropertyChanged(nameof(NameErrors));
        OnPropertyChanged(nameof(INNErrors));
        OnPropertyChanged(nameof(OGRNErrors));

        OnPropertyChanged(nameof(Name));
        OnPropertyChanged(nameof(INN));
        OnPropertyChanged(nameof(OGRN));

        OnPropertyChanged(nameof(HasNameErrors));
        OnPropertyChanged(nameof(HasINNErrors));
        OnPropertyChanged(nameof(HasOGRNErrors));

        OnPropertyChanged(nameof(AreNoErrors));
    }

    public override string ToString()
        => $"PharmacyWrapper with PharmacyData - [ {PharmacyData} ]";


    public void BuckupData()
    {
        BackupedName = Name;
        BackupedINN = INN;
        BackupedOGRN = OGRN;
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
            INN = BackupedINN;
            OGRN = BackupedOGRN;

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
        Debug.WriteLine($"BeginEdit : For now the editable PharmacyWrapper = {this}");
    }

    public void CancelEdit()
    {
        Debug.WriteLine("Look at me! Im soooo lazy to implement CancelEdit");
        isModified = false;
    }

    public async void EndEdit()
    {
        Debug.WriteLine($"EndEdit : For now the editable PharmacyWrapper = {this}");
        await _repositoryControllerService.Pharmacies.UpdateAsync(PharmacyData);
    }

    public bool Equals(PharmacyWrapper? other) =>
        Name == other?.Name &&
        INN == other?.INN &&
        OGRN == other?.OGRN;

    #endregion
}