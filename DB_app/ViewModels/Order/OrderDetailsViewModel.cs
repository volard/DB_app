using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using DB_app.Contracts.ViewModels;
using DB_app.Core.Contracts.Services;
using DB_app.Entities;
using DB_app.Services.Messages;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace DB_app.ViewModels;

public partial class OrderDetailsViewModel : ObservableValidator, INavigationAware
{


    public async void OnNavigatedTo(object? parameter)
    {
        if (parameter is OrderWrapper model) // If we got smth to show
        {
            CurrentOrder = model;

            if (model.IsInEdit)
            {
                AvailableHospitals = new(await _repositoryControllerService.Hospitals.GetAsync());
                var temp = await _repositoryControllerService.Addresses.GetHospitalsLocationsAsync();
                CurrentOrder.AvailableAddresses = new(await _repositoryControllerService.Addresses.GetHospitalsLocationsAsync());
                AvailableProducts = new(await _repositoryControllerService.Products.GetAsync());
                CurrentOrder.Backup();
            }
            if (!model.IsNew) { PageTitle = "Order " + model.Id; }
        }
        else // If we wanna let user to create one
        {
            AvailableHospitals = new(await _repositoryControllerService.Hospitals.GetAsync());
            CurrentOrder.AvailableAddresses = new(await _repositoryControllerService.Addresses.GetHospitalsLocationsAsync());
            AvailableProducts = new(await _repositoryControllerService.Products.GetAsync());
        }
    }

    public void OnNavigatedFrom()
    { 
        // Not used
    }

    #region Members




    #endregion



    #region Properties

    private readonly IRepositoryControllerService _repositoryControllerService
         = App.GetService<IRepositoryControllerService>();


    public OrderWrapper CurrentOrder { get; set; } = new();


    
    [ObservableProperty]
    private ObservableCollection<Hospital> availableHospitals;

    [ObservableProperty]
    private ObservableCollection<Product> availableProducts;


    [ObservableProperty]
    private string pageTitle = "New order";


    #endregion


    #region Required for DataGrids

    public OrderItem OrderItemModel { get; set; }
    public string OrderItemMedicineName { get => OrderItemModel.Product.Medicine.Name; }
    public string OrderItemMedicineType { get => OrderItemModel.Product.Medicine.Type; }
    public string OrderItemQuantity { get => OrderItemModel.Product.Quantity.ToString(); }
    public string ItemTotal { get => (OrderItemModel.Quantity * OrderItemModel.Product.Price).ToString(); }

    #endregion

}