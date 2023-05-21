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
using DB_app.Models;
using Windows.System;

namespace DB_app.ViewModels;

public partial class HospitalsGridViewModel : ObservableRecipient, INavigationAware,
    IRecipient<DeleteRecordMessage<HospitalWrapper>>
{

    public HospitalsGridViewModel()
    {
        WeakReferenceMessenger.Default.Register<AddRecordMessage<HospitalWrapper>>(this, (r, m) =>
        {
            if (r is not HospitalsGridViewModel hospitalViewModel) return;
            hospitalViewModel.Source.Insert(0, m.Value);
            OnPropertyChanged(nameof(Source));
        });
    }

    /**************************************/
    #region Properties
    /**************************************/

    /// <summary>
    /// Dependency representing Data Repository
    /// </summary>
    public readonly IRepositoryControllerService _repositoryControllerService = App.GetService<IRepositoryControllerService>();

    /// <summary>
    /// DataGrid's data collection
    /// </summary>
    public ObservableCollection<HospitalWrapper> Source { get; init; } = new ObservableCollection<HospitalWrapper>();

    /// <summary>
    /// Gets or sets a value that indicates whether to show a progress bar. 
    /// </summary>
    [ObservableProperty] private bool _isLoading;

    public readonly Microsoft.UI.Dispatching.DispatcherQueue _dispatcherQueue = Microsoft.UI.Dispatching.DispatcherQueue.GetForCurrentThread();

    /// <summary>
    /// Represents selected by user HospitalWrapper object
    /// </summary>
    [ObservableProperty]
    private HospitalWrapper? _selectedItem;

    private bool _isInactiveEnabled;

    #endregion


    /**************************************/
    #region Members
    /**************************************/


    public void Receive(DeleteRecordMessage<HospitalWrapper> message)
    {
        HospitalWrapper givenHospitalWrapper = message.Value;
        Source.Remove(givenHospitalWrapper);
    }
    


    public async Task ToggleInactive()
    {
        if (!_isInactiveEnabled)
        {
            IEnumerable<Hospital>? inactiveHospitals = await _repositoryControllerService.Hospitals.GetInactiveAsync();
            foreach(Hospital item in inactiveHospitals)
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
        _isInactiveEnabled = !_isInactiveEnabled;
    }

    /// <summary>
    /// Occurs when <c><see cref="CommunityToolkit.WinUI.UI.Controls.InAppNotification"/></c> is displaying
    /// </summary>
    public event EventHandler<NotificationConfigurationEventArgs>? DisplayInAppNotification;


    public async Task DeleteSelected()
    {
        if (SelectedItem == null) return;
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


    public async void OnNavigatedTo(object parameter)
    {

    }
    



    public async void Load()
    {
        await _dispatcherQueue.EnqueueAsync(() =>
        {
            IsLoading = true;
            Source.Clear();
        });

        IEnumerable<Hospital>? items = await Task.Run(_repositoryControllerService.Hospitals.GetAsync);

        await _dispatcherQueue.EnqueueAsync(() =>
        {
            foreach (Hospital item in items)
            {
                Source.Add(new HospitalWrapper(item));
            }
            IsLoading = false;
        });
    }


    public void OnNavigatedFrom(){}

    #endregion

}