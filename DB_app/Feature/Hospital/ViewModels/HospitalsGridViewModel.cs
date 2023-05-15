using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using DB_app.Contracts.ViewModels;
using DB_app.Core.Contracts.Services;
using DB_app.Services.Messages;
using CommunityToolkit.Mvvm.Messaging;
using DB_app.Helpers;
using DB_app.Repository;
using Microsoft.UI.Dispatching;
using CommunityToolkit.WinUI;
using System.Diagnostics;
using System.Collections.Specialized;

namespace DB_app.ViewModels;

public partial class HospitalsGridViewModel : ObservableRecipient, INavigationAware,
    IRecipient<DeleteRecordMessage<HospitalWrapper>>
{

    public HospitalsGridViewModel()
    {
        WeakReferenceMessenger.Default.Register<AddRecordMessage<HospitalWrapper>>(this, (r, m) =>
        {
            if (r is HospitalsGridViewModel hospitalViewModel)
            {
                hospitalViewModel.Source.Insert(0, m.Value);
                OnPropertyChanged(nameof(Source));
            }
        });
    }

    /**************************************/
    #region Properties
    /**************************************/

    /// <summary>
    /// Dependency representing Data Repository
    /// </summary>
    private readonly IRepositoryControllerService _repositoryControllerService = App.GetService<IRepositoryControllerService>();

    /// <summary>
    /// DataGrid's data collection
    /// </summary>
    public ObservableCollection<HospitalWrapper> Source { get; set; } = new();

    /// <summary>
    /// Gets or sets a value that indicates whether to show a progress bar. 
    /// </summary>
    [ObservableProperty]
    private bool _isLoading;

    private readonly DispatcherQueue _dispatcherQueue = DispatcherQueue.GetForCurrentThread();

    /// <summary>
    /// Represents selected by user HospitalWrapper object
    /// </summary>
    [ObservableProperty]
    private HospitalWrapper? _selectedItem;

    private bool IsInactiveEnabled = false;

    #endregion


    /**************************************/
    #region Members
    /**************************************/


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

    /// <summary>
    /// Occurs when <c><see cref="CommunityToolkit.WinUI.UI.Controls.InAppNotification"/></c> is displaying
    /// </summary>
    public event EventHandler<NotificationConfigurationEventArgs>? DisplayInAppNotification;


    public async Task DeleteSelected()
    {
        if (SelectedItem != null)
        {
            try
            {
                int id = SelectedItem.Id;
                await _repositoryControllerService.Hospitals.DeleteAsync(id);

                Source.Remove(SelectedItem);

                DisplayInAppNotification?.Invoke(this, new NotificationConfigurationEventArgs("Операция успешно выполнена", NotificationHelper.SuccessStyle));

            }
            catch (LinkedRecordOperationException)
            {
                DisplayInAppNotification?.Invoke(this, new NotificationConfigurationEventArgs("Адресс связан с организацией. Удалите связанную организацию, чтобы удалить адрес", NotificationHelper.ErrorStyle));
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