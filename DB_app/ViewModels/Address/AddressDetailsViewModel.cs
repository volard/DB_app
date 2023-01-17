using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using DB_app.Contracts.ViewModels;
using DB_app.Core.Contracts.Services;
using DB_app.Services.Messages;
using System.Diagnostics;

namespace DB_app.ViewModels;

public partial class AddressDetailsViewModel : ObservableRecipient, INavigationAware
{
    private readonly IRepositoryControllerService _repositoryControllerService
         = App.GetService<IRepositoryControllerService>();

    /// <summary>
    /// Current AddressWrapper to edit
    /// </summary>
    public AddressWrapper CurrentAddress { get; set; }


    /// <summary>
    /// Saves customer data that was edited.
    /// </summary>
    public async Task SaveAsync()
    {
        await CurrentAddress.ApplyChanges();
        if (CurrentAddress.isNew)
        {
            await _repositoryControllerService.Addresses.InsertAsync(CurrentAddress.AddressData);
        }
        else
        {
            await _repositoryControllerService.Addresses.UpdateAsync(CurrentAddress.AddressData);
        }
    }

    public void OnNavigatedTo(object? parameter)
    {
        if (parameter != null)
        {
            CurrentAddress = (AddressWrapper)parameter;
            CurrentAddress.Backup();
        }
        else
        {
            CurrentAddress = new();
            
        }
    }

    public void OnNavigatedFrom()
    {
        // Not used
    }
}

