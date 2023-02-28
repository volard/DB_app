using CommunityToolkit.Mvvm.ComponentModel;
using DB_app.Core.Contracts.Services;
using DB_app.Entities;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Diagnostics;

namespace DB_app.ViewModels;

/// <summary>
/// Provides wrapper for the <see cref="Pharmacy"/> model class, encapsulating various services for access by the UI.
/// </summary>

public sealed partial class PharmacyWrapper : ObservableValidator, IEditableObject
{

    #region Constructors
    public PharmacyWrapper(Pharmacy? pharmacy = null)
    {
        if (pharmacy == null)
        {
            IsNew = true;
            PharmacyData = new();
        }
        else { PharmacyData = pharmacy; }
    }


    #endregion






    #region Properties

    private readonly IRepositoryControllerService _repositoryControllerService
        = App.GetService<IRepositoryControllerService>();




    private Pharmacy _pharmacyData = null!;

    public Pharmacy PharmacyData 
    {
        get => _pharmacyData;
        set 
        {   
            _pharmacyData = value;
            Name = _pharmacyData.Name;
        } 
    }


    /// <summary>
    /// Name of pharmacy
    /// </summary>
    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required(ErrorMessage = "City is Required")]
    private string? _name;


    /// <summary>
    /// Indicates whether its closed or opened orginization
    /// </summary>
    [ObservableProperty]
    private bool _isActive;

    /// <summary>
    /// Indicates about changes that is not synced with UI DataGrid
    /// </summary>
    [ObservableProperty]
    private bool isModified = false;


    [ObservableProperty]
    private bool isInEdit = false;

    /// <summary>
    /// Indicates whether its a new object
    /// </summary>
    [ObservableProperty]
    private bool isNew = false;


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


    private Pharmacy? _backupData;




    #endregion






    #region Members

     public override string ToString()
        => $"PharmacyWrapper with PharmacyData - [ {PharmacyData} ]";

    public bool Equals(PharmacyWrapper? other) =>
        Name == other?.Name;

    #endregion





    #region Modification methods


   

    public void Backup()
    {
    }

    /// <summary>
    /// Go back to prevoius data after updating
    /// </summary>
    public async Task Revert()
    {
        if (_backupData != null)
        {
            PharmacyData = _backupData;
            await App.GetService<IRepositoryControllerService>().Pharmacies.UpdateAsync(PharmacyData);
        }
    }

      public async Task<bool> SaveAsync()
    {
        ValidateAllProperties();
        if (HasErrors) return false;
        EndEdit();
        if (!isNew)
        {
            await App.GetService<IRepositoryControllerService>().Pharmacies.UpdateAsync(PharmacyData);
        }
        else
        {
            await App.GetService<IRepositoryControllerService>().Pharmacies.InsertAsync(PharmacyData);
        }
        return true;
    }



    #endregion






    #region IEditable implementation
    public void BeginEdit()
    {
        IsModified = true;
        Backup();
    }

    public void CancelEdit()
    {
        
        IsModified = false;
    }

    public async void EndEdit()
    {
        await _repositoryControllerService.Pharmacies.UpdateAsync(PharmacyData);
    }


    #endregion
}