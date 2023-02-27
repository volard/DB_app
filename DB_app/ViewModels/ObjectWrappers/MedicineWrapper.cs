using CommunityToolkit.Mvvm.ComponentModel;
using DB_app.Core.Contracts.Services;
using DB_app.Entities;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DB_app.ViewModels;

/// <summary>
/// Provides wrapper for the <see cref="Medicine"/> model class, encapsulating various services for access by the UI.
/// </summary>
public sealed partial class MedicineWrapper : ObservableValidator, IEditableObject
{

    #region Constructors

    public MedicineWrapper(Medicine? medicine = null)
    {
        if (medicine == null)
        {
            IsNew = true;
            MedicineData = new();
        }
        else { MedicineData = medicine; }
    }

    #endregion





    #region Properties

    private readonly IRepositoryControllerService _repositoryControllerService
        = App.GetService<IRepositoryControllerService>();


    private Medicine _medicineData = null!;


    private Medicine? _backupData;

    public Medicine MedicineData
    {
        get => _medicineData;
        set
        {
            _medicineData = value;
            Name = _medicineData.Name;
            Type = _medicineData.Type;
        }
    }



    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required(ErrorMessage = "Name is Required")]
    private string? _name;


    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required(ErrorMessage = "Type is Required")]
    private string? _type;


    public int Id { get => MedicineData.Id; }

    // TODO implement cancel button on notification popup
    private Medicine? BackupData;


    /// <summary>
    /// Indicates about changes that is not synced with UI DataGrid
    /// </summary>
    [ObservableProperty]
    private bool _isModified = false;

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




    #region Members

     public bool Equals(object? obj)
    {
        if (obj is not MedicineWrapper other) return false;
        return 
            Name == other?.Name &&
            Type == other?.Type;
    }


    public override string ToString() => $"MedicineWrapper with MedicineData '{Name}' under '{Type}' type";


    #endregion





    #region Modification methods



    /// <summary>
    /// Go back to prevoius data after updating
    /// </summary>
    public async Task Revert()
    {
        if (_backupData != null)
        {
            MedicineData = _backupData;
            await App.GetService<IRepositoryControllerService>().Medicines.UpdateAsync(_medicineData);
        }
    }


     public async Task<bool> SaveAsync()
    {
        ValidateAllProperties();
        if (HasErrors) return false;
        EndEdit();
        if (IsNew)
        {
            await App.GetService<IRepositoryControllerService>().Medicines.InsertAsync(MedicineData);
        }
        else
        {
            await App.GetService<IRepositoryControllerService>().Medicines.UpdateAsync(MedicineData);
        }
        return true;
    }



    public void Backup() =>
        _backupData = _medicineData;


    #endregion





    #region IEditable implementation


    public void BeginEdit()
    {
        this.IsInEdit = true;
        OnPropertyChanged(nameof(IsInEdit));
        Backup();
    }

    public void CancelEdit()
    {
        Name = _medicineData.Name;
        Type = _medicineData.Type;
        IsModified = false;
        IsModified = false;
    }

    public async void EndEdit()
    {
        IsInEdit = false;
        _medicineData.Name = Name;
        _medicineData.Type = Type;
    }


    #endregion

}