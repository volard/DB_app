using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.WinUI;
using DB_app.Contracts.ViewModels;
using DB_app.Core.Contracts.Services;
using DB_app.Helpers;
using DB_app.Models;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml.Data;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

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
        SelectedHospital = AvailableHospitals[1];
        await LoadSource(SelectedHospital);
    }


    // NOTE subject to change
    private void Source_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        OnPropertyChanged(nameof(Source));
    }

    // Create grouping for collection
    public ObservableCollection<GroupInfoCollection<OrderItem>> GroupedOrders = new();

    public CollectionViewSource GroupedItemsViewSource = new();



    /// <summary>
    /// Retrieves items from the data source.
    /// </summary>
    public async Task LoadSource(Hospital hospital)
    {
        await _dispatcherQueue.EnqueueAsync(() =>
        {
            IsSourceLoading = true;
            Source.Clear();
        });

        IEnumerable<OrderItem>? orderItems = await Task.Run(() => _repositoryControllerService.Hospitals.GetHospitalsOrderItems(4));

        IEnumerable<OrderItem> enumerableItems = orderItems as OrderItem[] ?? orderItems.ToArray();

        HashSet<string> types = new(enumerableItems.Select(item => item.Product.Medicine.Type));

        List<double> sumsPerType = new();
//GroupedOrders.Clear();

            foreach (string type in types)
            {
                double counter = 0;
                GroupInfoCollection<OrderItem> info = new() { Key = type };
                foreach (OrderItem orderItem in enumerableItems.Where(item => item.Product.Medicine.Type == type))
                {
                    counter += orderItem.LocalTotal;
                    info.Add(orderItem);
                }
                sumsPerType.Add(counter);
                GroupedOrders.Add(info);
            }

            GroupedItemsViewSource = new CollectionViewSource { IsSourceGrouped = true, Source = GroupedOrders };

            //IsSourceLoading = false;

        //await _dispatcherQueue.EnqueueAsync(() =>
        //{
            
        //});
    }


    //public CollectionViewSource GroupData(string groupBy = "Range")
    //{
    //    ObservableCollection<GroupInfoCollection<Order>> groups = new ObservableCollection<GroupInfoCollection<Order>>();
    //    var query = from item in _items
    //                orderby item
    //                group item by item.Type into g
    //                select new { GroupName = g.Key, Items = g };
        
    //    foreach (var g in query)
    //    {
    //        GroupInfoCollection<DataGridDataItem> info = new GroupInfoCollection<DataGridDataItem>();
    //        info.Key = g.GroupName;
    //        foreach (var item in g.Items)
    //        {
    //            info.Add(item);
    //        }

    //        groups.Add(info);
    //    }

    //    groupedItems = new CollectionViewSource();
    //    groupedItems.IsSourceGrouped = true;
    //    groupedItems.Source = groups;

    //    return groupedItems;
    //}



    public void OnNavigatedFrom()
    {
        Source.CollectionChanged -= Source_CollectionChanged;
    }
}
