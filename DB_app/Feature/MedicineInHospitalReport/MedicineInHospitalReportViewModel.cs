using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.WinUI;
using DB_app.Contracts.ViewModels;
using DB_app.Core.Contracts.Services;
using DB_app.Models;
using Microsoft.UI.Dispatching;
using System.Collections.ObjectModel;

namespace DB_app.ViewModels;

public partial class MedicineInHospitalReportViewModel : ObservableRecipient, INavigationAware
{

    private readonly IRepositoryControllerService _repositoryControllerService = App.GetService<IRepositoryControllerService>();

    private readonly DispatcherQueue _dispatcherQueue = DispatcherQueue.GetForCurrentThread();

    public MedicineInHospitalReportViewModel()
    {

    }

    /// <summary>
    /// DataGrid's data collection
    /// </summary>
    public ObservableCollection<Order> Source { get; set; } = new();
    public ObservableCollection<Hospital> AvailableHospitals { get; set; } = new();



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
        await LoadAvailableHospitals();
        SelectedHospital = AvailableHospitals[1];
        LoadSource(SelectedHospital);

    }

    private void Source_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        OnPropertyChanged(nameof(Source));
    }

    /// <summary>
    /// Retrieves items from the data source.
    /// </summary>
    public async void LoadSource(Hospital hospital)
    {
        await _dispatcherQueue.EnqueueAsync(() =>
        {
            IsSourceLoading = true;
            Source.Clear();
        });

        var items = await _repositoryControllerService.Orders.GetHospitalOrders(hospital.Id);



        await _dispatcherQueue.EnqueueAsync(() =>
        {
            foreach (var item in items)
            {
                Source.Add(item);
            }

            IsSourceLoading = false;
        });
    }



    /// <summary>
    /// Retrieves items from the data source.
    /// </summary>
    public async Task LoadAvailableHospitals()
    {
        await _dispatcherQueue.EnqueueAsync(() =>
        {
            IsHospitalsLoading = true;
            AvailableHospitals.Clear();
        });

        var items = await Task.Run(_repositoryControllerService.Hospitals.GetAsync);

        await _dispatcherQueue.EnqueueAsync(() =>
        {
            foreach (var item in items)
            {
                AvailableHospitals.Add(item);
            }

            IsHospitalsLoading = false;
        });
    }


    public void OnNavigatedFrom()
    {
        Source.CollectionChanged -= Source_CollectionChanged;
    }
}
