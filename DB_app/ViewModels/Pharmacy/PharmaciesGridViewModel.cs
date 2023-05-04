using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.ComponentModel;

using DB_app.Contracts.ViewModels;
using DB_app.Core.Contracts.Services;
using DB_app.Models;
using DB_app.Services.Messages;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;
using CommunityToolkit.Mvvm.Messaging;
using System.Diagnostics;
using DB_app.Repository;
using DB_app.Helpers;

namespace DB_app.ViewModels;

public partial class PharmaciesGridViewModel : ObservableRecipient, INavigationAware, IRecipient<DeleteRecordMessage<PharmacyWrapper>>
{
private readonly IRepositoryControllerService _repositoryControllerService
        = App.GetService<IRepositoryControllerService>();

    /// <summary>
    /// DataGrid's data collection
    /// </summary>
    public ObservableCollection<PharmacyWrapper> Source { get; set; }
        = new ObservableCollection<PharmacyWrapper>();

    public PharmaciesGridViewModel()
    {
        WeakReferenceMessenger.Default.Register(this);
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
    private PharmacyWrapper? selectedItem;


    public event EventHandler<ListEventArgs>? OperationRejected;

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

                int id = SelectedItem.Id;
                await _repositoryControllerService.Pharmacies.DeleteAsync(id);

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
            var data = await _repositoryControllerService.Pharmacies.GetAsync();

            foreach (var item in data)
            {
                Source.Add(new PharmacyWrapper(item));
            }
        }
    }

    public void OnNavigatedFrom()
    {
    }
}