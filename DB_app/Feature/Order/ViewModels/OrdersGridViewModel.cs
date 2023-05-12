using AppUIBasics.Helper;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using DB_app.Contracts.ViewModels;
using DB_app.Core.Contracts.Services;
using DB_app.Helpers;
using DB_app.Repository;
using DB_app.Services.Messages;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace DB_app.ViewModels;

public sealed partial class OrdersGridViewModel : ObservableRecipient, INavigationAware, IRecipient<DeleteRecordMessage<OrderWrapper>>
{
private readonly IRepositoryControllerService _repositoryControllerService
        = App.GetService<IRepositoryControllerService>();

    /// <summary>
    /// DataGrid's data collection
    /// </summary>
    public ObservableCollection<OrderWrapper> Source { get; set; }
        = new ObservableCollection<OrderWrapper>();

    public OrdersGridViewModel()
    {
        WeakReferenceMessenger.Default.Register(this);
    }

    public void Receive(DeleteRecordMessage<OrderWrapper> message)
    {
        var givenOrderWrapper = message.Value;
        Source.Remove(givenOrderWrapper);
    }


    /// <summary>
    /// Represents selected by user AddressWrapper object
    /// </summary>
    [ObservableProperty]
    private OrderWrapper selectedItem;



    public event EventHandler<NotificationConfigurationEventArgs>? OperationRejected;


    public async Task DeleteSelected()
    {
        if (SelectedItem != null)
        {
            try
            {

                int id = SelectedItem.Id;
                await _repositoryControllerService.Orders.DeleteAsync(id);

                Source.Remove(SelectedItem);

                OperationRejected?.Invoke(this, new NotificationConfigurationEventArgs("Everything is good", NotificationType.Success));

            }
            catch (LinkedRecordOperationException)
            {
                OperationRejected?.Invoke(this, new NotificationConfigurationEventArgs("Адресс связан с организацией. Удалите связанную организацию, чтобы удалить адрес", NotificationType.Error));
            }
        }
    }


    public async void OnNavigatedTo(object parameter)
    {
        if (Source.Count < 1)
        {
            Source.Clear();
            var data = await _repositoryControllerService.Orders.GetAsync();

            foreach (var item in data)
            {
                Source.Add(new OrderWrapper(item));
            }
        }
    }

    public void OnNavigatedFrom()
    {
    }
}
