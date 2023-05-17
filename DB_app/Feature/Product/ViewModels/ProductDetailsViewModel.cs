
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.WinUI;
using DB_app.Contracts.ViewModels;
using DB_app.Core.Contracts.Services;
using DB_app.Helpers;
using DB_app.Models;
using Microsoft.UI.Dispatching;
using System.Collections.ObjectModel;

namespace DB_app.ViewModels;

public partial class ProductDetailsViewModel : ObservableRecipient, INavigationAware
{
    private readonly IRepositoryControllerService _repositoryControllerService  = App.GetService<IRepositoryControllerService>();

    private readonly DispatcherQueue _dispatcherQueue = DispatcherQueue.GetForCurrentThread();

    private async Task LoadAvailablePharmaciesAsync()
    {
        await _dispatcherQueue.EnqueueAsync(() =>
        {
            IsPharmaciesLoading = true;
        });

        IEnumerable<Pharmacy>? pharmacies = await _repositoryControllerService.Pharmacies.GetAsync();

        await _dispatcherQueue.EnqueueAsync(() =>
        {
            AvailablePharmacies.Clear();
            foreach (Pharmacy pharmacy in pharmacies)
            {
                AvailablePharmacies.Add(pharmacy);
            }

            IsMedicinesLoading = false;
        });
    }

    private async Task LoadAvailableMedicinesAsync()
    {
        await _dispatcherQueue.EnqueueAsync(() =>
        {
            IsMedicinesLoading = true;
        });

        IEnumerable<Medicine> medicines = await _repositoryControllerService.Medicines.GetAsync();

        await _dispatcherQueue.EnqueueAsync(() =>
        {
            AvailablePharmacies.Clear();
            foreach (Medicine medicine in medicines)
            {
                AvailableMedicines.Add(medicine);
            }

            IsMedicinesLoading = false;
        });
    }

    public void OnNavigatedTo(object? parameter)
    {
        if (parameter is ProductWrapper model)
        {
            CurrentProduct = model;
            CurrentProduct.Backup();

            if (CurrentProduct.IsInEdit)
            {
                _ = LoadAvailableMedicinesAsync();
                _ = LoadAvailablePharmaciesAsync();
            }
        }

        if (CurrentProduct.IsNew)
            PageTitle = "New_Product".GetLocalizedValue();
        else
            PageTitle = "Product/Text".GetLocalizedValue() + " #" + CurrentProduct.Id;
    }

    public void OnNavigatedFrom(){ /* Not used */ }


    public ObservableCollection<Pharmacy> AvailablePharmacies= new ObservableCollection<Pharmacy>();

    /// <summary>
    /// Gets or sets a value that indicates whether to show a progress bar. 
    /// </summary>
    [ObservableProperty]
    private bool _isPharmaciesLoading = false;

    public ObservableCollection<Medicine> AvailableMedicines = new ObservableCollection<Medicine>();

    /// <summary>
    /// Gets or sets a value that indicates whether to show a progress bar. 
    /// </summary>
    [ObservableProperty]
    private bool _isMedicinesLoading = false;

    /// <summary>
    /// Current_value ProductWrapper to edit
    /// </summary>
    public ProductWrapper CurrentProduct { get; private set; } = new ProductWrapper{ IsNew = true, IsInEdit = true };

    [ObservableProperty]
    private string _pageTitle;


}

