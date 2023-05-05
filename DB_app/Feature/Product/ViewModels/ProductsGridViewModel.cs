using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using DB_app.Contracts.ViewModels;
using DB_app.Core.Contracts.Services;
using DB_app.Services.Messages;
using DB_app.Helpers;
using System.Collections.ObjectModel;
using DB_app.Repository;

namespace DB_app.ViewModels;

public partial class ProductsGridViewModel : ObservableRecipient, INavigationAware, IRecipient<DeleteRecordMessage<ProductWrapper>>
{
private readonly IRepositoryControllerService _repositoryControllerService
        = App.GetService<IRepositoryControllerService>();

    /// <summary>
    /// DataGrid's data collection
    /// </summary>
    public ObservableCollection<ProductWrapper> Source { get; set; }
        = new ObservableCollection<ProductWrapper>();

    public ProductsGridViewModel()
    {
        WeakReferenceMessenger.Default.Register(this);
    }

    public void Receive(DeleteRecordMessage<ProductWrapper> message)
    {
        var givenProductWrapper = message.Value;
        Source.Remove(givenProductWrapper);
    }


    /// <summary>
    /// Represents selected by user AddressWrapper object
    /// </summary>
    [ObservableProperty]
    private ProductWrapper? _selectedItem;


    public event EventHandler<ListEventArgs>? OperationRejected;


    public async Task DeleteSelected()
    {
        if (SelectedItem != null)
        {
            try
            {

                int id = SelectedItem.Id;
                await _repositoryControllerService.Products.DeleteAsync(id);

                Source.Remove(SelectedItem);

                OperationRejected?.Invoke(this, new ListEventArgs(new List<String>() { "Everything is good" }));

            }
            catch (LinkedRecordOperationException)
            {
                OperationRejected?.Invoke(this, new ListEventArgs(new List<String>() { "Адресс связан с организацией. Удалите связанную организацию, чтобы удалить адрес" }));
            }
        }
    }

    private bool IsOutOfStockEnabled = false;

    public async Task ToggleOutOfStock()
    {
        if (!IsOutOfStockEnabled)
        {
            var outOfStockProducts = await _repositoryControllerService.Products.GetOutOfStockAsync();
            foreach (var item in outOfStockProducts)
            {
                Source.Insert(0, new ProductWrapper(item));
            }
        }
        else
        {
            int i = 0;
            while (i < Source.Count)
            {
                if (Source[i].ProductData.Quantity == 0) Source.Remove(Source[i]);
                ++i;
            }
        }
        IsOutOfStockEnabled = !IsOutOfStockEnabled;
    }


    public async void OnNavigatedTo(object parameter)
    {
        if (Source.Count < 1)
        {
            Source.Clear();
            var data = await _repositoryControllerService.Products.GetAsync();

            foreach (var item in data)
            {
                Source.Add(new ProductWrapper(item));
            }
        }
    }

    public void OnNavigatedFrom()
    {
    }
}
