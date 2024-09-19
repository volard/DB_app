using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.WinUI;
using DB_app.Core.Contracts.Services;
using DB_app.Models;
using Microsoft.UI.Dispatching;
using System.Collections.ObjectModel;

namespace DB_app.ViewModels;

public partial class PharmacyWithMedicineReportViewModel : ObservableObject
{

    private readonly IRepositoryControllerService _repositoryControllerService = App.GetService<IRepositoryControllerService>();

    private readonly DispatcherQueue _dispatcherQueue = DispatcherQueue.GetForCurrentThread();



    /// <summary>
    /// DataGrid's data collection
    /// </summary>
    public ObservableCollection<Product> Source { get; init; } = new();
    public ObservableCollection<Medicine> AvailableMedicines { get; } = new ObservableCollection<Medicine>();



    [ObservableProperty]
    private Medicine? _selectedMedicine;

    /// <summary>
    /// Gets or sets a value that indicates whether to show a progress bar. 
    /// </summary>
    [ObservableProperty]
    private bool _isMedicinesLoading = false;

    [ObservableProperty]
    private bool _isSourceLoading = false;


    /// <summary>
    /// Retrieves items from the data source.
    /// </summary>
    public async Task LoadSource()
    {
        await _dispatcherQueue.EnqueueAsync(() =>
        {
            IsSourceLoading = true;
            Source.Clear();
        });

        IEnumerable<Product>? products = await Task.Run(async () => await _repositoryControllerService.Products.GetProductsRepresenting(SelectedMedicine));


        await _dispatcherQueue.EnqueueAsync(() =>
        {
            foreach (Product product in products)
            {
                Source.Add(product);
            }

            
        });
        IsSourceLoading = false;
    }

    /// <summary>
    /// Retrieves items from the data source.
    /// </summary>
    public async Task LoadMedicine()
    {
        await _dispatcherQueue.EnqueueAsync(() =>
        {
            IsMedicinesLoading = true;
            AvailableMedicines.Clear();
        });

        IEnumerable<Medicine>? medicines = await Task.Run(_repositoryControllerService.Medicines.GetUnique);


        await _dispatcherQueue.EnqueueAsync(() =>
        {
            foreach (Medicine medicine in medicines)
            {
                AvailableMedicines.Add(medicine);
            }

            IsMedicinesLoading = false;
        });
    }
}
