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

public partial class AddressesGridViewModel : ObservableRecipient, INavigationAware, IRecipient<DeleteRecordMessage<AddressWrapper>>
{
    private readonly IRepositoryControllerService _repositoryControllerService = App.GetService<IRepositoryControllerService>();

    /// <summary>
    /// DataGrid's data collection
    /// </summary>
    public ObservableCollection<AddressWrapper> Source { get; init; } = new ObservableCollection<AddressWrapper>();

    private readonly DispatcherQueue _dispatcherQueue = DispatcherQueue.GetForCurrentThread();

    public AddressesGridViewModel()
    {
        WeakReferenceMessenger.Default.Register<AddRecordMessage<AddressWrapper>>(this, (r, m) =>
        {
            if (r is not AddressesGridViewModel addressViewModel) return;
            
            addressViewModel.Source.Insert(0, m.Value);
            OnPropertyChanged(nameof(Source));
        });
    }

    public void Receive(DeleteRecordMessage<AddressWrapper> message)
    {
        AddressWrapper givenAddressWrapper = message.Value;
        Source.Remove(givenAddressWrapper);
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
    private AddressWrapper? _selectedItem;


    public event EventHandler<NotificationConfigurationEventArgs>? DisplayNotification;


    public async Task DeleteSelected()
    {
        if (SelectedItem == null) return;
        try
        {

            int id = SelectedItem.AddressData.Id;
            await _repositoryControllerService.Addresses.DeleteAsync(id);

            Source.Remove(SelectedItem);

            DisplayNotification?.Invoke(this, new NotificationConfigurationEventArgs("Everything is good", NotificationHelper.SuccessStyle));

        }
        catch (LinkedRecordOperationException)
        {
            DisplayNotification?.Invoke(this, new NotificationConfigurationEventArgs("Адресс связан с организацией. Удалите связанную организацию, чтобы удалить адрес", NotificationHelper.ErrorStyle));
        }
    }


    /// <summary>
    /// Retrieves items from the data source.
    /// </summary>
    public async void Load()
    {
        await _dispatcherQueue.EnqueueAsync(() =>
        {
            IsLoading = true;

        });

        IEnumerable<Address>? items = await Task.Run(_repositoryControllerService.Addresses.GetAsync);

        await _dispatcherQueue.EnqueueAsync(() =>
        {
            Source.Clear();
            foreach (Address item in items)
            {
                Source.Add(new AddressWrapper(item));
            }

            IsLoading = false;
        });
    }


    public void OnNavigatedTo(object parameter)
    {
        
    }

    public void OnNavigatedFrom() { }
}
