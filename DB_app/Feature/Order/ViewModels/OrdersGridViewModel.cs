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
using Windows.System.Profile;


namespace DB_app.ViewModels;

public sealed partial class OrdersGridViewModel : ObservableRecipient, INavigationAware, IRecipient<DeleteRecordMessage<OrderWrapper>>
{
    private readonly IRepositoryControllerService _repositoryControllerService = App.GetService<IRepositoryControllerService>();

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

    public DispatcherQueue _dispatcherQueue = DispatcherQueue.GetForCurrentThread();
    
    

    public void OnNavigatedTo(object parameter)
    {
    }

    public async void Load()
    {
        await _dispatcherQueue.EnqueueAsync(() =>
        {
            IsLoading = true;
        });

        IEnumerable<Order>? items = await Task.Run(_repositoryControllerService.Orders.GetAsync);

        await _dispatcherQueue.EnqueueAsync(() =>
        {
            Source.Clear();
            foreach (Order item in items)
            {
                Source.Add(new OrderWrapper(item));
            }

            IsLoading = false;
        });
    }

    public void OnNavigatedFrom() { }
}
