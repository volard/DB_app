using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.WinUI;
using DB_app.Contracts.ViewModels;
using DB_app.Core.Contracts.Services;
using DB_app.Services.Messages;
using DB_app.Helpers;
using DB_app.Models;
using System.Collections.ObjectModel;
using DB_app.Repository;
using Microsoft.UI.Dispatching;

namespace DB_app.ViewModels;

public partial class ProductsGridViewModel : ObservableRecipient, INavigationAware, IRecipient<DeleteRecordMessage<ProductWrapper>>
{
    
    private readonly IRepositoryControllerService _repositoryControllerService = App.GetService<IRepositoryControllerService>();

    /// <summary>
    /// DataGrid's data collection
    /// </summary>
    public ObservableCollection<ProductWrapper> Source { get; } = new ObservableCollection<ProductWrapper>();
    
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

        IEnumerable<Product>? items = await _repositoryControllerService.Products.GetAsync();

        await _dispatcherQueue.EnqueueAsync(() =>
        {
            foreach (Product item in items)
            {
                Source.Add(new ProductWrapper(item));
            }

            IsLoading = false;
        });
    }
    
    public ProductsGridViewModel()
    {
        WeakReferenceMessenger.Default.Register<AddRecordMessage<ProductWrapper>>(this, (r, m) =>
        {
            if (r is not ProductsGridViewModel productsGridViewModel)
                return;
            
            productsGridViewModel.Source.Insert(0, m.Value);
            OnPropertyChanged(nameof(Source));
        });
    }

    public void Receive(DeleteRecordMessage<ProductWrapper> message)
    {
        ProductWrapper givenProductWrapper = message.Value;
        Source.Remove(givenProductWrapper);
    }


    /// <summary>
    /// Represents selected by user AddressWrapper object
    /// </summary>
    [ObservableProperty]
    private ProductWrapper? _selectedItem;


    public event EventHandler<NotificationConfigurationEventArgs>? DisplayNotification;


    public async Task DeleteSelected()
    {
        if (SelectedItem != null)
        {
            try
            {

                int id = SelectedItem.Id;
                await _repositoryControllerService.Products.DeleteAsync(id);

                Source.Remove(SelectedItem);

                DisplayNotification?.Invoke(this, new NotificationConfigurationEventArgs("Everything is good", NotificationHelper.SuccessStyle));

            }
            catch (LinkedRecordOperationException)
            {
                DisplayNotification?.Invoke(this, new NotificationConfigurationEventArgs("Адресс связан с организацией. Удалите связанную организацию, чтобы удалить адрес", NotificationHelper.ErrorStyle));
            }
        }
    }

    private bool _isOutOfStockEnabled = false;

    public async Task ToggleOutOfStock()
    {
        if (!_isOutOfStockEnabled)
        {
            IEnumerable<Product>? outOfStockProducts = await _repositoryControllerService.Products.GetOutOfStockAsync();
            foreach (Product item in outOfStockProducts)
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
        _isOutOfStockEnabled = !_isOutOfStockEnabled;
    }


    public void OnNavigatedTo(object parameter)
    {
        if (Source.Count >= 1) return;
            LoadItems();
    }

    public void OnNavigatedFrom()
    {
    }
}
