using CommunityToolkit.Mvvm.ComponentModel;
using DB_app.Core.Contracts.Services;
using DB_app.Entities;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace DB_app.ViewModels;

/// <summary>
/// Provides wrapper for the <see cref="Order"/> model class, encapsulating various services for access by the UI.
/// </summary>
public sealed partial class OrderWrapper : ObservableValidator, IEditableObject
{

    #region Constructors

    public OrderWrapper(Order? order = null)
    {
        if (order != null)
        OrderData = order;
    }

    #endregion


    #region Properties

    private readonly IRepositoryControllerService _repositoryControllerService
        = App.GetService<IRepositoryControllerService>();


    public Order OrderData { get; set; }


    [Required(ErrorMessage = "Hospital-customer is Required")]
    public Hospital HospitalCustomer
    {
        get => OrderData.HospitalCustomer;
        set
        {
            ValidateProperty(value);
            if (!GetErrors(nameof(HospitalCustomer)).Any())
            {
                OrderData.HospitalCustomer = value;
                OnPropertyChanged();
                IsModified = true;
            }
        }
    }

    /// <summary>
    /// City of the current OrderWrapper's data object
    /// </summary>
    public string Surename_main_doctor { get => OrderData.HospitalCustomer.Surename_main_doctor; }

    /// <summary>
    /// Street of the current OrderWrapper's data object
    /// </summary>
    public string PharmacyName { get => "placeholder"; }

    public DateTime DatePlaced { get => OrderData.DatePlaced; }



    [Required(ErrorMessage = "Shipping address is Required")]
    public Address ShippingAddress
    {
        get => OrderData.ShippingAddress;
        set
        {
            ValidateProperty(value);
            if (!GetErrors(nameof(ShippingAddress)).Any())
            {
                OrderData.ShippingAddress = value;
                OnPropertyChanged();
                IsModified = true;
            }
        }
    }

    private Pharmacy pharmacySeller;

    [Required(ErrorMessage = "Pharmacy-seller is Required")]
    public Pharmacy PharmacySeller
    {
        get => pharmacySeller;
        set
        {
            ValidateProperty(value);
            if (!GetErrors(nameof(PharmacySeller)).Any())
            {
                pharmacySeller = value;
                OnPropertyChanged();
                IsModified = true;
            }

        }
    }


    public int Id { get => OrderData.Id; }

    public ObservableCollection<OrderItem> ObservableItems
    {
        get => new(OrderData.Items);
        set
        {
            OrderData.Items = value.ToList();
            IsModified = true;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// Gets the order's total price.
    /// </summary>
    public double Total => ObservableItems.Sum(item => item.Product.Price * item.Quantity);



    private Order? _backupData;

    /// <summary>
    /// Indicates about changes that is not synced with UI DataGrid
    /// </summary>
    [ObservableProperty]
    private bool isModified = false;

    /// <summary>
    /// Indicates whether its a new object
    /// </summary>
    [ObservableProperty]
    public bool _isNew = false;

    #endregion


    #region Modification methods

    

    public void Buckup()
    {
        _backupData = OrderData;
    }


    /// <summary>
    /// Go back to prevoius data after updating
    /// </summary>
    public async Task Revert()
    {
        if (_backupData != null)
        {
            OrderData = _backupData;
            await App.GetService<IRepositoryControllerService>().Orders.UpdateAsync(OrderData);
        }
    }

    public async Task<bool> SaveAsync()
    {
        ValidateAllProperties();
        if (HasErrors) return false;
        EndEdit();
        if (IsNew)
        {
            await App.GetService<IRepositoryControllerService>().Orders.InsertAsync(OrderData);
        }
        else
        {
            await App.GetService<IRepositoryControllerService>().Orders.UpdateAsync(OrderData);
        }
        return true;
    }


        
    #endregion


    #region Members

   public bool Equals(OrderWrapper? other) =>
        Id == other?.Id;

    public override string ToString()
        => $"OrderWrapper with OrderData - [ {OrderData} ]";


    #endregion


    #region IEditable implementation


    public void BeginEdit()
    {
        isModified = true;
        Buckup();
    }

    public void CancelEdit()
    {
        
        isModified = false;
    }

    public async void EndEdit()
    {
        await _repositoryControllerService.Orders.UpdateAsync(OrderData);
    }

 
    #endregion

}
