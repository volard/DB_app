using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.WinUI;
using DB_app.Contracts.ViewModels;
using DB_app.Core.Contracts.Services;
using DB_app.Helpers;
using DB_app.Models;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml.Data;
using System.Collections.ObjectModel;

namespace DB_app.ViewModels;

public partial class MedicineInHospitalReportViewModel : ObservableRecipient, INavigationAware
{

    public MedicineInHospitalReportViewModel()
    {
    }

    private readonly IRepositoryControllerService _repositoryControllerService = App.GetService<IRepositoryControllerService>();

    private readonly DispatcherQueue _dispatcherQueue = DispatcherQueue.GetForCurrentThread();



    /// <summary>
    /// DataGrid's data collection
    /// </summary>
    public ObservableCollection<OrderItem> Source { get; init; } = new ObservableCollection<OrderItem>();
    public ObservableCollection<Hospital> AvailableHospitals { get; } = new ObservableCollection<Hospital>();



    [ObservableProperty]
    private Hospital _selectedHospital;

    /// <summary>
    /// Gets or sets a value that indicates whether to show a progress bar. 
    /// </summary>
    [ObservableProperty]
    private bool _isHospitalsLoading = false;

    [ObservableProperty]
    private bool _isSourceLoading = false;

    public async void OnNavigatedTo(object parameter)
    {
        Source.CollectionChanged += Source_CollectionChanged;
        CollectionsHelper.LoadCollectionAsync(AvailableHospitals, _dispatcherQueue, _repositoryControllerService.Hospitals.GetAsync);
        SelectedHospital = AvailableHospitals[0];
    }


    // NOTE subject to change
    private void Source_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        OnPropertyChanged(nameof(Source));
    }

    // Create grouping for collection
    public ObservableCollection<GroupInfoCollection<Medicine>> GroupedOrders = new();

    public CollectionViewSource GroupedItemsViewSource = new();

    public struct StoredMedicine
    {
        public StoredMedicine(Medicine medicine, int quantity)
        {
            Medicine = medicine;
            Quantity = quantity;
        }

        public Medicine Medicine { get; set; }
        public int Quantity { get; set; }
    }

    public Dictionary<string, int> QuantityPerType = new();

    /// <summary>
    /// Retrieves items from the data source.
    /// </summary>
    public async Task LoadSource(Hospital hospital)
    {
        await _dispatcherQueue.EnqueueAsync(() =>
        {
            IsSourceLoading = true;
            Source.Clear();
            GroupedOrders.Clear();
            QuantityPerType.Clear();
        });

        IEnumerable<OrderItem>? orderItems = await Task.Run(() => _repositoryControllerService.Hospitals.GetHospitalsOrderItems(hospital.Id));

        var groupedItems = orderItems.GroupBy(item => item.Product.Medicine.Id);

        List<StoredMedicine> calculatedItems = new();

        foreach (var groupedItem in groupedItems)
        {
            if (groupedItem == null) continue;

            calculatedItems.Add
                (
                new StoredMedicine(
                    groupedItem.First().Product.Medicine, groupedItem.Sum(item => item.Quantity)
                    )
                );
        }

        var regroupedItems = calculatedItems.GroupBy(item => item.Medicine.Type);

        foreach (var type in regroupedItems)
        {
            GroupInfoCollection<Medicine> info = new()
            {
                Key = type.Key + " [ " + type.Sum(item => item.Quantity) + " ]"
            };
            QuantityPerType[type.Key] = type.Sum(item => item.Quantity);
            foreach (var storedItem in type)
            {
                info.Add(storedItem.Medicine);
            }
            GroupedOrders.Add(info);
        }

        GroupedItemsViewSource = new CollectionViewSource { IsSourceGrouped = true, Source = GroupedOrders };

        IsSourceLoading = false;

        //await _dispatcherQueue.EnqueueAsync(() =>
        //{

        //});
    }


    public void OnNavigatedFrom()
    {
        Source.CollectionChanged -= Source_CollectionChanged;
    }
}
