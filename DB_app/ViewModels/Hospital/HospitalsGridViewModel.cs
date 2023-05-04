using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using DB_app.Contracts.ViewModels;
using DB_app.Core.Contracts.Services;
using DB_app.Services.Messages;
using CommunityToolkit.Mvvm.Messaging;
using DB_app.Helpers;
using DB_app.Repository;
using System.Timers;

namespace DB_app.ViewModels;

public partial class HospitalsGridViewModel : ObservableRecipient, INavigationAware, IRecipient<DeleteRecordMessage<HospitalWrapper>>
{
    private readonly IRepositoryControllerService _repositoryControllerService
        = App.GetService<IRepositoryControllerService>();

    /// <summary>
    /// DataGrid's data collection
    /// </summary>
    public ObservableCollection<HospitalWrapper> Source { get; set; }
        = new ObservableCollection<HospitalWrapper>();

    public HospitalsGridViewModel()
    {
        WeakReferenceMessenger.Default.Register(this);
    }

    public void Receive(DeleteRecordMessage<HospitalWrapper> message)
    {
        var givenHospitalWrapper = message.Value;
        Source.Remove(givenHospitalWrapper);
    }


    /// <summary>
    /// Represents selected by user HospitalWrapper object
    /// </summary>
    [ObservableProperty]
    private HospitalWrapper? _selectedItem;


    public event EventHandler<ListEventArgs>? OperationRejected;

    private bool IsInactiveEnabled = false;

    public async Task ToggleInactive()
    {
        if (!IsInactiveEnabled)
        {
            var inactiveHospitals = await _repositoryControllerService.Hospitals.GetInactiveAsync();
            foreach(var item in inactiveHospitals)
            {
                Source.Insert(0, new HospitalWrapper(item));
            }
        }
        else 
        {
            int i = 0;
            while(i < Source.Count)
            {
                if (!Source[i].IsActive) Source.Remove(Source[i]);
                ++i;
            }
        }
        IsInactiveEnabled = !IsInactiveEnabled;
    }


    public async Task DeleteSelected()
    {
        if (SelectedItem != null)
        {
            try
            {

                int id = SelectedItem.HospitalData.Id;
                await _repositoryControllerService.Addresses.DeleteAsync(id);

                Source.Remove(SelectedItem);

                OperationRejected?.Invoke(this, new ListEventArgs(new List<String>() { "Everything is good" }));

            }
            catch (LinkedRecordOperationException)
            {
                OperationRejected?.Invoke(this, new ListEventArgs(new List<String>() { "Адресс связан с организацией. Удалите связанную организацию, чтобы удалить адрес" }));
            }
        }
    }


    public async void OnNavigatedTo(object parameter)
    {
        if (Source.Count < 1)
        {
            Source.Clear();
            var data = await _repositoryControllerService.Hospitals.GetAsync();

            foreach (var item in data)
            {
                Source.Add(new HospitalWrapper(item));
            }
        }
    }

    public void OnNavigatedFrom()
    {
    }
}