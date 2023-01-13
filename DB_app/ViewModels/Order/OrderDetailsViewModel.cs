using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using DB_app.Core.Contracts.Services;
using DB_app.Models;
using DB_app.Services.Messages;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace DB_app.ViewModels;

public partial class OrderDetailsViewModel : ObservableRecipient, IRecipient<ShowOrderDetailsMessage>
{

    #region Constructors


    public OrderDetailsViewModel()
    {
        CurrentOrder = new()
        {
            isNew = true
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

    public void Receive(ShowOrderDetailsMessage message)
    {
        CurrentOrder = message.Value;
        CurrentOrder.NotifyAboutProperties();
    }

    /// <summary>
    /// Saves hospital that was edited or created
    /// </summary>
    public async Task SaveAsync()
    {
        if (CurrentOrder.isNew) // Create new medicine
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

    public void NotifyGridAboutChange() => WeakReferenceMessenger.Default.Send(new AddOrderMessage(CurrentOrder));

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


    //public ObservableCollection<Pharmacy> AvailablePharmacies { get; set; }
    //public ObservableCollection<Hospital> AvailableHospitals { get; set; }
    //public ObservableCollection<Address> AvailableAddresses { get; set; } = new();


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

    public Pharmacy selectedPharmacy
    {
        get => CurrentOrder.OrderData.PharmacySeller;
        set
        {
            if (CurrentOrder.OrderData.PharmacySeller != value)
            {
                CurrentOrder.IsModified = true;
                CurrentOrder.OrderData.PharmacySeller = value;
                OnPropertyChanged();
            }
        }
    }

    [ObservableProperty]
    private string pageTitle;

    [ObservableProperty]
    private bool isHospitalSelected = false;

    #endregion


    #region Required for OrderItems DataGrid

    public OrderItem OrderItemModel { get; set; }

    public string MedicineName { get => OrderItemModel.Product.Medicine.Name; }
    public string MedicineType { get => OrderItemModel.Product.Medicine.Type; }
    public string Quantity { get => OrderItemModel.Product.Quantity.ToString(); }
    public string ItemTotal { get => (OrderItemModel.Quantity * OrderItemModel.Product.Price).ToString(); }

    #endregion


    #region Required for addresses DataGrid

    public Address AddressModel { get; set; }

    public string City { get => AddressModel.City; }
    public string Street { get => AddressModel.Street; }
    public string Building { get => AddressModel.Building; }


    #endregion
}