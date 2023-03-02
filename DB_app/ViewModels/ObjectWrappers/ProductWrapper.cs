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
            PharmacyName = _productData.Pharmacy.Name;
            MedicineName = _productData.Medicine.Name;
            Medicine = _productData.Medicine;
            Pharmacy = _productData.Pharmacy;
            MedicineType = _productData.Medicine.Type;
        }
    }





    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required(ErrorMessage = "Medicine is Required")]
    private Medicine? medicine;


    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required]
    private string? pharmacyName;

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required]
    private string? medicineName;

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required]
    private string? medicineType;


    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required(ErrorMessage = "Quantity is Required")]
    //[RegularExpression("([1-9]+)", ErrorMessage = "Please enter a Number")]
    private int? quantity;


    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required(ErrorMessage = "Price is Required")]
    //[RegularExpression("([1-9]+)", ErrorMessage = "Please enter a Number")]
    private double? price;


    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required(ErrorMessage = "Pharmacy is Required")]
    private Pharmacy? pharmacy;



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
    private bool isNew = false;

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
