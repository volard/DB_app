using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using DB_app.Core.Contracts.Services;
using DB_app.Services.Messages;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_app.ViewModels;

public partial class HospitalDetailsViewModel : ObservableRecipient, IRecipient<ShowHospitalDetailsMessage>
{
    private readonly IRepositoryControllerService _repositoryControllerService
         = App.GetService<IRepositoryControllerService>();

    public HospitalDetailsViewModel()
    {
        CurrentHospital = new()
        {
            isNew = true
        };
        WeakReferenceMessenger.Default.Register(this);
    }

    public void Receive(ShowHospitalDetailsMessage message)
    {
        Debug.WriteLine("________________");
        Debug.WriteLine($"You know, I'm standing here with the {message.Value} item so peacfully");
        Debug.WriteLine("________________");
        CurrentHospital = message.Value;
        CurrentHospital.NotifyAboutProperties();
    }

    public HospitalDetailsViewModel(HospitalWrapper HospitalWrapper)
    {
        CurrentHospital = HospitalWrapper;
        WeakReferenceMessenger.Default.Register(this);
    }

    /// <summary>
    /// Current HospitalWrapper to edit
    /// </summary>
    public HospitalWrapper CurrentHospital { get; set; }


    /// <summary>
    /// Saves customer data that was edited.
    /// </summary>
    public async Task SaveAsync()
    {
        if (CurrentHospital.isNew) // Create new medicine
        {
            await _repositoryControllerService.Hospitals.InsertAsync(CurrentHospital.HospitalData);
        }
        else // Update existing medicine
        {
            await _repositoryControllerService.Hospitals.UpdateAsync(CurrentHospital.HospitalData);
        }
    }

    public void NotifyGridAboutChange() => WeakReferenceMessenger.Default.Send(new AddHospitalMessage(CurrentHospital));
}


