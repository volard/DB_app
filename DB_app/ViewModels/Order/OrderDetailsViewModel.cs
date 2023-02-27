using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using DB_app.Core.Contracts.Services;
using DB_app.Entities;
using DB_app.Services.Messages;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace DB_app.ViewModels;

public partial class OrderDetailsViewModel : ObservableRecipient, IRecipient<ShowRecordDetailsMessage<OrderWrapper>>
{

    #region Constructors


    public OrderDetailsViewModel()
    {
        CurrentOrder = new()
        {
            IsNew = true
        };
        pageTitle = "New order";

        Initialize();
    }

    public void Initialize()
    {
        WeakReferenceMessenger.Default.Register(this);
        availablePharmacies = new(_repositoryControllerService.Pharmacies.GetAsync().Result);
        AvailableHospitals = new(_repositoryControllerService.Hospitals.GetAsync().Result);
        AvailableAddresses = new(getAvailableAddresses());
    }

    public OrderDetailsViewModel(OrderWrapper OrderWrapper)
    {
        CurrentOrder = OrderWrapper;

        Initialize();
    }


    #endregion



    #region Members

    public void Receive(ShowRecordDetailsMessage<OrderWrapper> message)
    {
        CurrentOrder = message.Value;
    }

    /// <summary>
    /// Saves hospital that was edited or created
    /// </summary>
    public async Task SaveAsync()
    {
        if (CurrentOrder.IsNew) // Create new medicine
        {
            await _repositoryControllerService.Orders.InsertAsync(CurrentOrder.OrderData);
        }
        else // Update existing medicine
        {
            await _repositoryControllerService.Orders.UpdateAsync(CurrentOrder.OrderData);
        }
    }


    [ObservableProperty]
    public Address selectedExistingAddress;


    public IEnumerable<Address> getAvailableAddresses()
    {
        if (CurrentOrder.HospitalCustomer != null)
            return CurrentOrder.HospitalCustomer.Addresses;
        else 
            return Enumerable.Empty<Address>();
    }

    public void NotifyGridAboutChange() => WeakReferenceMessenger.Default.Send(new AddRecordMessage<OrderWrapper>(CurrentOrder));

    #endregion



    #region Properties

    private readonly IRepositoryControllerService _repositoryControllerService
         = App.GetService<IRepositoryControllerService>();


    private OrderWrapper currentOrder;

    /// <summary>
    /// Current OrderWrapper to edit
    /// </summary>
    public OrderWrapper CurrentOrder
    {
        get
        {
            return currentOrder;
        }
        set
        {
            currentOrder = value;
            pageTitle = "Order #" + currentOrder.Id;
        }
    }


    [ObservableProperty]
    private ObservableCollection<Pharmacy> availablePharmacies;

    [ObservableProperty]
    private ObservableCollection<Hospital> availableHospitals;

    [ObservableProperty]
    private ObservableCollection<Address> availableAddresses;

    [ObservableProperty]
    private ObservableCollection<ProductWrapper> availableProducts = new();

    public async Task GetAvailableProducts()
    {
        var _data = await _repositoryControllerService.Products.GetFromPharmacy(SelectedPharmacy.Id);
        var _anotherdata = new List<ProductWrapper>();
        foreach (var item in _data)
        {
            _anotherdata.Add(new ProductWrapper(item));
        }

        AvailableProducts = new(_anotherdata);
    }


    [ObservableProperty]
    private Address selectedAddress;

    public Hospital SelectedHospital
    {
        get => CurrentOrder.OrderData.HospitalCustomer;
        set
        {
            if (CurrentOrder.OrderData.HospitalCustomer != value)
            {
                CurrentOrder.IsModified = true;
                CurrentOrder.OrderData.HospitalCustomer = value;
                OnPropertyChanged();
                IsHospitalSelected = true;
                AvailableAddresses = new(getAvailableAddresses());
            }
        }
    }

    public Pharmacy SelectedPharmacy
    {
        get => CurrentOrder.PharmacySeller;
        set
        {
            CurrentOrder.IsModified = true;
            CurrentOrder.PharmacySeller = value;
            OnPropertyChanged();
            _ = GetAvailableProducts();
        }
    }

    [ObservableProperty]
    private string pageTitle;

    [ObservableProperty]
    private bool isHospitalSelected = false;

    [ObservableProperty]
    private bool _isNew = false;

    #endregion


    #region Required for OrderItems DataGrid

    public OrderItem OrderItemModel { get; set; }

    public string OrderItemMedicineName { get => OrderItemModel.Product.Medicine.Name; }
    public string OrderItemMedicineType { get => OrderItemModel.Product.Medicine.Type; }
    public string OrderItemQuantity { get => OrderItemModel.Product.Quantity.ToString(); }
    public string ItemTotal { get => (OrderItemModel.Quantity * OrderItemModel.Product.Price).ToString(); }

    #endregion

    public ProductWrapper ProductModel { get; set; }

    public string ProductMedicineName { get => ProductModel.Medicine.Name; }

    public string ProductMedicineType { get => ProductModel.Medicine.Type; }

    public double ProductPrice { get => ProductModel.Price; }
    public int ProductQuantity { get => ProductModel.Quantity; }
}