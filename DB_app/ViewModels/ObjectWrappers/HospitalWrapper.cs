using CommunityToolkit.Mvvm.ComponentModel;
using DB_app.Core.Contracts.Services;
using DB_app.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace DB_app.ViewModels;

public partial class HospitalWrapper : ObservableValidator, IEditableObject, IEquatable<HospitalWrapper>
{

    #region Constructors

    public HospitalWrapper(Hospital hospital)
    {
        HospitalData = hospital;
        ErrorsChanged += Suspect_ErrorsChanged;
        NotifyAboutProperties();
    }

    public HospitalWrapper()
    {
        HospitalData = new();
        ErrorsChanged += Suspect_ErrorsChanged;
        NotifyAboutProperties();
    }

    #endregion


    #region Properties

    private readonly IRepositoryControllerService _repositoryControllerService
        = App.GetService<IRepositoryControllerService>();


    public Hospital HospitalData { get; set; }


    [Required(ErrorMessage = "Maindoctor's name is Required")]
    public string Name_main_doctor
    {
        get => HospitalData.Name_main_doctor;
        set
        {
            ValidateProperty(value);
            if (!GetErrors(nameof(Name_main_doctor)).Any())
            {
                HospitalData.Name_main_doctor = value;
                OnPropertyChanged();
            }
        }
    }


    [Required(ErrorMessage = "Maindoctor's surename is Required")]
    public string Surename_main_doctor
    {
        get => HospitalData.Surename_main_doctor;
        set
        {
            ValidateProperty(value);
            if (!GetErrors(nameof(Surename_main_doctor)).Any())
            {
                HospitalData.Surename_main_doctor = value;
                OnPropertyChanged();
            }
        }
    }


    [Required(ErrorMessage = "Maindoctor's middlename is Required")]
    public string Middlename_main_doctor
    {
        get => HospitalData.Middlename_main_doctor;
        set
        {
            ValidateProperty(value);
            if (!GetErrors(nameof(Middlename_main_doctor)).Any())
            {
                HospitalData.Middlename_main_doctor = value;
                OnPropertyChanged();
            }
        }
    }


    [Required(ErrorMessage = "INN is Required")]
    [RegularExpression("([0-9]+)", ErrorMessage = "Please enter a Number")]
    public string INN
    {
        get => HospitalData.INN;
        set
        {
            ValidateProperty(value);
            if (!GetErrors(nameof(INN)).Any())
            {
                HospitalData.INN = value;
                OnPropertyChanged();
            }
        }
    }


    [Required(ErrorMessage = "OGRN is Required")]
    [RegularExpression("([0-9]+)", ErrorMessage = "Please enter a Number")]
    public string OGRN
    {
        get => HospitalData.OGRN;
        set
        {
            ValidateProperty(value);
            if (!GetErrors(nameof(OGRN)).Any())
            {
                HospitalData.OGRN = value;
                OnPropertyChanged();
            }
        }
    }


    public ObservableCollection<Address> ObservableAddresses
    {
        get => new(HospitalData.Addresses);
        set
        {
            HospitalData.Addresses = value.ToList();
            IsModified = true;
            OnPropertyChanged();
        }
    }


    public int Id { get => HospitalData.Id; }


    // TODO that looks disgusting. I wonder if functions in xaml bindings works properly for me
    public string Errors
        => string.Join(Environment.NewLine, from ValidationResult e in GetErrors(null) select e.ErrorMessage);
    public string Name_main_doctorErrors
        => string.Join(Environment.NewLine, from ValidationResult e in GetErrors(nameof(Name_main_doctor)) select e.ErrorMessage);
    public string Surename_main_doctorErrors
        => string.Join(Environment.NewLine, from ValidationResult e in GetErrors(nameof(Surename_main_doctor)) select e.ErrorMessage);
    public string Middlename_main_doctorErrors
        => string.Join(Environment.NewLine, from ValidationResult e in GetErrors(nameof(Middlename_main_doctor)) select e.ErrorMessage);
    public string INNErrors
        => string.Join(Environment.NewLine, from ValidationResult e in GetErrors(nameof(INN)) select e.ErrorMessage);
    public string OGRNErrors
        => string.Join(Environment.NewLine, from ValidationResult e in GetErrors(nameof(OGRN)) select e.ErrorMessage);


    public bool HasName_main_doctorErrors
        => GetErrors(nameof(Name_main_doctor)).Any();
    public bool HasSurename_main_doctorErrors
        => GetErrors(nameof(Surename_main_doctor)).Any();
    public bool HasMiddlename_main_doctorErrors
        => GetErrors(nameof(Middlename_main_doctor)).Any();
    public bool HasINNErrors
        => GetErrors(nameof(INN)).Any();
    public bool HasOGRNErrors
        => GetErrors(nameof(OGRN)).Any();
    public bool AreNoErrors
        => !HasErrors;


    // TODO implement cancel button on notification popup
    // TODO maybe it will be better to create another Hospital object instead of 
    // keeping the bunch of backuped properties
    public string? BackupedName_main_doctor;
    public string? BackupedSurename_main_doctor;
    public string? BackupedMiddlename_main_doctor;
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
        OnPropertyChanged(nameof(Name_main_doctor));
        OnPropertyChanged(nameof(Surename_main_doctor));
        OnPropertyChanged(nameof(Middlename_main_doctor));
        OnPropertyChanged(nameof(INN));
        OnPropertyChanged(nameof(OGRN));
        OnPropertyChanged(nameof(ObservableAddresses));
    }

    public void NotifyAboutAddressesChanged() =>
        OnPropertyChanged(nameof(ObservableAddresses));

    private void Suspect_ErrorsChanged(object sender, DataErrorsChangedEventArgs e)
    {
        OnPropertyChanged(nameof(Errors));
        OnPropertyChanged(nameof(Name_main_doctorErrors));
        OnPropertyChanged(nameof(Surename_main_doctorErrors));
        OnPropertyChanged(nameof(Middlename_main_doctorErrors));
        OnPropertyChanged(nameof(INNErrors));
        OnPropertyChanged(nameof(OGRNErrors));

        OnPropertyChanged(nameof(Name_main_doctor));
        OnPropertyChanged(nameof(Surename_main_doctor));
        OnPropertyChanged(nameof(Middlename_main_doctor));
        OnPropertyChanged(nameof(INN));
        OnPropertyChanged(nameof(OGRN));

        OnPropertyChanged(nameof(HasName_main_doctorErrors));
        OnPropertyChanged(nameof(HasSurename_main_doctorErrors));
        OnPropertyChanged(nameof(HasMiddlename_main_doctorErrors));
        OnPropertyChanged(nameof(HasINNErrors));
        OnPropertyChanged(nameof(HasOGRNErrors));

        OnPropertyChanged(nameof(AreNoErrors));
    }

    public override string ToString()
        => $"HospitalWrapper with HospitalData - [ {HospitalData} ]";


    public void BuckupData()
    {
        BackupedName_main_doctor        = Name_main_doctor;
        BackupedSurename_main_doctor    = Surename_main_doctor;
        BackupedMiddlename_main_doctor  = Middlename_main_doctor;
        BackupedINN                     = INN;
        BackupedOGRN                    = OGRN;
    }

    public void ApplyChanges() => isModified = true;

    public void UndoChanges()
    {
        if (
                BackupedName_main_doctor        != null &&
                BackupedSurename_main_doctor    != null &&
                BackupedMiddlename_main_doctor  != null &&
                BackupedINN                     != null &&
                BackupedOGRN                    != null
           )
        {
            Name_main_doctor        = BackupedName_main_doctor;
            Surename_main_doctor    = BackupedSurename_main_doctor;
            Middlename_main_doctor  = BackupedMiddlename_main_doctor;
            INN                     = BackupedINN;
            OGRN                    = BackupedOGRN;

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
        Debug.WriteLine($"BeginEdit : For now the editable HospitalWrapper = {this}");
    }

    public void CancelEdit()
    {
        Debug.WriteLine("Look at me! Im soooo lazy to implement CancelEdit");
        isModified = false;
    }

    public async void EndEdit()
    {
        Debug.WriteLine($"EndEdit : For now the editable HospitalWrapper = {this}");
        await _repositoryControllerService.Hospitals.UpdateAsync(HospitalData);
    }

    public bool Equals(HospitalWrapper? other) =>
        Name_main_doctor        == other?.Name_main_doctor          &&
        Surename_main_doctor    == other?.Surename_main_doctor      &&
        Middlename_main_doctor  == other?.Middlename_main_doctor    &&
        INN                     == other?.INN                       &&
        OGRN                    == other?.OGRN;

    #endregion
}
