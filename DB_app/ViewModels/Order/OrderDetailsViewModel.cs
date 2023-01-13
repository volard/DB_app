using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using DB_app.Core.Contracts.Services;
using DB_app.Models;
using DB_app.Services.Messages;
using System.Collections.ObjectModel;

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
        WeakReferenceMessenger.Default.Register(this);
        AvailableAddresses = new(_repositoryControllerService.Addresses.GetAsync().Result);
        pageTitle = "New order";
    }



    public OrderDetailsViewModel(OrderWrapper OrderWrapper)
    {
        CurrentOrder = OrderWrapper;
        WeakReferenceMessenger.Default.Register(this);
        AvailableAddresses = new(getAvailableAddresses());
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
        return _repositoryControllerService.Addresses.GetAsync().Result;
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
            AvailableAddresses = new(getAvailableAddresses());
        }
    }

    public ObservableCollection<Address> AvailableAddresses { get; set; }

    [ObservableProperty]
    public Address selectedAddress;

    [ObservableProperty]
    public string pageTitle;

    #endregion



    #region Required for addresses DataGrid

    public Address AddressModel { get; set; }

    public string City { get => AddressModel.City; }
    public string Street { get => AddressModel.Street; }
    public string Building { get => AddressModel.Building; }


    #endregion


}