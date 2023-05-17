using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.WinUI;
using DB_app.Contracts.ViewModels;
using DB_app.Core.Contracts.Services;
using DB_app.Helpers;
using DB_app.Models;
using DB_app.Repository;
using DB_app.Services.Messages;
using Microsoft.UI.Dispatching;
using System.Collections.ObjectModel;


namespace DB_app.ViewModels;

public sealed partial class OrdersGridViewModel : ObservableRecipient, INavigationAware, IRecipient<DeleteRecordMessage<OrderWrapper>>
{
    private readonly IRepositoryControllerService _repositoryControllerService  = App.GetService<IRepositoryControllerService>();

    /// <summary>
    /// DataGrid's data collection
    /// </summary>
    public ObservableCollection<OrderWrapper> Source { get; } = new ObservableCollection<OrderWrapper>();

    public void Receive(DeleteRecordMessage<OrderWrapper> message)
    {
        OrderWrapper givenOrderWrapper = message.Value;
        Source.Remove(givenOrderWrapper);
    }


    /// <summary>
    /// Represents selected by user AddressWrapper object
    /// </summary>
    [ObservableProperty]
    private OrderWrapper? _selectedItem;



    public event EventHandler<NotificationConfigurationEventArgs>? DisplayNotification;


    public async Task DeleteSelected()
    {
        if (SelectedItem == null) return;
        
        try
        {
            int id = SelectedItem.Id;
            await _repositoryControllerService.Orders.DeleteAsync(id);

            Source.Remove(SelectedItem);

            DisplayNotification?.Invoke(this, new NotificationConfigurationEventArgs("Everything is good", NotificationHelper.SuccessStyle));

        }
        catch (LinkedRecordOperationException)
        {
            DisplayNotification?.Invoke(this, new NotificationConfigurationEventArgs("Адресс связан с организацией. Удалите связанную организацию, чтобы удалить адрес", NotificationHelper.ErrorStyle));
        }
    }
    
    
    /// <summary>
    /// Gets or sets a value that indicates whether to show a progress bar. 
    /// </summary>
    [ObservableProperty]
    private bool _isLoading;

    private readonly DispatcherQueue _dispatcherQueue = DispatcherQueue.GetForCurrentThread();
    
    
    /// <summary>
    /// Retrieves items from the data source.
    /// </summary>
    private async void LoadItems()
    {
        await _dispatcherQueue.EnqueueAsync(() =>
        {
            IsLoading = true;
            Source.Clear();
        });

        IEnumerable<Order>? orders = await _repositoryControllerService.Orders.GetAsync();

        await _dispatcherQueue.EnqueueAsync(() =>
        {
            foreach (Order order in orders)
            {
                Source.Add(new OrderWrapper(order));
            }

            IsLoading = false;
        });
    }


    public void OnNavigatedTo(object parameter)
    {
        if (Source.Count >= 1) return;
        
        LoadItems();
    }

    public void OnNavigatedFrom() { }
}
