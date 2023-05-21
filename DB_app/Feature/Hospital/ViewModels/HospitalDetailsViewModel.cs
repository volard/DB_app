using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.WinUI;
using DB_app.Contracts.ViewModels;
using DB_app.Core.Contracts.Services;
using DB_app.Helpers;
using DB_app.Models;
using Microsoft.UI.Dispatching;
using System.Collections.ObjectModel;

namespace DB_app.ViewModels;

public partial class HospitalDetailsViewModel : ObservableRecipient, INavigationAware
{
    /**************************************/
    #region Members
    /**************************************/

    public void OnNavigatedTo(object? parameter)
    {
        if (parameter is not HospitalWrapper model) return;
        
        CurrentHospital = model;
        CurrentHospital.Backup();

        if (CurrentHospital.IsInEdit)
        {
            LoadAvailableAddresses();
        }
    }

   public void LoadAvailableAddresses()
       {
           CollectionsHelper.LoadCollectionAsync(
               AvailableAddresses, _dispatcherQueue, _repositoryControllerService.Addresses.GetFreeAddressesAsync
           );
       }

    public void OnNavigatedFrom() { /* Not used */ }

    #endregion


    /**************************************/
    #region Properties
    /**************************************/

    private readonly DispatcherQueue _dispatcherQueue = DispatcherQueue.GetForCurrentThread();

    private readonly IRepositoryControllerService _repositoryControllerService = App.GetService<IRepositoryControllerService>();
    
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(PageTitle))]
    private HospitalWrapper _currentHospital = new HospitalWrapper { IsNew = true, IsInEdit = true };
    

    public ObservableCollection<Address> AvailableAddresses { get; } = new ObservableCollection<Address>();

    

    /// <summary>
    /// Location object that bound to hospital and selected by user. 
    /// </summary>
    [ObservableProperty]
    private HospitalLocation? _selectedExistingLocation;

    /// <summary>
    /// Gets or sets a value that indicates whether to show a progress bar. 
    /// </summary>
    [ObservableProperty]
    private bool _isLoading;

    [ObservableProperty]
    private Address? _selectedAddress;

    public string PageTitle{
        get
        {
             if (CurrentHospital.IsNew)
                return "New_Hospital".GetLocalizedValue();
             return "Hospital/Text".GetLocalizedValue() + " #" + CurrentHospital.Id;
        }
       
    }

    #endregion

}