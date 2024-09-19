using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.WinUI;
using DB_app.Core.Contracts.Services;
using DB_app.Models;
using Microsoft.UI.Dispatching;
using System.Collections.ObjectModel;

namespace DB_app.ViewModels;
public struct StoringHospital
{
    public StoringHospital(Hospital hospital, int quantity) : this()
    {
        Hospital = hospital;
        Quantity = quantity;
    }

    public Hospital Hospital { get; set; }
    public int Quantity { get; set; }
}
public partial class HospitalsWithMedicineReportViewModel : ObservableObject
{

    private readonly IRepositoryControllerService _repositoryControllerService = App.GetService<IRepositoryControllerService>();

    private readonly DispatcherQueue _dispatcherQueue = DispatcherQueue.GetForCurrentThread();



    /// <summary>
    /// DataGrid's data collection
    /// </summary>
    public ObservableCollection<StoringHospital> Source { get; init; } = new();
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

        IEnumerable<Hospital>? hospitals = await Task.Run(async () => await _repositoryControllerService.Medicines.GetHospitalsContaining(SelectedMedicine));

        await _dispatcherQueue.EnqueueAsync(() =>
        {
            foreach (Hospital hospital in hospitals)
            {
                Source.Add(
                    new StoringHospital(hospital, 8)
                    ) ;
            }
            IsSourceLoading = false;
        });
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

            
        });
        IsMedicinesLoading = false;
    }
}
