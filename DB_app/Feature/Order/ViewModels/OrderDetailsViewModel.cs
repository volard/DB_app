using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.WinUI;
using CommunityToolkit.WinUI.UI.Controls;
using DB_app.Contracts.ViewModels;
using DB_app.Core.Contracts.Services;
using DB_app.Helpers;
using DB_app.Models;
using DocumentFormat.OpenXml.Bibliography;
using Microsoft.UI.Dispatching;
using System.Collections.ObjectModel;

namespace DB_app.ViewModels;

public partial class OrderDetailsViewModel : ObservableValidator, INavigationAware
{
    
    /**************************************/
    #region Navigation implementation

    
    public void OnNavigatedTo(object? parameter)
        {
            //if (parameter is not OrderWrapper model) return;
            //CurrentOrder = model;
            //CurrentOrder.Backup();
        }
    
    public void OnNavigatedFrom() { /* Not used */ }

        
    #endregion
    /**************************************/



    public async void LoadAvailableHospitals()
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
    #region Properties

    private readonly DispatcherQueue _dispatcherQueue = DispatcherQueue.GetForCurrentThread();
    
    
    private readonly IRepositoryControllerService _repositoryControllerService = App.GetService<IRepositoryControllerService>();
    
    
    public ObservableCollection<Hospital> AvailableHospitals = new ObservableCollection<Hospital>();

    [ObservableProperty]
    private bool _isShippingAddressesLoading;

    
    [ObservableProperty]
    private bool _isHospitalsLoading;

    
    public ObservableCollection<Address> AvailableShippingAddresses = new ObservableCollection<Address>();



    public async void LoadAvailableShippingAddresses(int hospitalId)
    {
        await _dispatcherQueue.EnqueueAsync(() =>
        {
            IsShippingAddressesLoading = true;
        });

        IEnumerable<Address>? items = await Task.Run(async () =>
        {
            var origin =  await _repositoryControllerService.Hospitals.GetHospitalLocations(hospitalId);
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



    [ObservableProperty]
    private bool _isProductsLoading;
    

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(PageTitle))]
    private OrderWrapper _currentOrder = new OrderWrapper { IsNew = true, IsInEdit = true };

    
    public ObservableCollection<Product> AvailableProducts = new ObservableCollection<Product>();
    
    
    public string PageTitle
    {
        get
        {
            if (CurrentOrder.IsNew) return "New_Order".GetLocalizedValue();
            else return "Order/Text".GetLocalizedValue() + " #" + CurrentOrder.Id;
        }
    }

    #endregion
    /**************************************/
    
    
    
    /**************************************/
    #region Methods


    public async Task RemoveOrderItem(OrderItem orderItem)
    {
        CurrentOrder.OrderItems.Remove(orderItem);
        Product? temp = AvailableProducts.FirstOrDefault(el => Equals(el, orderItem.Product));
        int index;
        if (temp == null)
        {
            temp = await _repositoryControllerService.Products.GetAsync(orderItem.Product.Id);
            index = 0;
        }
        else
        {
            index = AvailableProducts.IndexOf(temp);
            AvailableProducts.RemoveAt(index);
        }
        temp.Quantity += orderItem.Quantity;
        AvailableProducts.Insert(index, temp);
    }
    

    public async Task UpdateOrderItem(OrderItem orderItem, int difference)
    {
        int index = CurrentOrder.OrderItems.IndexOf(orderItem);
        CurrentOrder.OrderItems.RemoveAt(index);
        orderItem.Quantity += difference;
        CurrentOrder.OrderItems.Insert(index, orderItem);

        Product? temp = AvailableProducts.FirstOrDefault(el => Equals(el, orderItem.Product));
        if (temp == null)
        {
            temp = await _repositoryControllerService.Products.GetAsync(orderItem.Product.Id);
            index = 0;
        }
        else
        {
            index = AvailableProducts.IndexOf(temp);
            AvailableProducts.RemoveAt(index);
        }
        int resultValue = temp.Quantity + difference;
        temp.Quantity = resultValue;
        AvailableProducts.Insert(index, temp);
    }


    public void InsertOrderItem(Product product, int quantity)
    {
        OrderItem? found = null;
        int index;
        if (CurrentOrder.OrderItems.Any())
        {
            found = CurrentOrder.OrderItems.FirstOrDefault(el => el.Product == product);
        }
        
        if (found != null)
        {
            found.Quantity += quantity;
            
            index = CurrentOrder.OrderItems.IndexOf(found);    //
            CurrentOrder.OrderItems.RemoveAt(index);           //
            CurrentOrder.OrderItems.Insert(index, found);      //
        }
        else
        {
            CurrentOrder.OrderItems.Add(new OrderItem(CurrentOrder.OrderData, product, quantity));
        }
        
        // Updates products page
        Product temp = AvailableProducts.First(el => Equals(el, product)) ?? throw new ArgumentNullException(nameof(product));
        temp.Quantity -= quantity;
        
        index = AvailableProducts.IndexOf(temp);
        AvailableProducts.RemoveAt(index);
        if (temp.Quantity != 0)
        {
            AvailableProducts.Insert(index, temp);
        }
    }


    public async Task LoadProducts()
    {
        await _dispatcherQueue.EnqueueAsync(() =>
        {
            IsProductsLoading = true;
        });

        IEnumerable<Product>? items = await Task.Run(_repositoryControllerService.Products.GetAsync);

        await _dispatcherQueue.EnqueueAsync(() =>
        {
            AvailableProducts.Clear();
            foreach (var item in items)
            {
                AvailableProducts.Add(item);
            }

            IsProductsLoading  = false;
        });
    }

    #endregion
}