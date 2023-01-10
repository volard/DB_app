using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using DB_app.Core.Contracts.Services;
using DB_app.Models;
using DB_app.Services.Messages;
using Microsoft.UI.Xaml;
using System.Diagnostics;

namespace DB_app.ViewModels;

public partial class AddressDetailsViewModel : ObservableRecipient, IRecipient<ShowAddressDetailsMessage>
{
    private readonly IRepositoryControllerService _repositoryControllerService
         = App.GetService<IRepositoryControllerService>();

    public AddressDetailsViewModel()
    {
        CurrentAddress = new()
        {
            isNew = true
        };
        WeakReferenceMessenger.Default.Register(this);
    }

    public void Receive(ShowAddressDetailsMessage message)
    {
        Debug.WriteLine("________________");
        Debug.WriteLine($"You know, I'm standing here with the {message.Value} item so peacfully");
        Debug.WriteLine("________________");
        CurrentAddress = message.Value;
        CurrentAddress.NotifyAboutProperties();
    }

    public AddressDetailsViewModel(AddressWrapper AddressWrapper)
    {
        CurrentAddress = AddressWrapper;
        WeakReferenceMessenger.Default.Register(this);
    }

    /// <summary>
    /// Current AddressWrapper to edit
    /// </summary>
    public AddressWrapper CurrentAddress { get; set; }


    /// <summary>
    /// Saves customer data that was edited.
    /// </summary>
    public async Task SaveAsync()
    {
        if (CurrentAddress.isNew) // Create new Address
        {
            await _repositoryControllerService.Addresses.InsertAsync(CurrentAddress.AddressData);
        }
        else // Update existing Address
        {
            await _repositoryControllerService.Addresses.UpdateAsync(CurrentAddress.AddressData);
        }
    }

    public void NotifyGridAboutChange() => WeakReferenceMessenger.Default.Send(new AddAddressMessage(CurrentAddress));
}

