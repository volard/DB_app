using CommunityToolkit.Mvvm.ComponentModel;
using DB_app.Core.Contracts.Services;
using DB_app.Helpers;
using DB_app.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DB_app.ViewModels;

/// <summary>
/// Provides wrapper for the <see cref="Product"/> model class, encapsulating various services for access by the UI.
/// </summary>
public sealed partial class ProductWrapper : ObservableValidator, IEditableObject
{
    /**************************************/
    #region Constructors
    

    public ProductWrapper(Product? product = null)
    {
        if (product == null)
        {
            IsNew = true;
        }
        else 
        { 
            ProductData = product; 
        }

        InitFields();
    }

    #endregion
    /**************************************/




    /**************************************/
    #region Constants

    private readonly IRepositoryControllerService _repositoryControllerService = App.GetService<IRepositoryControllerService>();

    #endregion
    /**************************************/





    /**************************************/
    #region Fields

    private Product? _backupData;

    #endregion
    /**************************************/






    /**************************************/
    #region Properties


    public Product ProductData { get; set; } = new();


    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required(ErrorMessage = "Medicine is Required")]
    private Medicine? _productMedicine;


    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required(ErrorMessage = "Pharmacy is Required")]
    private Pharmacy? _productPharmacy;


    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required(ErrorMessage = "Quantity is Required")]
    [Range(0, int.MaxValue, ErrorMessage = "Please enter a value bigger than {1}")]
    //[RegularExpression("([1-9]+)", ErrorMessage = "Please enter a Number")]
    private int _quantity = 0;


    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required(ErrorMessage = "Price is Required")]
    [Range(0, int.MaxValue, ErrorMessage = "Please enter a value bigger than {1}")]
    //[RegularExpression("([1-9]+)", ErrorMessage = "Please enter a Number")]
    private double _price = 0;


    // These properties needed for grid table
    public string PharmacyName { get => ProductData.Pharmacy.Name; }
    public string MedicineName { get => ProductData.Medicine.Name; }
    public string MedicineType { get => ProductData.Medicine.Type; }


    public int Id { get => ProductData.Id; }



    /// <summary>
    /// Indicates about changes that is not synced with UI DataGrid
    /// </summary>
    public bool IsModified
    {
        get
        {
            return
                 != HospitalData.Name_main_doctor ||
                Surename_main_doctor != HospitalData.Surename_main_doctor ||
                Middlename_main_doctor != HospitalData.Middlename_main_doctor || isDifferent;
        }
    }

    [ObservableProperty]
    private bool _isInEdit = false;

    /// <summary>
    /// Indicates whether its a new object
    /// </summary>
    [ObservableProperty]
    private bool _isNew = false;

    #endregion
    /**************************************/



    /**************************************/
    #region Methods


    public bool Equals(ProductWrapper? other) =>
        ProductMedicine == other?.ProductMedicine &&
        ProductMedicine == other?.ProductMedicine &&
        Price == other?.Price &&
        Quantity == other?.Quantity;

    public override string ToString()
        => $"ProductWrapper with ProductData - [ {ProductData} ]";


    public void InitFields()
    {
        ProductMedicine = ProductData.Medicine;
        ProductPharmacy = ProductData.Pharmacy;
        Quantity = ProductData.Quantity;
        Price = ProductData.Price;
    }


    #endregion
    /**************************************/



    /**************************************/
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
        _backupData = ProductData;


    #endregion
    /**************************************/



    /**************************************/
    #region IEditable implementation
    


    public void BeginEdit()
    {
        IsInEdit = true;
        Backup();
    }

    public void CancelEdit()
    {
        IsInEdit = false;
    }

    public async void EndEdit()
    {
        IsInEdit= false;
        ProductData.Pharmacy = ProductPharmacy;
        ProductData.Medicine = ProductMedicine;
        ProductData.Price    = Price;
        ProductData.Quantity = Quantity; 
    }


    #endregion
    /**************************************/
}
