using CommunityToolkit.Mvvm.ComponentModel;
using DB_app.Core.Contracts.Services;
using DB_app.Models;
using Microsoft.UI.Xaml;

namespace DB_app.ViewModels;

public partial class MedicineDetailsViewModel : ObservableObject
{
    private readonly IRepositoryControllerService _repositoryControllerService
         = App.GetService<IRepositoryControllerService>();

    public MedicineDetailsViewModel()
    {
        currentMedicine = new()
        {
            isNew = true
        };
    }

    public MedicineDetailsViewModel(MedicineWrapper medicineWrapper)
    {
        currentMedicine = medicineWrapper;
    }


    private MedicineWrapper currentMedicine;

    /// <summary>
    /// Current MedicineWrapper to edit
    /// </summary>
    public MedicineWrapper CurrentMedicine
    {
        get 
        {
            return currentMedicine;
        }
        set
        {
            currentMedicine = value;
        }
    }


    /// <summary>
    /// Saves customer data that was edited.
    /// </summary>
    public async Task SaveAsync()
    {
        if (CurrentMedicine.isNew) // Create new medicine
        {
            await _repositoryControllerService.Medicines.InsertAsync(CurrentMedicine.MedicineData);
        }
        else // Update existing medicine
        {
            await _repositoryControllerService.Medicines.UpdateAsync(CurrentMedicine.MedicineData);
        }
    }
}

