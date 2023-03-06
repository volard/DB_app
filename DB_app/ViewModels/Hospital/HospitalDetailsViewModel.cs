using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using DB_app.Contracts.ViewModels;
using DB_app.Core.Contracts.Services;
using DB_app.Entities;
using DB_app.Services.Messages;
using System.Collections.ObjectModel;

namespace DB_app.ViewModels;

public partial class HospitalDetailsViewModel : ObservableRecipient, INavigationAware
{

    #region Constructors


    public HospitalDetailsViewModel(HospitalWrapper? hospital = null)
    {
        if (hospital == null)
        {
            CurrentHospital = new()
            {
                IsNew = true
            };
        }
        else 
        { 
            AvailableAddresses = new(getAvailableAddresses());
            //pageTitle = "New hospital";
        }
    }


    #endregion



    #region Members

    /// <summary>
    /// Saves hospital that was edited or created
    /// </summary>
    public async Task SaveAsync()
    {
        if (CurrentHospital.IsNew) // Create new medicine
        {
            await _repositoryControllerService.Hospitals.InsertAsync(CurrentHospital.HospitalData);
        }
        else // Update existing medicine
        {
            await _repositoryControllerService.Hospitals.UpdateAsync(CurrentHospital.HospitalData);
        }
    }


    [ObservableProperty]
    public Address selectedExistingAddress;


    public IEnumerable<Address> getAvailableAddresses()
    {
        List<Address> _addresses = new();

        foreach (var item in _repositoryControllerService.Hospitals.GetAsync().Result.Select(a => a.Addresses))
            _addresses.AddRange(item);

        return _repositoryControllerService.Addresses.GetAsync().Result.
                 Except(_addresses);
    }


    #endregion



    #region Properties

    private readonly IRepositoryControllerService _repositoryControllerService
         = App.GetService<IRepositoryControllerService>();


    public void OnNavigatedTo(object? parameter)
    {
        if (parameter is HospitalWrapper model)
        {
            CurrentHospital = model;
            //this.PageTitle = "Edit hospital";
            CurrentHospital.Backup();
        }
    }


    //private string PageTitle = "New hospital";

    public void OnNavigatedFrom()
    {
        // Not used
    }



    private HospitalWrapper currentHospital;

    /// <summary>
    /// Current HospitalWrapper to edit
    /// </summary>
    public HospitalWrapper CurrentHospital { get; set; } = new();


    public ObservableCollection<Address> AvailableAddresses { get; set; }

    [ObservableProperty]
    public Address selectedAddress;

    [ObservableProperty]
    public string pageTitle;

    #endregion



}