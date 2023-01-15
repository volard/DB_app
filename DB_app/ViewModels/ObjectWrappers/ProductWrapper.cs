using CommunityToolkit.Mvvm.ComponentModel;
using DB_app.Core.Contracts.Services;
using DB_app.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.UserDataTasks;

namespace DB_app.ViewModels;

public partial class ProductWrapper : ObservableValidator, IEditableObject, IEquatable<ProductWrapper>
{
    #region Constructors

    public ProductWrapper(Product product)
    {
        ProductData = product;
        ErrorsChanged += Suspect_ErrorsChanged;
        NotifyAboutProperties();
    }

    public ProductWrapper()
    {
        ProductData = new();
        ErrorsChanged += Suspect_ErrorsChanged;
        NotifyAboutProperties();
    }

    #endregion


    #region Properties

    private readonly IRepositoryControllerService _repositoryControllerService
        = App.GetService<IRepositoryControllerService>();


    public Product ProductData { get; set; }


    [Required(ErrorMessage = "Medicine is Required")]
    public Medicine Medicine
    {
        get => ProductData.Medicine;
        set
        {
            ValidateProperty(value);
            if (!GetErrors(nameof(Medicine)).Any())
            {
                ProductData.Medicine = value;
                IsModified = true;
                OnPropertyChanged(nameof(CanSave));
            }
        }
    }



    [Required(ErrorMessage = "Quantity is Required")]
    //[RegularExpression("([1-9]+)", ErrorMessage = "Please enter a Number")]
    public int Quantity
    {
        get => ProductData.Quantity;
        set
        {
            ValidateProperty(value);
            if (!GetErrors(nameof(Quantity)).Any())
            {
                ProductData.Quantity = value;
                OnPropertyChanged();
                IsModified = true;
                OnPropertyChanged(nameof(CanSave));
            }
            Debug.WriteLine("Quantity errors: " + QuantityErrors);
        }
    }


    [Required(ErrorMessage = "Price is Required")]
    //[RegularExpression("([1-9]+)", ErrorMessage = "Please enter a Number")]
    // TODO change number validation to money validation
    public double Price
    {
        get => ProductData.Price;
        set
        {
            ValidateProperty(value);
            if (!GetErrors(nameof(Price)).Any())
            {
                ProductData.Price = value;
                OnPropertyChanged();
                IsModified= true; 
                OnPropertyChanged(nameof(CanSave));
            }
            
        }
    }


    [Required(ErrorMessage = "Pharmacy is Required")]
    public Pharmacy Pharmacy
    {
        get => ProductData.Pharmacy;
        set
        {
            ValidateProperty(value);
            if (!GetErrors(nameof(Pharmacy)).Any())
            {
                ProductData.Pharmacy = value;
                IsModified = true;
                OnPropertyChanged(nameof(CanSave));
            }
        }
    }

    public int Id { get => ProductData.Id; }


    // TODO that looks disgusting. I wonder if functions in xaml bindings works properly for me
    public string Errors
        => string.Join(Environment.NewLine, from ValidationResult e in GetErrors(null) select e.ErrorMessage);
    public string MedicineErrors
        => string.Join(Environment.NewLine, from ValidationResult e in GetErrors(nameof(Medicine)) select e.ErrorMessage);
    public string PharmacyErrors
        => string.Join(Environment.NewLine, from ValidationResult e in GetErrors(nameof(Pharmacy)) select e.ErrorMessage);
    public string PriceErrors
        => string.Join(Environment.NewLine, from ValidationResult e in GetErrors(nameof(Price)) select e.ErrorMessage);
    public string QuantityErrors
        => string.Join(Environment.NewLine, from ValidationResult e in GetErrors(nameof(Quantity)) select e.ErrorMessage);


    public bool HasMedicineErrors
        => GetErrors(nameof(Medicine)).Any();
    public bool HasPharmacyErrors
        => GetErrors(nameof(Pharmacy)).Any();
    public bool HasPriceErrors
        => GetErrors(nameof(Price)).Any();
    public bool HasQuantityErrors
        => GetErrors(nameof(Quantity)).Any();
    public bool AreNoErrors
        => !HasErrors;
    public bool CanSave
        => !HasErrors && (isModified || isNew);


    // TODO implement cancel button on notification popup
    // TODO maybe it will be better to create another Product object instead of 
    // keeping the bunch of backuped properties
    public Medicine? BackupedMedicine;
    public Pharmacy? BackupedPharmacy;
    public double? BackupedPrice;
    public int? BackupedQuantity;


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
        OnPropertyChanged(nameof(Errors));
        OnPropertyChanged(nameof(MedicineErrors));
        OnPropertyChanged(nameof(PharmacyErrors));
        OnPropertyChanged(nameof(PriceErrors));

        OnPropertyChanged(nameof(Medicine));
        OnPropertyChanged(nameof(Pharmacy));
        OnPropertyChanged(nameof(Price));
        OnPropertyChanged(nameof(Quantity));

        OnPropertyChanged(nameof(HasMedicineErrors));
        OnPropertyChanged(nameof(HasPharmacyErrors));
        OnPropertyChanged(nameof(HasPriceErrors));
        OnPropertyChanged(nameof(HasQuantityErrors));

        OnPropertyChanged(nameof(CanSave));
    }

    public void Suspect_ErrorsChanged(object sender, DataErrorsChangedEventArgs e)
    {
        NotifyAboutProperties();
    }

    public override string ToString()
        => $"ProductWrapper with ProductData - [ {ProductData} ]";


    public void BuckupData()
    {
        BackupedMedicine = Medicine;
        BackupedPharmacy = Pharmacy;
        BackupedPrice = Price;
        BackupedQuantity = Quantity;
    }

    public void ApplyChanges() => IsModified = true;

    public void UndoChanges()
    {
        if (
                BackupedMedicine != null &&
                BackupedPharmacy != null &&
                BackupedPrice != null &&
                BackupedQuantity != null
           )
        {
            Medicine = BackupedMedicine;
            Pharmacy = BackupedPharmacy;
            Price = (double)BackupedPrice;
            Quantity = (int)BackupedQuantity;

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
        Debug.WriteLine($"BeginEdit : For now the editable ProductWrapper = {this}");
    }

    public void CancelEdit()
    {
        Debug.WriteLine("Look at me! Im soooo lazy to implement CancelEdit");
        isModified = false;
    }

    public async void EndEdit()
    {
        Debug.WriteLine($"EndEdit : For now the editable ProductWrapper = {this}");
        await _repositoryControllerService.Products.UpdateAsync(ProductData);
    }

    public bool Equals(ProductWrapper? other) =>
        Medicine == other?.Medicine &&
        Pharmacy == other?.Pharmacy &&
        Price == other?.Price &&
        BackupedQuantity == other?.Quantity;

    #endregion

    /// <summary>
    /// City of the current ProductWrapper's data object
    /// </summary>
    public string PharmacyName { get => ProductData.Pharmacy.Name; }

    /// <summary>
    /// Street of the current ProductWrapper's data object
    /// </summary>
    public string MedicineName { get => ProductData.Medicine.Name; }

    /// <summary>
    /// Building of the current ProductWrapper's data object
    /// </summary>
    public string MedicineType { get => ProductData.Medicine.Type; }
}
