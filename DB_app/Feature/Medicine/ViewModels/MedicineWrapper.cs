using CommunityToolkit.Mvvm.ComponentModel;
using DB_app.Core.Contracts.Services;
using DB_app.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DB_app.ViewModels;

/// <summary>
/// Provides wrapper for the <see cref="Medicine"/> model class, 
/// encapsulating various services for access by the UI.
/// </summary>
public sealed partial class MedicineWrapper : 
    ObservableValidator, IEditableObject
{
    /**************************************/
    #region Constructors
    
    public MedicineWrapper(Medicine? medicine = null)
    {
        if (medicine == null)
        {
            IsNew = true;
        }
        else { MedicineData = medicine; }

        InitFields();
    }

    #endregion
    /**************************************/



    /**************************************/
    #region Properties
    

    private readonly IRepositoryControllerService _repositoryControllerService = App.GetService<IRepositoryControllerService>();

    private Medicine? _backupData;

    private Medicine MedicineData { get; set; } = new Medicine();


    [ObservableProperty]
    [NotifyDataErrorInfo]
    [NotifyPropertyChangedFor(nameof(IsModified))]
    [Required(ErrorMessage = "Name is Required")]
    private string? _name;


    [ObservableProperty]
    [NotifyDataErrorInfo]
    [NotifyPropertyChangedFor(nameof(IsModified))]
    [Required(ErrorMessage = "Type is Required")]
    private string? _type;


    public int Id { get => MedicineData.Id; }


    /// <summary>
    /// Indicates about changes that is not synced with UI DataGrid
    /// </summary>
    public bool IsModified
    {
        get =>
                Name != MedicineData.Name ||
                Type != MedicineData.Type;
    }


    /// <summary>
    /// indicates whether its a new object
    /// </summary>
    [ObservableProperty]
    private bool _isNew = false;


    ///<summary>
    /// Indicate edit mode
    /// </summary>
    [ObservableProperty]
    private bool _isInEdit = false;

    #endregion
    /**************************************/


    

    /**************************************/
    #region Members
    
    public bool Equals(object? obj)
    {
        if (obj is not MedicineWrapper other) return false;
        return 
            Name == other?.Name &&
            Type == other?.Type;
    }


    public override string ToString() => $"MedicineWrapper with MedicineData '{Name}' under '{Type}' type";

    private void InitFields()
    {
        Name = MedicineData.Name;
        Type = MedicineData.Type;
    }

    #endregion
    /**************************************/



    /**************************************/
    #region Modification methods
    /**************************************/


    /// <summary>
    /// Go back to previous data after updating
    /// </summary>
    public async Task Revert()
    {
        if (_backupData != null)
        {
            MedicineData = _backupData;
            await _repositoryControllerService.Medicines.UpdateAsync(MedicineData);
        }
    }


     public async Task<bool> SaveAsync()
    {
        ValidateAllProperties();
        if (HasErrors) return false;
        EndEdit();
        if (IsNew)
        {
            await _repositoryControllerService.Medicines.InsertAsync(MedicineData);
        }
        else
        {
            await _repositoryControllerService.Medicines.UpdateAsync(MedicineData);
        }
        return true;
    }



    public void Backup() =>
        _backupData = MedicineData;


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
        IsInEdit = false;
    }

    public void EndEdit()
    {
        IsInEdit = false;
        OnPropertyChanged(nameof(IsModified));

        // NOTE the underlying code relays on preliminary data validation
        MedicineData.Name = Name;
        MedicineData.Type = Type;
    }


    #endregion

}