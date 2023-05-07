using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using DB_app.Contracts.ViewModels;
using DB_app.Core.Contracts.Services;
using DB_app.Helpers;
using DB_app.Repository;
using DB_app.Services.Messages;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace DB_app.ViewModels;

public partial class AddressesGridViewModel : ObservableRecipient, INavigationAware, IRecipient<DeleteRecordMessage<AddressWrapper>>
{
    private readonly IRepositoryControllerService _repositoryControllerService
        = App.GetService<IRepositoryControllerService>();

    /// <summary>
    /// DataGrid's data collection
    /// </summary>
    public ObservableCollection<AddressWrapper> Source { get; set; }
        = new ObservableCollection<AddressWrapper>();

    public AddressesGridViewModel()
    {
        WeakReferenceMessenger.Default.Register(this);
    }

    public void Receive(DeleteRecordMessage<AddressWrapper> message)
    {
        var _givenAddressWrapper = message.Value;
        Source.Remove(_givenAddressWrapper);
    }


    /// <summary>
    /// Represents selected by user AddressWrapper object
    /// </summary>
    [ObservableProperty]
    private AddressWrapper? selectedItem;


    public event EventHandler<ListEventArgs>? OperationRejected;


    public async Task DeleteSelected()
    {
        if (SelectedItem != null)
        {
            try
            {

                int id = SelectedItem.AddressData.Id;
                await _repositoryControllerService.Addresses.DeleteAsync(id);

                Source.Remove(SelectedItem);

                OperationRejected?.Invoke(this, new ListEventArgs(new List<String>() { "Everything is good" }));

            }
            catch (LinkedRecordOperationException)
            {
                OperationRejected?.Invoke(this, new ListEventArgs(new List<String>() { "Адресс связан с организацией. Удалите связанную организацию, чтобы удалить адрес" }));
            }
        }
    }


    public async void OnNavigatedTo(object parameter)
    {
        if (Source.Count < 1)
        {
            Source.Clear();
            var data = await _repositoryControllerService.Addresses.GetAsync();

            foreach (var item in data)
            {
                Source.Add(new AddressWrapper(item));
            }
        }
    }

    public void OnNavigatedFrom()
    {
    }
}
