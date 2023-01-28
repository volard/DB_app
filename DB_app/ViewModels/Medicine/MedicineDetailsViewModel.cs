using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using DB_app.Core.Contracts.Services;
using DB_app.Entities;
using DB_app.Services.Messages;
using Microsoft.UI.Xaml;
using System.Diagnostics;

namespace DB_app.ViewModels;

public partial class MedicineDetailsViewModel : ObservableRecipient, IRecipient<ShowRecordDetailsMessage<MedicineWrapper>>
{
    private readonly IRepositoryControllerService _repositoryControllerService
         = App.GetService<IRepositoryControllerService>();

    public MedicineDetailsViewModel()
    {
        CurrentMedicine = new();
        WeakReferenceMessenger.Default.Register(this);
    }

    public void Receive(ShowRecordDetailsMessage<MedicineWrapper> message)
    {
        CurrentMedicine = message.Value;
        //CurrentMedicine.NotifyAboutAllProperties();
    }

    public MedicineDetailsViewModel(MedicineWrapper medicineWrapper)
    {
        CurrentMedicine = medicineWrapper;
        WeakReferenceMessenger.Default.Register(this);
    }

    /// <summary>
    /// Current MedicineWrapper to edit
    /// </summary>
    public MedicineWrapper CurrentMedicine { get; set; }


    /// <summary>
    /// Saves customer data that was edited.
    /// </summary>
    public async Task SaveAsync()
    {
        if (CurrentMedicine.IsNew) // Create new medicine
        {
            await _repositoryControllerService.Medicines.InsertAsync(CurrentMedicine.MedicineData);
        }
        else // Update existing medicine
        {
            await _repositoryControllerService.Medicines.UpdateAsync(CurrentMedicine.MedicineData);
        }
    }

    public void NotifyGridAboutChange() => WeakReferenceMessenger.Default.Send(new AddRecordMessage<MedicineWrapper>(CurrentMedicine));
}

