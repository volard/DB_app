using CommunityToolkit.Mvvm.ComponentModel;
using DB_app.Core.Contracts.Services;
using DB_app.Entities;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DB_app.ViewModels;

/// <summary>
/// Provides wrapper for the Order model class, encapsulating various services for access by the UI.
/// </summary>
public partial class MedicineWrapper : ObservableValidator, IEditableObject, IEquatable<MedicineWrapper>
{

    public MedicineWrapper(Medicine? medicine = null)
    {
        MedicineData = medicine ?? new();
        ErrorsChanged += Suspect_ErrorsChanged;

    }


    private void Suspect_ErrorsChanged(object sender, DataErrorsChangedEventArgs e)
    {
        NotifyAboutAllProperties();
    }

    public void NotifyAboutAllProperties() =>
        OnPropertyChanged(string.Empty);

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
            if (!GetErrors(nameof(Name)).Any())
            {
                MedicineData.Name = value;
                OnPropertyChanged();
            }
        }
    }


    [Required(ErrorMessage = "Type is Required")]
    public string Type
    {
        get => MedicineData.Type;
        set
        {
            ValidateProperty(value);
            if (!GetErrors(nameof(Type)).Any())
            {
                MedicineData.Type = value;
                OnPropertyChanged();
            }
        }
    }


    public string Errors => string.Join(Environment.NewLine, from ValidationResult e in GetErrors(null) select e.ErrorMessage);
    public string NameErrors => string.Join(Environment.NewLine, from ValidationResult e in GetErrors(nameof(Name)) select e.ErrorMessage);
    public string TypeErrors => string.Join(Environment.NewLine, from ValidationResult e in GetErrors(nameof(Type)) select e.ErrorMessage);
    public bool HasNameErrors => GetErrors(nameof(Name)).Any();
    public bool HasTypeErrors => GetErrors(nameof(Type)).Any();
    public bool AreNoErrors => !HasErrors;

    public int Id { get => MedicineData.Id; }

    // TODO implement cancel button on notification popup
    private Medicine? BackupData;


    /// <summary>
    /// Indicates about changes that is not synced with UI DataGrid
    /// </summary>
    [ObservableProperty]
    private bool isModified = false;

    /// <summary>
    /// indicates whether its a new object
    /// </summary>
    public bool IsNew { get; private set; } = false;

    #endregion


    #region Modification methods

    public void BuckupData()
    {
        BackupData = MedicineData;   
    }

    public void ApplyChanges() => isModified = true;

    public void UndoChanges()
    {
        if (BackupData != null)
        {
            BackupData = MedicineData;
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
        await _repositoryControllerService.Medicines.UpdateAsync(MedicineData);
    }

    public bool Equals(MedicineWrapper? other) =>
        Name == other?.Name &&
        Type == other?.Type;

    #endregion

    public override string ToString() => $"MedicineWrapper with MedicineData '{Name}' under '{Type}' type";
}