using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using DB_app.Core.Contracts.Services;
using DB_app.Entities;
using DB_app.Services.Messages;
using System.Collections.ObjectModel;

namespace DB_app.ViewModels;

public partial class HospitalDetailsViewModel : ObservableRecipient, IRecipient<ShowRecordDetailsMessage<HospitalWrapper>>
{

    #region Constructors


    public HospitalDetailsViewModel()
    {
        CurrentHospital = new()
        {
            isNew = true
        };
        WeakReferenceMessenger.Default.Register(this);
        AvailableAddresses = new(getAvailableAddresses());
        pageTitle = "New hospital";
    }



    public HospitalDetailsViewModel(HospitalWrapper HospitalWrapper)
    {
        CurrentHospital = HospitalWrapper;
        WeakReferenceMessenger.Default.Register(this);
        AvailableAddresses = new(getAvailableAddresses());
    }


    #endregion



    #region Members

    public void Receive(ShowRecordDetailsMessage<HospitalWrapper> message)
    {
        CurrentHospital = message.Value;
        CurrentHospital.NotifyAboutProperties();
    }

    /// <summary>
    /// Saves hospital that was edited or created
    /// </summary>
    public async Task SaveAsync()
    {
        if (CurrentHospital.isNew) // Create new medicine
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

    public void NotifyGridAboutChange() => WeakReferenceMessenger.Default.Send(new AddRecordMessage<HospitalWrapper>(CurrentHospital));

    #endregion



    #region Properties

    private readonly IRepositoryControllerService _repositoryControllerService
         = App.GetService<IRepositoryControllerService>();


    private HospitalWrapper currentHospital;

    /// <summary>
    /// Current HospitalWrapper to edit
    /// </summary>
    public HospitalWrapper CurrentHospital
    {
        get
        {
            return currentHospital;
        }
        set
        {
            currentHospital = value;
            pageTitle = "Hospital #" + currentHospital.Id;
            AvailableAddresses = new(getAvailableAddresses());
        }
    }

    public ObservableCollection<Address> AvailableAddresses { get; set; }

    [ObservableProperty]
    public Address selectedAddress;

    [ObservableProperty]
    public string pageTitle;

    #endregion



}