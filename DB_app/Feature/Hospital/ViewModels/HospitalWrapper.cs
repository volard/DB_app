﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using DB_app.Core.Contracts.Services;
using DB_app.Models;
using DB_app.Services.Messages;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace DB_app.ViewModels;


/// <summary>
/// Provides wrapper for the <see cref="Hospital"/> model class, encapsulating various services for access by the UI.
/// </summary>
public sealed partial class HospitalWrapper : ObservableValidator, IEditableObject
{

    /**************************************/
    #region Constructors
    /**************************************/

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
        else 
        { 
            HospitalData = hospital;
        }

        InitFields();
        ObservableLocations.CollectionChanged += ObservableLocations_CollectionChanged;
    }

    public void ObservableLocations_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        ValidateProperty(ObservableLocations, nameof(ObservableLocations));
        OnPropertyChanged(nameof(IsModified));
    }

    #endregion


    /**************************************/
    #region Properties
    /**************************************/

    private readonly IRepositoryControllerService _repositoryControllerService = App.GetService<IRepositoryControllerService>();

    private Hospital? _backupData;

    private void InitFields()
    {
        Name_main_doctor = HospitalData.Name_main_doctor;
        Surename_main_doctor = HospitalData.Surename_main_doctor;
        Middlename_main_doctor = HospitalData.Middlename_main_doctor;
        IsActive = HospitalData.IsActive;
        ObservableLocations = new(HospitalData.Locations);
    }

    public Hospital HospitalData { get; set; } = new();

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [MinLength(2)]
    [NotifyPropertyChangedFor(nameof(IsModified))]
    [Required(ErrorMessage = "Maindoctor's name is requireed")]
    private string? _name_main_doctor;


    [ObservableProperty]
    [NotifyDataErrorInfo]
    [MinLength(2)]
    [NotifyPropertyChangedFor(nameof(IsModified))]
    [Required(ErrorMessage = "Maindoctor's name is requireed")]
    private string? _middlename_main_doctor;


    [ObservableProperty]
    [NotifyDataErrorInfo]
    [MinLength(2)]
    [NotifyPropertyChangedFor(nameof(IsModified))]
    [Required(ErrorMessage = "Maindoctor's name is requireed")]
    private string? _surename_main_doctor;


    [MinLength(1, ErrorMessage = "Organisation have to have at least one address")]
    [NotifyDataErrorInfo]
    [NotifyPropertyChangedFor(nameof(IsModified))]
    [ObservableProperty]
    private ObservableCollection<HospitalLocation> _observableLocations = new();
    

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsModified))]
    private bool _isActive;


    public int Id { get => HospitalData.Id; }


    // TODO allocate this in new Collecion extension class
    static public bool IsDifferent<T>(List<T> lhs, List<T> rhs)
    {
        if(lhs.Count != rhs.Count) return true;
        for(int i = 0; i < lhs.Count; i++)
        {
            if (!lhs.ElementAt(i).Equals(rhs.ElementAt(i))) return true;
        }
        return false;
    }


    /// <summary>
    /// Indicates about changes that is not synced with UI DataGrid
    /// </summary>
    public bool IsModified
    {
        get
        {
            bool isDifferent = IsDifferent(ObservableLocations.ToList(), HospitalData.Locations);

            return
                Name_main_doctor != HospitalData.Name_main_doctor ||
                Surename_main_doctor != HospitalData.Surename_main_doctor ||
                Middlename_main_doctor != HospitalData.Middlename_main_doctor || isDifferent;
        }
    }

    
    /// <summary>
    /// Indicates whether its a new object
    /// </summary>
    [ObservableProperty]
    private bool _isNew = false;

    /// <summary>
    /// Indicates edit mode
    /// </summary>
    [ObservableProperty]
    private bool _isInEdit = false;

    #endregion


    /**************************************/
    #region Members
    /**************************************/

    public override string ToString() => $"HospitalWrapper with HospitalData - [ {HospitalData} ]";


    public override bool Equals(object? obj)
    {
        if (obj is HospitalWrapper otherWrapper)
        {
            return
                Name_main_doctor       == otherWrapper?.Name_main_doctor &&
                Surename_main_doctor   == otherWrapper?.Surename_main_doctor &&
                Middlename_main_doctor == otherWrapper?.Middlename_main_doctor &&
                ObservableLocations    == otherWrapper?.ObservableLocations;
        }
        return false;
    }

    #endregion


    /**************************************/
    #region Modification methods
    /**************************************/

    public void Backup() => _backupData = HospitalData;


    /// <summary>
    /// Go back to prevoius data after updating
    /// </summary>
    public async Task RevertAsync()
    {
        if (_backupData != null)
        {
            HospitalData = _backupData;
            await App.GetService<IRepositoryControllerService>().Hospitals.UpdateAsync(HospitalData);
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
        IsNew = false;

        // Sync with grid
        WeakReferenceMessenger.Default.Send(new AddRecordMessage<HospitalWrapper>(this));
        return true;
    }

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
        HospitalData.Name_main_doctor = Name_main_doctor!;
        HospitalData.Surename_main_doctor = Surename_main_doctor!;
        HospitalData.Middlename_main_doctor = Middlename_main_doctor!;
        HospitalData.IsActive = IsActive;
        HospitalData.Locations = ObservableLocations.ToList();
    }

    
    #endregion
}
