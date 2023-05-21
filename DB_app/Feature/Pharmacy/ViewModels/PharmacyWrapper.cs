using CommunityToolkit.Mvvm.ComponentModel;
using DB_app.Core.Contracts.Services;
using DB_app.Models;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using DB_app.Services.Messages;
using DB_app.Helpers;
using System.Collections.Specialized;

namespace DB_app.ViewModels;

/// <summary>
/// Provides wrapper for the <see cref="Pharmacy"/> model class, encapsulating various services for access by the UI.
/// </summary>

public sealed partial class PharmacyWrapper : ObservableValidator, IEditableObject
{
    /**************************************/
    #region Constructors

    public PharmacyWrapper(Pharmacy? pharmacy = null)
    {
        if (pharmacy == null)
        {
            IsNew = true;
        }
        else 
        { 
            PharmacyData = pharmacy; 
        }

        InitFields();
        ObservableLocations.CollectionChanged += (object? sender, NotifyCollectionChangedEventArgs e) =>
        {
            ValidateProperty(ObservableLocations, nameof(ObservableLocations));
            OnPropertyChanged(nameof(IsModified));
        };
    }


    #endregion
    /**************************************/



    /**************************************/
    #region Properties


    private readonly IRepositoryControllerService _repositoryControllerService = App.GetService<IRepositoryControllerService>();


    /// <summary>
    /// Indicates about changes that is not synced with UI DataGrid
    /// </summary>
    public bool IsModified
    {
        get
        {
            bool isDifferent = CollectionsHelper.IsDifferent(ObservableLocations.ToList(), PharmacyData.Locations);

            return
                Name == PharmacyData.Name ||
                IsActive == PharmacyData.IsActive || isDifferent;
        }
    }


    public Pharmacy PharmacyData { get; set; } = new();


    /// <summary>
    /// Name of productPharmacy
    /// </summary>
    [ObservableProperty]
    [NotifyDataErrorInfo]
    [NotifyPropertyChangedFor(nameof(IsModified))]
    [Required(ErrorMessage = "City is Required")]
    private string? _name;


    /// <summary>
    /// Indicates whether its closed or opened orginization
    /// </summary>
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsModified))]
    private bool _isActive;


    [ObservableProperty]
    private bool _isInEdit = false;


    /// <summary>
    /// Indicates whether its a new object
    /// </summary>
    [ObservableProperty]
    private bool _isNew = false;


    [MinLength(1, ErrorMessage = "Organisation have to have at least one address")]
    [NotifyDataErrorInfo]
    [NotifyPropertyChangedFor(nameof(IsModified))]
    [ObservableProperty]
    private ObservableCollection<PharmacyLocation> _observableLocations = new();


    public int Id { get => PharmacyData.Id; }


    private Pharmacy? _backupData;




    #endregion
    /**************************************/



    /**************************************/
    #region Members


    private void InitFields()
    {
        Name = PharmacyData.Name;
        IsActive = PharmacyData.IsActive;
        ObservableLocations = new(PharmacyData.Locations);
    }


    public override string ToString()
        => $"PharmacyWrapper with PharmacyData - [ {PharmacyData} ]";


    public bool Equals(PharmacyWrapper? other) =>
        Name == other?.Name ||
        IsActive == other?.IsActive;


    #endregion
    /**************************************/



    /**************************************/
    #region Modification methods
    

    public void Backup() => _backupData = PharmacyData;


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
        if (IsNew)
        {
            await App.GetService<IRepositoryControllerService>().Pharmacies.InsertAsync(PharmacyData);
        }
        else
        {
            await App.GetService<IRepositoryControllerService>().Pharmacies.UpdateAsync(PharmacyData);
        }


        // Sync with grid
        WeakReferenceMessenger.Default.Send(new AddRecordMessage<PharmacyWrapper>(this));
        return true;
    }



    #endregion
    /**************************************/




    /**************************************/
    #region IEditable implementation


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
        PharmacyData.Name = Name!;
        PharmacyData.IsActive = IsActive;
        PharmacyData.Locations = ObservableLocations.ToList();
    }


    #endregion
    /**************************************/
}