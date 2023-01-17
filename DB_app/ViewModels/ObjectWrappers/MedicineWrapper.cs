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
public partial class MedicineWrapper : ObservableValidator, IEditableObject, IEquatable<MedicineWrapper>
{


    public MedicineWrapper(Medicine medicine)
    {
        MedicineData = medicine;
        ErrorsChanged += Suspect_ErrorsChanged;
        NotifyAboutProperties();
    }

    public MedicineWrapper()
    {
        MedicineData = new();
        ErrorsChanged += Suspect_ErrorsChanged;
        NotifyAboutProperties();
    }

    private void Suspect_ErrorsChanged(object sender, DataErrorsChangedEventArgs e)
    {
        OnPropertyChanged(nameof(Errors));
        OnPropertyChanged(nameof(NameErrors));
        OnPropertyChanged(nameof(TypeErrors));
        OnPropertyChanged(nameof(HasNameErrors));
        OnPropertyChanged(nameof(HasTypeErrors));
        OnPropertyChanged(nameof(AreNoErrors));
    }

    public override string ToString() => $"MedicineWrapper with MedicineData '{Name}' under '{Type}' type";

    #region Properties

    private readonly IRepositoryControllerService _repositoryControllerService
        = App.GetService<IRepositoryControllerService>();


    public Medicine MedicineData { get; set; }


    [Required(ErrorMessage = "Name is Required")]
    public string Name
    {
        get => MedicineData.Name;
        set
        {
            ValidateProperty(value);
            Debug.WriteLine($"I've just validated the name and got this errors: {Errors}");
            if (!GetErrors(nameof(Name)).Any())
            {
                MedicineData.Name = value;
                OnPropertyChanged();
            }
            Debug.WriteLine($"\nfor name property especially: {GetErrors(nameof(Name))}");
        }
    }


    [Required(ErrorMessage = "Type is Required")]
    public string Type
    {
        get => MedicineData.Type;
        set
        {
            ValidateProperty(value);
            Debug.WriteLine($"I've just validated the type and got this errors: {Errors}");
            if (!GetErrors(nameof(Type)).Any())
            {
                MedicineData.Type = value;
                OnPropertyChanged();
            }
            Debug.WriteLine($"\nfor type property especially: {GetErrors(nameof(Name))}");
        }
    }

    public void NotifyAboutProperties()
    {
        OnPropertyChanged(nameof(Name));
        OnPropertyChanged(nameof(Type));
    }


    public string Errors => string.Join(Environment.NewLine, from ValidationResult e in GetErrors(null) select e.ErrorMessage);
    public string NameErrors => string.Join(Environment.NewLine, from ValidationResult e in GetErrors(nameof(Name)) select e.ErrorMessage);
    public string TypeErrors => string.Join(Environment.NewLine, from ValidationResult e in GetErrors(nameof(Type)) select e.ErrorMessage);
    public bool HasNameErrors => GetErrors(nameof(Name)).Any();
    public bool HasTypeErrors => GetErrors(nameof(Type)).Any();
    public bool AreNoErrors => !HasErrors;

    public int Id { get => MedicineData.Id; }

    // TODO implement cancel button on notification popup
    public string? BackupedName;
    public string? BackupedType;


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
        BackupedName = MedicineData.Name;
        BackupedType = MedicineData.Type;
    }

    public void ApplyChanges() => isModified = true;

    public void UndoChanges()
    {
        if (BackupedName != null && BackupedType != null)
        {
            Name = BackupedName;
            Type = BackupedType;
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
        Debug.WriteLine($"BeginEdit : For now the editable medicineWrapper = {MedicineData}");
    }

    public void CancelEdit()
    {
        Debug.WriteLine("Look at me! Im soooo lazy to implement CancelEdit");
        isModified = false;
    }

    public async void EndEdit()
    {
        Debug.WriteLine($"EndEdit : For now the editable medicineWrapper = {MedicineData}");
        await _repositoryControllerService.Medicines.UpdateAsync(MedicineData);
    }

    public bool Equals(MedicineWrapper? other) =>
        Name == other?.Name &&
        Type == other?.Type;

    #endregion
}