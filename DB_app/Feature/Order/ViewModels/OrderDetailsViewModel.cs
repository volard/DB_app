using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.WinUI;
using DB_app.Contracts.ViewModels;
using DB_app.Core.Contracts.Services;
using DB_app.Helpers;
using DB_app.Models;
using DB_app.Services.Messages;
using Microsoft.UI.Dispatching;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace DB_app.ViewModels;

public partial class OrderDetailsViewModel : ObservableValidator, INavigationAware
{


    public void OnNavigatedTo(object? parameter)
    {
        if (parameter is OrderWrapper model)
        {
            CurrentOrder = model;
            CurrentOrder.Backup();

            if (CurrentOrder.IsInEdit)
            {
                _ = LoadAvailableHospitalsAsync();
                _ = CurrentOrder.LoadAvailableProducts();
            }
        }

        if (CurrentOrder.IsNew)
            PageTitle = "New_Order".GetLocalizedValue();
        else
            PageTitle = "Order/Text".GetLocalizedValue() + " #" + CurrentOrder.Id;

    }



    

    public void OnNavigatedFrom() {  /* Not used */ }

    #region Members


    private readonly DispatcherQueue _dispatcherQueue = DispatcherQueue.GetForCurrentThread();
    private readonly IRepositoryControllerService _repositoryControllerService = App.GetService<IRepositoryControllerService>();
    public ObservableCollection<Hospital> AvailableHospitals = new ObservableCollection<Hospital>();
    public ObservableCollection<Address> AvailableShippingAddresses = new ObservableCollection<Address>();


    public async Task LoadAvailableShippingAddressesAsync(Hospital hospital)
    {
        IEnumerable<HospitalLocation>? hospitalLocations = await _repositoryControllerService.Hospitals.GetHospitalLocations(hospital.Id);
        IEnumerable<Address> addresses = hospitalLocations.Select(location => location.Address);

        await _dispatcherQueue.EnqueueAsync(() =>
        {
            AvailableShippingAddresses.Clear();
            foreach (Address address in addresses)
            {
                AvailableShippingAddresses.Add(address);
            }
        });
    }


    public async Task LoadAvailableHospitalsAsync()
    {
        IEnumerable<Hospital>? hospitals = await _repositoryControllerService.Hospitals.GetAsync();

        await _dispatcherQueue.EnqueueAsync(() =>
        {
            AvailableHospitals.Clear();
            foreach (Hospital hospital in hospitals)
            {
                AvailableHospitals.Add(hospital);
            }
        });
    }

    #endregion



    #region Properties


    public OrderWrapper CurrentOrder { get; set; } = new OrderWrapper { IsNew = true, IsInEdit = true };



    [ObservableProperty]
    private string? _pageTitle;


    #endregion

}