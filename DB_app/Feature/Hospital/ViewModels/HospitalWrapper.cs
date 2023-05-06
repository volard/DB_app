using CommunityToolkit.Mvvm.ComponentModel;
using DB_app.Core.Contracts.Services;
using DB_app.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DB_app.ViewModels;

/// <summary>
/// Provides wrapper for the <see cref="Hospital"/> model class, encapsulating various services for access by the UI.
/// </summary>
public sealed partial class HospitalWrapper : ObservableValidator, IEditableObject
{

    #region Constructors

    /// <summary>
    /// Initialize new HospitalWrapper object
    /// </summary>
    /// <param name="hospital">Hospital model representing by the wrapper</param>
    public HospitalWrapper(Hospital? hospital = null)
    {
       if (hospital == null)
        {
            IsNew = true;
        }
        else { HospitalData = hospital; }
    }

    #endregion


    #region Properties

    private readonly IRepositoryControllerService _repositoryControllerService = App.GetService<IRepositoryControllerService>();

    private Hospital? _backupData;

    private Hospital _hospitalData = new(); 

    public Hospital HospitalData
    {
        get => _hospitalData;
        set
        {
            _hospitalData = value;
            Name_main_doctor = _hospitalData.Name_main_doctor;
            Surename_main_doctor = _hospitalData.Surename_main_doctor;
            Middlename_main_doctor = _hospitalData.Middlename_main_doctor;
            IsActive = _hospitalData.IsActive;
            ObservableLocations = new(HospitalData.Locations);
        }
    }

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required(ErrorMessage = "Maindoctor's name is requireed")]
    private string? _name_main_doctor;

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required(ErrorMessage = "Maindoctor's name is requireed")]
    private string? _middlename_main_doctor;

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required(ErrorMessage = "Maindoctor's name is requireed")]
    private string? _surename_main_doctor;

    [ObservableProperty]
    private bool _isActive;
    
    public int Id { get => HospitalData.Id; }

    public ObservableCollection<HospitalLocation> ObservableLocations = new();

    /// <summary>
    /// Indicates about changes that is not synced with UI DataGrid
    /// </summary>
    [ObservableProperty]
    private bool _isModified = false;

    /// <summary>
    /// Indicates whether its a new object
    /// </summary>
    [ObservableProperty]
    private bool isNew = false;

    /// <summary>
    /// Indicate edit mode
    /// </summary>
    [ObservableProperty]
    private bool _isInEdit = false;

    #endregion



    #region Members

    public override string ToString()
        => $"HospitalWrapper with HospitalData - [ {HospitalData} ]";


    public override bool Equals(object? obj)
    {
        if (obj is not HospitalWrapper other) return false;
        return
            Name_main_doctor == other?.Name_main_doctor &&
            Surename_main_doctor == other?.Surename_main_doctor &&
            Middlename_main_doctor == other?.Middlename_main_doctor;
    }

    #endregion



    #region Modification methods

     public void Backup() => _backupData = _hospitalData;


    /// <summary>
    /// Go back to prevoius data after updating
    /// </summary>
    public async Task Revert()
    {
        if (_backupData != null)
        {
            HospitalData = _backupData;
            await App.GetService<IRepositoryControllerService>().Hospitals.UpdateAsync(_hospitalData);
        }
    }


    public async Task<bool> SaveAsync()
    {
        ValidateAllProperties();
        if (HasErrors) return false;
        EndEdit();
        if (IsNew)
            await App.GetService<IRepositoryControllerService>().Hospitals.InsertAsync(HospitalData);
        else
            await App.GetService<IRepositoryControllerService>().Hospitals.UpdateAsync(HospitalData);
        return true;
    }

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
        Name_main_doctor = _hospitalData.Name_main_doctor;
        Surename_main_doctor = _hospitalData.Surename_main_doctor;
        Middlename_main_doctor = _hospitalData.Middlename_main_doctor;
        IsModified = false;
        IsInEdit = false;
    }


    public async void EndEdit()
    {
        IsInEdit = false;
        _hospitalData.Name_main_doctor = Name_main_doctor;
        _hospitalData.Surename_main_doctor = Surename_main_doctor;
        _hospitalData.Middlename_main_doctor = Middlename_main_doctor;
    }

    
    #endregion
}
