﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.WinUI;
using DB_app.Core.Contracts.Services;
using DB_app.Helpers;
using DB_app.Models;
using DB_app.Services.Messages;
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
    /**************************************/
    #region Constructors

    public OrderWrapper(Order? order = null)
    {
        if (order == null)
        {
            IsNew = true;
        }
        else
        {
            OrderData = order;
        }
        
        InitFields();
    }



    #endregion
    /**************************************/
    
    
    
    /**************************************/
    #region Properties

    
    private readonly IRepositoryControllerService _repositoryControllerService = App.GetService<IRepositoryControllerService>();
    
    
    private readonly DispatcherQueue _dispatcherQueue = DispatcherQueue.GetForCurrentThread();

    
    public DateTime DatePlaced => OrderData.DatePlaced;

    private Order _orderData = new Order();

    public Order OrderData
    {
        get => _orderData;
        set
        {
            _orderData = value;
            InitFields();
        }
    }

    
    public int Id => OrderData.Id;

    
    /// <summary>
    /// Indicates about changes that is not synced with UI DataGrid
    /// </summary>
    public bool IsModified
    {
        get
        {
            bool isDifferent = CollectionsHelper.IsDifferent(OrderItems.ToList(), OrderData.Items);

            return
                !Equals(OrderHospital, OrderData.HospitalCustomer) ||
                !Equals(SelectedAddress, OrderData.ShippingAddress)
                || isDifferent;
        }
    }
    
    
    /// <summary>
    /// Gets or sets a value that indicates whether to show a progress bar. 
    /// </summary>
    [ObservableProperty]
    private bool _isLoading;
    
    
    [ObservableProperty]
    [NotifyDataErrorInfo]
    [NotifyPropertyChangedFor(nameof(IsModified))]
    [Required(ErrorMessage = "Hospital-customer is Required")]
    private Hospital? _orderHospital;
    
    
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsModified))]
    [NotifyPropertyChangedFor(nameof(Total))]
    private ObservableCollection<OrderItem> _orderItems = new ObservableCollection<OrderItem>();
    

    [ObservableProperty]
    private bool _isProductLoading;
    

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required(ErrorMessage = "Shipping address is Required")]
    [NotifyPropertyChangedFor(nameof(IsModified))]
    private Address? _selectedAddress;


    /// <summary>
    /// Gets the order's total price.
    /// </summary>
    public double? Total => OrderItems.Sum(item => item.Product.Price * item.Quantity);


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
    /**************************************/
    


    /**************************************/
    #region Modification methods

    public void Backup()
    {
        _backupData = OrderData;
    }


    /// <summary>
    /// Go back to previous data after updating
    /// </summary>
    public async Task Revert()
    {
        if (_backupData != null)
        {
            OrderData = _backupData;
            await _repositoryControllerService.Orders.UpdateAsync(OrderData);
        }
    }

    
    public async Task<bool> SaveAsync()
    {
        ValidateAllProperties();
        if (HasErrors) return false;

        EndEdit();
        if (IsNew)
        {
            await _repositoryControllerService.Orders.InsertAsync(OrderData);
        }
        else
        {
            await _repositoryControllerService.Orders.UpdateAsync(OrderData);
        }
        
        // Sync with grid
        WeakReferenceMessenger.Default.Send(new AddRecordMessage<OrderWrapper>(this));
        return true;
    }

    #endregion
    /**************************************/
    
    

    /**************************************/
    #region Methods


    public bool Equals(OrderWrapper? other) =>
         Id == other?.Id;


    public override string ToString()
        => $"OrderWrapper with OrderData - [ {OrderData} ]";
    
    
    private void InitFields()
    {
        OrderHospital = OrderData.HospitalCustomer;
        SelectedAddress = OrderData.ShippingAddress;
        OrderItems = new ObservableCollection<OrderItem>(OrderData.Items);
    }


    #endregion
    /**************************************/







    public ObservableCollection<Hospital> AvailableHospitals = new ObservableCollection<Hospital>();

    [ObservableProperty]
    private bool _isShippingAddressesLoading;


    [ObservableProperty]
    private bool _isHospitalsLoading;


    public ObservableCollection<Address> AvailableShippingAddresses = new ObservableCollection<Address>();



    public async Task LoadAvailableShippingAddresses(int hospitalId)
    {
        await _dispatcherQueue.EnqueueAsync(() =>
        {
            IsShippingAddressesLoading = true;
        });

        IEnumerable<Address>? items = await Task.Run(async () =>
        {
            var origin = await _repositoryControllerService.Hospitals.GetHospitalLocations(hospitalId);
            return origin.Select(el => el.Address);
        });

        await _dispatcherQueue.EnqueueAsync(() =>
        {
            AvailableShippingAddresses.Clear();
            foreach (var item in items)
            {
                AvailableShippingAddresses.Add(item);
            }

            IsShippingAddressesLoading = false;
        });
    }





    public async Task LoadAvailableHospitals()
    {
        await _dispatcherQueue.EnqueueAsync(() =>
        {
            IsHospitalsLoading = true;
        });

        IEnumerable<Hospital>? items = await Task.Run(_repositoryControllerService.Hospitals.GetAsync);

        await _dispatcherQueue.EnqueueAsync(() =>
        {
            AvailableHospitals.Clear();
            foreach (var item in items)
            {
                AvailableHospitals.Add(item);
            }

            IsHospitalsLoading = false;
        });
    }





    /**************************************/
    #region IEditable implementation


    public void BeginEdit()
    {
        IsInEdit = true;
        Backup();
    }

    
    public void CancelEdit()
    {
        InitFields();
        IsInEdit = false;
    }

    
    public void EndEdit()
    {
        IsInEdit = false;
        OnPropertyChanged(nameof(IsModified));
        
        // NOTE the underlying code relays on preliminary data validation
        OrderData.HospitalCustomer = OrderHospital;
        OrderData.ShippingAddress = SelectedAddress;
        OrderData.Items = OrderItems.ToList();
    }


    #endregion
    /**************************************/

}
