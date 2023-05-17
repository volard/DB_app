using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using DB_app.Contracts.ViewModels;
using DB_app.Core.Contracts.Services;
using DB_app.Services.Messages;
using CommunityToolkit.Mvvm.Messaging;
using DB_app.Repository;
using DB_app.Helpers;
using Microsoft.UI.Dispatching;
using CommunityToolkit.WinUI;

namespace DB_app.ViewModels;

public partial class PharmaciesGridViewModel : ObservableRecipient, INavigationAware, 
    IRecipient<DeleteRecordMessage<PharmacyWrapper>>
{
    private readonly IRepositoryControllerService _repositoryControllerService = App.GetService<IRepositoryControllerService>();

    /// <summary>
    /// DataGrid's data collection
    /// </summary>
    public ObservableCollection<PharmacyWrapper> Source { get; set; } = new();

    public PharmaciesGridViewModel()
    {
        WeakReferenceMessenger.Default.Register<AddRecordMessage<PharmacyWrapper>>(this, (r, m) =>
        {
            if (r is PharmaciesGridViewModel pharmacyViewModel)
            {
                pharmacyViewModel.Source.Insert(0, m.Value);
                OnPropertyChanged(nameof(Source));
            }
        });
    }

    public void Receive(DeleteRecordMessage<PharmacyWrapper> message)
    {
        var givenPharmacyWrapper = message.Value;
        Source.Remove(givenPharmacyWrapper);
    }


    /// <summary>
    /// Represents selected by user AddressWrapper object
    /// </summary>
    [ObservableProperty]
    private PharmacyWrapper? _selectedItem;

    /// <summary>
    /// Occurs when <c><see cref="CommunityToolkit.WinUI.UI.Controls.InAppNotification"/></c> is displaying
    /// </summary>
    public event EventHandler<NotificationConfigurationEventArgs>? OperationRejected;

    private bool IsInactiveEnabled = false;

    public async Task ToggleInactive()
    {
        if (!IsInactiveEnabled)
        {
            var inactivePharmacies = await _repositoryControllerService.Pharmacies.GetInactiveAsync();
            foreach (var item in inactivePharmacies)
            {
                Source.Insert(0, new PharmacyWrapper(item));
            }
        }
        else
        {
            int i = 0;
            while (i < Source.Count)
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
                int id = SelectedItem.Id;
                await _repositoryControllerService.Pharmacies.DeleteAsync(id);

                Source.Remove(SelectedItem);

                OperationRejected?.Invoke(
                    this, new NotificationConfigurationEventArgs("Everything is good", NotificationHelper.SuccessStyle));

            }
            catch (LinkedRecordOperationException)
            {
                OperationRejected?.Invoke(
                    this, new NotificationConfigurationEventArgs("Адресс связан с организацией. Удалите связанную организацию, " +
                    "чтобы удалить адрес", NotificationHelper.ErrorStyle));
            }
        }
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

        var items = await Task.Run(_repositoryControllerService.Pharmacies.GetAsync);

        await _dispatcherQueue.EnqueueAsync(() =>
        {
            foreach (var item in items)
            {
                Source.Add(new PharmacyWrapper(item));
            }

            IsLoading = false;
        });
    }

    /// <summary>
    /// Gets or sets a value that indicates whether to show a progress bar. 
    /// </summary>
    [ObservableProperty]
    private bool _isLoading;

    private readonly DispatcherQueue _dispatcherQueue = DispatcherQueue.GetForCurrentThread();


    public void OnNavigatedTo(object parameter)
    {
        if (Source.Count >= 1) return;
        LoadItems();
    }

    public void OnNavigatedFrom()
    {
    }
}