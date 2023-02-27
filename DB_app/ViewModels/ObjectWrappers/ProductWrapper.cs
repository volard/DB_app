using CommunityToolkit.Mvvm.ComponentModel;
using DB_app.Core.Contracts.Services;
using DB_app.Entities;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DB_app.ViewModels;

/// <summary>
/// Provides wrapper for the <see cref="Product"/> model class, encapsulating various services for access by the UI.
/// </summary>
public sealed partial class ProductWrapper : ObservableValidator, IEditableObject
{
    #region Constructors

    public ProductWrapper(Product? product = null)
    {
        if (product == null)
        {
            IsNew = true;
        }
        else { ProductData = product; }
    }


    #endregion





    #region Properties

    private readonly IRepositoryControllerService _repositoryControllerService
        = App.GetService<IRepositoryControllerService>();


    private Product _productData = null!;


    public Product ProductData
    {
        get => _productData;
        set
        {
            _productData = value;
            Medicine = _productData.Medicine;
        }
    }


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
            }
        }
    }



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
            }
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
            }
        }
    }

    public int Id { get => ProductData.Id; }

    private Product? _backupData;
   

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
    private bool _isNew = false;

    #endregion





    #region Members

    public bool Equals(ProductWrapper? other) =>
        Medicine == other?.Medicine &&
        Pharmacy == other?.Pharmacy &&
        Price == other?.Price &&
        Quantity == other?.Quantity;

    public override string ToString()
        => $"ProductWrapper with ProductData - [ {ProductData} ]";


    #endregion





    #region Modification methods

    /// <summary>
    /// Go back to prevoius data after updating
    /// </summary>
    public async Task Revert()
    {
        if (_backupData != null)
        {
            ProductData = _backupData;
            await App.GetService<IRepositoryControllerService>().Products.UpdateAsync(ProductData);
        }
    }


     public async Task<bool> SaveAsync()
    {
        ValidateAllProperties();
        if (HasErrors) return false;
        EndEdit();
        if (IsNew)
        {
            await App.GetService<IRepositoryControllerService>().Products.InsertAsync(ProductData);
        }
        else
        {
            await App.GetService<IRepositoryControllerService>().Products.UpdateAsync(ProductData);
        }
        return true;
    }



    public void Backup() =>
        _backupData = _productData;


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
        await _repositoryControllerService.Products.UpdateAsync(ProductData);
    }


    #endregion
}
