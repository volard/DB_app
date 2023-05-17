using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.WinUI;
using DB_app.Core.Contracts.Services;
using DB_app.Helpers;
using DB_app.Models;
using DB_app.Services.Messages;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.UI.Dispatching;
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
        else
        {
            _orderData = order;
        }
        
        InitFields();
    }



    #endregion


    #region Properties

    private readonly IRepositoryControllerService _repositoryControllerService = App.GetService<IRepositoryControllerService>();
    private readonly DispatcherQueue _dispatcherQueue = DispatcherQueue.GetForCurrentThread();

    public DateTime DatePlaced => _orderData.DatePlaced;

    private Order _orderData = new Order();

    private void InitFields()
    {
        OrderHospital = _orderData.HospitalCustomer;
        SelectedAddress = _orderData.ShippingAddress;
        OrderItems = new ObservableCollection<OrderItem>(_orderData.Items);
    }


    
    /// <summary>
    /// Gets or sets a value that indicates whether to show a progress bar. 
    /// </summary>
    [ObservableProperty]
    private bool _isLoading;
    
    /// <summary>
    /// Loads the addresses data.
    /// </summary>
    public async Task LoadAvailableProductsAsync()
    {
        await _dispatcherQueue.EnqueueAsync(() =>
        {
            IsLoading = true;
        });

        IEnumerable<Product>? products = await _repositoryControllerService.Products.GetAsync();

        await _dispatcherQueue.EnqueueAsync(() =>
        {
            AvailableProducts.Clear();
            foreach (Product product in products)
            {
                AvailableProducts.Add(product);
            }

            IsLoading = false;
        });
    }

    
    [ObservableProperty]
    [NotifyDataErrorInfo]
    [NotifyPropertyChangedFor(nameof(IsModified))]
    [Required(ErrorMessage = "Hospital-customer is Required")]
    private Hospital? _orderHospital;


    [NotifyPropertyChangedFor(nameof(IsModified))]
    [ObservableProperty]
    private ObservableCollection<OrderItem> _orderItems = new ObservableCollection<OrderItem>();


    [NotifyPropertyChangedFor(nameof(IsModified))]
    [ObservableProperty]
    private ObservableCollection<Product> _availableProducts = new ObservableCollection<Product>();

    
    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required(ErrorMessage = "Shipping address is Required")]
    [NotifyPropertyChangedFor(nameof(IsModified))]
    private Address? _selectedAddress;


    /// <summary>
    /// Gets the order's total price.
    /// </summary>
    public double? Total => OrderItems != null ? OrderItems.Sum(item => item.Product.Price * item.Quantity) : 0;


    private Order? _backupData;


    /// <summary>
    /// Indicates whether object in edit mode
    /// </summary>
    [ObservableProperty]
    private bool _isInEdit;


    /// <summary>
    /// Indicates whether its a new object
    /// </summary>
    [ObservableProperty]
    private bool _isNew;


    #endregion


    #region Modification methods

    public void Backup()
    {
        _backupData = _orderData;
    }


    /// <summary>
    /// Go back to previous data after updating
    /// </summary>
    public async Task Revert()
    {
        if (_backupData != null)
        {
            _orderData = _backupData;
            await _repositoryControllerService.Orders.UpdateAsync(_orderData);
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
            await _repositoryControllerService.Orders.InsertAsync(_orderData);
        }
        else
        {
            await _repositoryControllerService.Orders.UpdateAsync(_orderData);
        }
        
        
        // Sync with grid
        WeakReferenceMessenger.Default.Send(new AddRecordMessage<OrderWrapper>(this));
        return true;
    }

    #endregion
    
    
    /// <summary>
    /// Indicates about changes that is not synced with UI DataGrid
    /// </summary>
    public bool IsModified
    {
        get
        {
            bool isDifferent = CollectionsHelper.IsDifferent(OrderItems.ToList(), _orderData.Items);

            return
                OrderHospital != _orderData.HospitalCustomer ||
                SelectedAddress != _orderData.ShippingAddress
                || isDifferent;
        }
    }


    #region Members


    public bool Equals(OrderWrapper? other) =>
         Id == other?.Id;


    public override string ToString()
        => $"OrderWrapper with OrderData - [ {_orderData} ]";


    public async Task RemoveOrderItem(OrderItem orderItem)
    {
        OrderItems.Remove(orderItem);
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
        int _index = OrderItems.IndexOf(orderItem);
        int old_value = OrderItems[_index].Quantity;
        OrderItems.RemoveAt(_index);
        orderItem.Quantity = new_quantity_value;
        OrderItems.Insert(_index, orderItem);

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

    public int Id => _orderData.Id;
    
    public void AddOrderItem(Product product, int quantity)
    {
        OrderItem? found = null;
        int _index;
        if (OrderItems.Count != 0)
        {
            found = OrderItems.FirstOrDefault(el => el.Product == product);
        }
        if (found != null)
        {
            found.Quantity += quantity;
            _index = OrderItems.IndexOf(found);
            OrderItems.RemoveAt(_index);
            OrderItems.Insert(_index, found);
        }
        else
        {
            OrderItems.Add(new OrderItem(_orderData, product, quantity));
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
        
        Backup();
    }

    public void CancelEdit()
    {
        
        IsInEdit = false;
    }

    public void EndEdit()
    {
        IsInEdit = false;
        _orderData.HospitalCustomer = OrderHospital;
        _orderData.ShippingAddress = SelectedAddress;
        _orderData.Items = OrderItems.ToList();
    }


    #endregion
    

}
