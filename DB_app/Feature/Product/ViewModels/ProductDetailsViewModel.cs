
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
    

    public void OnNavigatedTo(object? parameter)
    {
        if (parameter is not ProductWrapper model) return;
        CurrentProduct = model;
            CurrentProduct.Backup();

        if (CurrentProduct.IsInEdit)
        {
            CollectionsHelper.LoadCollectionAsync(AvailableMedicines, _dispatcherQueue, _repositoryControllerService.Medicines.GetAsync);
            CollectionsHelper.LoadCollectionAsync(AvailablePharmacies, _dispatcherQueue, _repositoryControllerService.Pharmacies.GetAsync);
        }
    }

    public void OnNavigatedFrom(){ /* Not used */ }


    public ObservableCollection<Pharmacy> AvailablePharmacies= new ObservableCollection<Pharmacy>();

    public ObservableCollection<Medicine> AvailableMedicines = new ObservableCollection<Medicine>();
    
    /// <summary>
    /// Gets or sets a value that indicates whether to show a progress bar. 
    /// </summary>
    [ObservableProperty]
    private bool _isPharmaciesLoading = false;
    
   

    /// <summary>
    /// Gets or sets a value that indicates whether to show a progress bar. 
    /// </summary>
    [ObservableProperty]
    private bool _isMedicinesLoading = false;

    /// <summary>
    /// Current_value ProductWrapper to edit
    /// </summary>
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(PageTitle))]
    private ProductWrapper _currentProduct = new ProductWrapper{ IsNew = true, IsInEdit = true };


    public string PageTitle
    {
        get
        {
            if (CurrentProduct.IsNew)
                return "New_Product".GetLocalizedValue();
            return "Product/Text".GetLocalizedValue() + " #" + CurrentProduct.Id;
        }
    }


}

