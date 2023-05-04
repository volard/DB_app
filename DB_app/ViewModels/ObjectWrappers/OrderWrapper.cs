using CommunityToolkit.Mvvm.ComponentModel;
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

    private readonly IRepositoryControllerService _repositoryControllerService = App.GetService<IRepositoryControllerService>();

    private Order _orderData = new();

    public Order OrderData
    {
        get { return _orderData; }
        set
        {
            _orderData = value;
            OrderHospital = _orderData.HospitalCustomer;
            ShippingAddress = _orderData.ShippingAddress;
            ObservableOrderItems = new(_orderData.Items);
        }
    }

    [Required(ErrorMessage = "Hospital-customer is Required"), ObservableProperty, NotifyPropertyChangedFor(nameof(AvailableAddresses))]
    private Hospital _orderHospital;

    [ObservableProperty, Required(ErrorMessage = "Shipping address is Required")]
    private Address _shippingAddress;

    // Required for orders datagrid
    public int Id { get => OrderData.Id; }
    public string Surename_main_doctor { get => OrderData.HospitalCustomer.Surename_main_doctor; }
    public DateTime DatePlaced { get => OrderData.DatePlaced; }

    [ObservableProperty]
    private ObservableCollection<OrderItem> observableOrderItems = new();

    [ObservableProperty]
    private ObservableCollection<Product> availableProducts;

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

    /// <summary>
    /// Gets the order's total price.
    /// </summary>
    public double? Total => ObservableOrderItems != null ? ObservableOrderItems.Sum(item => item.Product.Price * item.Quantity) : 0;

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
        if (HasErrors)
        {
            return false;
        }
            
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


    public async Task RemoveOrderItem(OrderItem orderItem)
    {
        ObservableOrderItems.Remove(orderItem);
        OnPropertyChanged(nameof(Total));

        var _temp = AvailableProducts.FirstOrDefault(el => el == orderItem.Product);
        int _index;
        if (_temp == null)
        {
            _temp = await _repositoryControllerService.Products.GetAsync(orderItem.Product.Id);
            _index = 0;
        }
        else
        {
            _index = AvailableProducts.IndexOf(_temp);
            AvailableProducts.RemoveAt(_index);
        }
        _temp.Quantity += orderItem.Quantity;
        AvailableProducts.Insert(_index, _temp);
    }


    /// <summary>
    /// Replace orderItem with its new version
    /// </summary>
    /// <param name="orderItem"></param>
    public async Task UpdateOrderItem(OrderItem orderItem, int new_quantity_value)
    {
        int _index = ObservableOrderItems.IndexOf(orderItem);
        int old_value = ObservableOrderItems[_index].Quantity;
        ObservableOrderItems.RemoveAt(_index);
        orderItem.Quantity = new_quantity_value;
        ObservableOrderItems.Insert(_index, orderItem);

        OnPropertyChanged(nameof(Total));

        var _temp = AvailableProducts.FirstOrDefault(el => el == orderItem.Product);
        if( _temp == null )
        {
            _temp = await _repositoryControllerService.Products.GetAsync(orderItem.Product.Id);
            _index = 0;
        }
        else
        {
            _index = AvailableProducts.IndexOf(_temp);
            AvailableProducts.RemoveAt(_index);
        }
        
        int result_value = _temp.Quantity - (new_quantity_value - old_value);
        _temp.Quantity = result_value;

        AvailableProducts.Insert(_index, _temp);
    }

    
    public void AddOrderItem(Product product, int quantity)
    {
        OrderItem? found = null;
        int _index;
        if (ObservableOrderItems.Count != 0)
        {
            found = ObservableOrderItems.FirstOrDefault(el => el.Product == product);
        }
        if (found != null)
        {
            found.Quantity += quantity;
            _index = ObservableOrderItems.IndexOf(found);
            ObservableOrderItems.RemoveAt(_index);
            ObservableOrderItems.Insert(_index, found);
        }
        else
        {
            ObservableOrderItems.Add(new OrderItem(OrderData, product, quantity));
        }

        OnPropertyChanged(nameof(Total));


        var _temp = AvailableProducts.First(el => el == product);
        _temp.Quantity -= quantity;
        _index = AvailableProducts.IndexOf(_temp);
        AvailableProducts.RemoveAt(_index);
        if(_temp.Quantity != 0)
        {
            AvailableProducts.Insert(_index, _temp);
        }
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
        IsInEdit = false;
    }

    public void EndEdit()
    {
        IsInEdit = false;
        OrderData.HospitalCustomer = OrderHospital;
        OrderData.ShippingAddress = ShippingAddress;
        OrderData.Items = ObservableOrderItems.ToList();
    }


    #endregion



}
