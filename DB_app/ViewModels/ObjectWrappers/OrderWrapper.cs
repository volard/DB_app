using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.WinUI.UI;
using DB_app.Core.Contracts.Services;
using DB_app.Models;
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
        if (order == null)
        {
            IsNew = true;
        }
        else { OrderData = order; }
    }

    #endregion


    #region Properties

    private readonly IRepositoryControllerService _repositoryControllerService
        = App.GetService<IRepositoryControllerService>();


    private Order _orderData = new();


    public Order OrderData
    {
        get { return _orderData; }
        set
        {
            _orderData = value;
            OrderHospital = _orderData.HospitalCustomer;
            ShippingAddress = _orderData.ShippingAddress;
            ObservableItems = new(_orderData.Items);
        }
    }


    [Required(ErrorMessage = "Hospital-customer is Required")]
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(AvailableAddresses))]
    private Hospital _orderHospital;


    [Required(ErrorMessage = "Shipping address is Required")]
    [ObservableProperty]
    private Address _shippingAddress;




    // Required for orders datagrid
    public int Id { get => OrderData.Id; }
    public string Surename_main_doctor { get => OrderData.HospitalCustomer.Surename_main_doctor; }
    public DateTime DatePlaced { get => OrderData.DatePlaced; }

    //[ObservableProperty]
    public ObservableCollection<OrderItem> ObservableItems = new();


    public void AddProduct(Product product, int quantity)
    {
        OrderItem? found;
        if (ObservableItems.Count != 0)
        {
            found = ObservableItems.FirstOrDefault(el => el.Product == product);
            if (found != null)
            {
                found.Quantity += quantity;
                var _index = ObservableItems.IndexOf(found);
                ObservableItems.RemoveAt(_index);
                ObservableItems.Insert(_index, found);
            }
            else
            {
                ObservableItems.Add(new OrderItem(OrderData, product, quantity));
            }
        }
        else
        {
            ObservableItems.Add(new OrderItem(OrderData, product, quantity));
        }
        OnPropertyChanged(nameof(Total));


        var temp = AvailableProducts.FirstOrDefault(el => el == product);
        temp.Quantity -= quantity;
        //var index = AvailableProducts.IndexOf(temp);
        //AvailableProducts.RemoveAt(index);
        //AvailableProducts.Insert(index, temp);
    }

    [ObservableProperty]
    private ObservableCollection<Product> availableProducts;


    public bool RemoveProduct(Product product, int quantity)
    {
        try
        {
            ObservableItems.Remove(ObservableItems.First(el => el.Product == product));
            OnPropertyChanged(nameof(Total));
            AvailableProducts.First(el => el == product).Quantity += quantity;
            OnPropertyChanged(nameof(AvailableProducts));
            return true;
        }
        catch (ArgumentNullException)
        {
            return false;
        }
    }

    public bool UpdateProduct(Product product, int quantity)
    {
        try
        {
            ObservableItems.First(el => el.Product == product).Quantity = quantity;
            OnPropertyChanged(nameof(Total));
            return true;
        }
        catch (ArgumentNullException) { return false; }
    }


    /// <summary>
    /// Gets the order's total price.
    /// </summary>
    public double? Total => ObservableItems != null ? ObservableItems.Sum(item => item.Product.Price * item.Quantity) : 0;


    private Order? _backupData;

    [ObservableProperty]
    private bool isModified = false;

    /// <summary>
    /// Indicates whether object in edit mode
    /// </summary>
    [ObservableProperty]
    private bool isInEdit = false;

    /// <summary>
    /// Indicates whether its a new object
    /// </summary>
    [ObservableProperty]
    private bool isNew = false;

    #endregion


    #region Modification methods



    public void Backup()
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
        IsModified = true;
        Backup();
    }

    public void CancelEdit()
    {
        IsModified = false;
        IsInEdit = false;
    }

    public void EndEdit()
    {
        IsInEdit = false;
        OrderData.HospitalCustomer = OrderHospital;
        OrderData.ShippingAddress = ShippingAddress;
        OrderData.Items = ObservableItems.ToList();
    }


    #endregion



    private ObservableCollection<Address> _availableAddresses;

    public ObservableCollection<Address> AvailableAddresses
    {
        get
        {
            if (OrderHospital != null)
            {
                return new(OrderHospital.Addresses);
            }
            return _availableAddresses;
        }
        set
        {
            _availableAddresses = value;
        }
    }


}
