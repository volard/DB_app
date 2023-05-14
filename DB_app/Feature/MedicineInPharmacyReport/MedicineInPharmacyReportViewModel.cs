using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.WinUI;
using CommunityToolkit.WinUI.UI.Controls;
using DB_app.Contracts.ViewModels;
using DB_app.Core.Contracts.Services;
using DB_app.Models;
using Microsoft.UI.Dispatching;
using System.Collections.ObjectModel;

namespace DB_app.ViewModels;

public partial class MedicineInPharmacyReportViewModel : ObservableRecipient, INavigationAware
{
    private readonly IRepositoryControllerService _repositoryControllerService = App.GetService<IRepositoryControllerService>();

    private readonly DispatcherQueue _dispatcherQueue = DispatcherQueue.GetForCurrentThread();

    public MedicineInPharmacyReportViewModel()
    {
        
    }

    /// <summary>
    /// DataGrid's data collection
    /// </summary>
    public ObservableCollection<Product> Source { get; set; } = new();
    public ObservableCollection<Pharmacy> AvailablePharmacies { get; set; } = new();



    [ObservableProperty]
    private Pharmacy _selectedPharmacy;

    /// <summary>
    /// Gets or sets a value that indicates whether to show a progress bar. 
    /// </summary>
    [ObservableProperty]
    private bool _isPharmaciesLoading = false;

    [ObservableProperty]
    private bool _isSourceLoading = false;

    public async void OnNavigatedTo(object parameter)
    {
        Source.CollectionChanged += Source_CollectionChanged;
        await LoadAvailablePharmacies();
        SelectedPharmacy = AvailablePharmacies[1];
        LoadSource(SelectedPharmacy);

    }

    private void Source_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        OnPropertyChanged(nameof(Source));
    }

    /// <summary>
    /// Retrieves items from the data source.
    /// </summary>
    public async void LoadSource(Pharmacy pharmacy)
    {
        await _dispatcherQueue.EnqueueAsync(() =>
        {
            IsSourceLoading = true;
            Source.Clear();
        });

        var items = await _repositoryControllerService.Products.GetFromPharmacy(pharmacy.Id);

        

        await _dispatcherQueue.EnqueueAsync(() =>
        {
            foreach (var item in new ObservableCollection<Product>(from item in items
                                                                   orderby item.Medicine.Name ascending
                                                                   select item
                                                     ))
            {
                Source.Add(item);
            }

            IsSourceLoading = false;
        });
    }


    
    /// <summary>
    /// Retrieves items from the data source.
    /// </summary>
    public async Task LoadAvailablePharmacies()
    {
        await _dispatcherQueue.EnqueueAsync(() =>
        {
            IsPharmaciesLoading = true;
            AvailablePharmacies.Clear();
        });

        var items = await Task.Run(_repositoryControllerService.Pharmacies.GetAsync);

        await _dispatcherQueue.EnqueueAsync(() =>
        {
            foreach (var item in items)
            {
                AvailablePharmacies.Add(item);
            }

            IsPharmaciesLoading = false;
        });
    }


    public void OnNavigatedFrom()
    {
        Source.CollectionChanged -= Source_CollectionChanged;
    }
}
