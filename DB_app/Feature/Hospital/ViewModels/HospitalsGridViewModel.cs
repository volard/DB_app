using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using DB_app.Contracts.ViewModels;
using DB_app.Core.Contracts.Services;
using DB_app.Services.Messages;
using CommunityToolkit.Mvvm.Messaging;
using DB_app.Helpers;
using DB_app.Repository;
using DB_app.Contracts.Services;
using Microsoft.UI.Dispatching;
using Microsoft.Windows.ApplicationModel.Resources;
using CommunityToolkit.WinUI;

namespace DB_app.ViewModels;

public partial class HospitalsGridViewModel : ObservableRecipient, INavigationAware,
    IRecipient<DeleteRecordMessage<HospitalWrapper>>
{

    public HospitalsGridViewModel()
    {
        WeakReferenceMessenger.Default.Register(this);
    }

    #region Properties

    /// <summary>
    /// Dependency representing Data Repository
    /// </summary>
    private readonly IRepositoryControllerService _repositoryControllerService = App.GetService<IRepositoryControllerService>();

    /// <summary>
    /// DataGrid's data collection
    /// </summary>
    public ObservableCollection<HospitalWrapper> Source { get; set; } = new ObservableCollection<HospitalWrapper>();

    /// <summary>
    /// Gets or sets a value that indicates whether to show a progress bar. 
    /// </summary>
    [ObservableProperty]
    private bool _isLoading;

    private readonly DispatcherQueue _dispatcherQueue = DispatcherQueue.GetForCurrentThread();

    /// <summary>
    /// Dependency representing Resource loader
    /// </summary>
    public readonly ResourceLoader _resourceLoader = App.GetService<ILocalizationService>().ResourceLoader;

    /// <summary>
    /// Represents selected by user HospitalWrapper object
    /// </summary>
    [ObservableProperty]
    private HospitalWrapper? _selectedItem;

    public event EventHandler<ListEventArgs>? OperationRejected;

    private bool IsInactiveEnabled = false;

    #endregion


    #region Members


    public void Receive(DeleteRecordMessage<HospitalWrapper> message)
    {
        var givenHospitalWrapper = message.Value;
        Source.Remove(givenHospitalWrapper);
    }


    /// <summary>
    /// Retrieves items from the data source.
    /// </summary>
    public async void LoadItems()
    {
        await _dispatcherQueue.EnqueueAsync(() =>
        {
            IsLoading = true;
            Source.Clear();
        });

        var items = await Task.Run(_repositoryControllerService.Hospitals.GetAsync);

        await _dispatcherQueue.EnqueueAsync(() =>
        {
            foreach (var item in items)
            {
                Source.Add(new HospitalWrapper(item));
            }

            IsLoading = false;
        });
    }


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
                else ++i;
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
            LoadItems();
        }
    }

    public void OnNavigatedFrom(){}

    #endregion
}