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


    public Product ProductData { get;
        private set; } = new Product();


    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required(ErrorMessage = "Medicine is Required")]
    private Medicine? _medicine;


    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required(ErrorMessage = "Pharmacy is Required")]
    private Pharmacy? _pharmacy;


    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required(ErrorMessage = "Quantity is Required")]
    [Range(0, int.MaxValue, ErrorMessage = "Please enter a value bigger than {1}")]
    //[RegularExpression("([1-9]+)", ErrorMessage = "Please enter a Number")]
    private int _quantity;


    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required(ErrorMessage = "Price is Required")]
    [Range(0, int.MaxValue, ErrorMessage = "Please enter a value bigger than {1}")]
    //[RegularExpression("([1-9]+)", ErrorMessage = "Please enter a Number")]
    private double _price;


    public int Id { get => ProductData.Id; }



    /// <summary>
    /// Indicates about changes that is not synced with UI DataGrid
    /// </summary>
    public bool IsModified
    {
        get => !Equals(Medicine, ProductData.Medicine) ||
               !Equals(Pharmacy, ProductData.Pharmacy);
    }

    [ObservableProperty]
    private bool _isInEdit;

    /// <summary>
    /// Indicates whether its a new object
    /// </summary>
    [ObservableProperty]
    private bool _isNew;

    #endregion
    /**************************************/



    /**************************************/
    #region Methods


    public bool Equals(ProductWrapper? other)
    {
        return Equals(Medicine, other?.Medicine) &&
               Equals(Medicine, other?.Medicine) &&
               Equals(Price, other?.Price) &&
               Quantity == other.Quantity;
    }

    public override string ToString()
        => $"ProductWrapper with ProductData - [ {ProductData} ]";


    private void InitFields()
    {
        Medicine = ProductData.Medicine;
        Pharmacy = ProductData.Pharmacy;
        Quantity = ProductData.Quantity;
        Price = ProductData.Price;
    }


    #endregion
    /**************************************/



    /**************************************/
    #region Modification methods


    /// <summary>
    /// Go back to previous data after updating
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
            await _repositoryControllerService.Products.InsertAsync(ProductData);
        }
        else
        {
            await _repositoryControllerService.Products.UpdateAsync(ProductData);
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
        IsInEdit= false;
        
        // NOTE the underlying code relays on preliminary data validation
        ProductData.Pharmacy = Pharmacy!;
        ProductData.Medicine = Medicine!;
        ProductData.Price    = Price;
        ProductData.Quantity = Quantity; 
    }


    #endregion
    /**************************************/
}
