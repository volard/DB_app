using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.WinUI;
using DB_app.Contracts.ViewModels;
using DB_app.Core.Contracts.Services;
using DB_app.Helpers;
using DB_app.Models;
using DB_app.Repository;
using DB_app.Services.Messages;
using Microsoft.UI.Dispatching;
using System.Collections.ObjectModel;

namespace DB_app.ViewModels;

public partial class MedicinesGridViewModel : ObservableRecipient, INavigationAware, IRecipient<DeleteRecordMessage<MedicineWrapper>>
{

    /// <summary>
    /// Dependency representing Data Repository
    /// </summary>
    private readonly IRepositoryControllerService _repositoryControllerService = App.GetService<IRepositoryControllerService>();

    /// <summary>
    /// DataGrid's data collection
    /// </summary>
    public ObservableCollection<MedicineWrapper> Source { get; init; } = new ObservableCollection<MedicineWrapper>();

    public MedicinesGridViewModel()
    {
        WeakReferenceMessenger.Default.Register<AddRecordMessage<MedicineWrapper>>(this, (r, m) =>
        {
            if (r is not MedicinesGridViewModel medicinesViewModel) return;
            medicinesViewModel.Source.Insert(0, m.Value);
            OnPropertyChanged(nameof(Source));
        });
    }

    public void Receive(DeleteRecordMessage<MedicineWrapper> message)
    {
        MedicineWrapper givenMedicineWrapper = message.Value;
        Source.Remove(givenMedicineWrapper);
    }


    /// <summary>
    /// Represents selected by user AddressWrapper object
    /// </summary>
    [ObservableProperty]
    private MedicineWrapper? _selectedItem;


    public event EventHandler<NotificationConfigurationEventArgs>? DisplayNotification;


    public async Task DeleteSelected()
    {
        if (SelectedItem != null)
        {
            try
            {

                int id = SelectedItem.Id;
                await _repositoryControllerService.Medicines.DeleteAsync(id);

                Source.Remove(SelectedItem);

                DisplayNotification?.Invoke(this, new NotificationConfigurationEventArgs("Everything is good", NotificationHelper.SuccessStyle));

            }
            catch (LinkedRecordOperationException)
            {
                DisplayNotification?.Invoke(this, new NotificationConfigurationEventArgs("Таблэтки связаны с чем-то. Удалите связанную организацию, чтобы удалить адрес", NotificationHelper.ErrorStyle));
            }
        }
    }

    /// <summary>
    /// Gets or sets a value that indicates whether to show a progress bar. 
    /// </summary>
    [ObservableProperty]
    private bool _isLoading;

    private readonly DispatcherQueue _dispatcherQueue = DispatcherQueue.GetForCurrentThread();

    public async void Load()
    {
        await _dispatcherQueue.EnqueueAsync(() =>
        {
            IsLoading = true;
            Source.Clear();
        });

        IEnumerable<Medicine>? items = await Task.Run(_repositoryControllerService.Medicines.GetAsync);

        await _dispatcherQueue.EnqueueAsync(() =>
        {
            foreach (var item in items)
            {
                Source.Add(new MedicineWrapper(item));
            }
            IsLoading = false;
        });
    }

    public void OnNavigatedTo(object parameter)
    {
        if (Source.Count >= 1) return;
        CollectionsHelper.LoadCollectionAsync<MedicineWrapper>(
            Source, _dispatcherQueue, async () =>
            {
                IEnumerable<Medicine> itemsOrigins = await _repositoryControllerService.Medicines.GetAsync();
                return itemsOrigins.Select(item => new MedicineWrapper(item));
            }
        );
    }

    public void OnNavigatedFrom() { }

}
