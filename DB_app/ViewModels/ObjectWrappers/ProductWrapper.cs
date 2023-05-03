using CommunityToolkit.Mvvm.ComponentModel;
using DB_app.Core.Contracts.Services;
using DB_app.Models;
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


    private Product _productData = new();


    public Product ProductData
    {
        get => _productData;
        set
        {
            _productData = value;
            ProductMedicine = _productData.Medicine;
            ProductPharmacy = _productData.Pharmacy;
            Quantity = _productData.Quantity;
            Price = _productData.Price;
        }
    }



    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required(ErrorMessage = "Medicine is Required")]
    private Medicine? productMedicine;


    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required(ErrorMessage = "Pharmacy is Required")]
    private Pharmacy? productPharmacy;


    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required(ErrorMessage = "Quantity is Required")]
    [Range(1, int.MaxValue, ErrorMessage = "Please enter a value bigger than {1}")]
    //[RegularExpression("([1-9]+)", ErrorMessage = "Please enter a Number")]
    private int quantity;


    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required(ErrorMessage = "Price is Required")]
    [Range(1, int.MaxValue, ErrorMessage = "Please enter a value bigger than {1}")]
    //[RegularExpression("([1-9]+)", ErrorMessage = "Please enter a Number")]
    private double price;



    // These properties needed for grid table
    public string PharmacyName { get => _productData.Pharmacy.Name; }
    public string MedicineName { get => _productData.Medicine.Name; }
    public string MedicineType { get => _productData.Medicine.Type; }




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
        ProductMedicine == other?.ProductMedicine &&
        ProductMedicine == other?.ProductMedicine &&
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
        IsNew = false;
        return true;
    }



    public void Backup() =>
        _backupData = _productData;


    #endregion




    #region IEditable implementation
    public void BeginEdit()
    {
        IsModified = true;
        IsInEdit = true;
        Backup();
    }

    public void CancelEdit()
    {
        IsInEdit = false;
        IsModified = false;
    }

    public async void EndEdit()
    {
        IsInEdit= false;
        _productData.Pharmacy = ProductPharmacy;
        _productData.Medicine = ProductMedicine;
        _productData.Price = Price;
        _productData.Quantity = Quantity; 
    }


    #endregion
}
