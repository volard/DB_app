using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.WinUI;
using DB_app.Contracts.ViewModels;
using DB_app.Core.Contracts.Services;
using DB_app.Helpers;
using DB_app.Repository;
using DB_app.Services.Messages;
using Microsoft.UI.Dispatching;
using System.Collections.ObjectModel;

namespace DB_app.ViewModels;

public partial class AddressesGridViewModel : ObservableRecipient, INavigationAware, IRecipient<DeleteRecordMessage<AddressWrapper>>
{
    private readonly IRepositoryControllerService _repositoryControllerService = App.GetService<IRepositoryControllerService>();

    /// <summary>
    /// DataGrid's data collection
    /// </summary>
    public ObservableCollection<AddressWrapper> Source { get; set; } = new();

    private readonly DispatcherQueue _dispatcherQueue = DispatcherQueue.GetForCurrentThread();

    public AddressesGridViewModel()
    {
        WeakReferenceMessenger.Default.Register<AddRecordMessage<AddressWrapper>>(this, (r, m) =>
        {
            if (r is AddressesGridViewModel addressViewModel)
            {
                addressViewModel.Source.Insert(0, m.Value);
                OnPropertyChanged(nameof(Source));
            }
        });
    }

    public void Receive(DeleteRecordMessage<AddressWrapper> message)
    {
        var _givenAddressWrapper = message.Value;
        Source.Remove(_givenAddressWrapper);
    }

    /// <summary>
    /// Gets or sets a value that indicates whether to show a progress bar. 
    /// </summary>
    [ObservableProperty]
    private bool _isLoading;


    /// <summary>
    /// Represents selected by user AddressWrapper object
    /// </summary>
    [ObservableProperty]
    private AddressWrapper? selectedItem;


    public event EventHandler<NotificationConfigurationEventArgs>? OperationRejected;


    public async Task DeleteSelected()
    {
        if (SelectedItem != null)
        {
            try
            {

                int id = SelectedItem.AddressData.Id;
                await _repositoryControllerService.Addresses.DeleteAsync(id);

                Source.Remove(SelectedItem);

                OperationRejected?.Invoke(this, new NotificationConfigurationEventArgs("Everything is good", NotificationHelper.SuccessStyle));

            }
            catch (LinkedRecordOperationException)
            {
                OperationRejected?.Invoke(this, new NotificationConfigurationEventArgs("Адресс связан с организацией. Удалите связанную организацию, чтобы удалить адрес", NotificationHelper.ErrorStyle));
            }
        }
    }


    /// <summary>
    /// Retrieves items from the data source.
    /// </summary>
    public async void LoadItems()
    {
        await _dispatcherQueue.EnqueueAsync(() =>
        {
            IsLoading = true;
            Source.Clear();
        });

        var items = await Task.Run(_repositoryControllerService.Addresses.GetAsync);

        await _dispatcherQueue.EnqueueAsync(() =>
        {
            foreach (var item in items)
            {
                Source.Add(new AddressWrapper(item));
            }

            IsLoading = false;
        });
    }


    public async void OnNavigatedTo(object parameter)
    {
        if (Source.Count < 1)
        {
            LoadItems();
        }
    }

    public void OnNavigatedFrom()
    {
    }
}
