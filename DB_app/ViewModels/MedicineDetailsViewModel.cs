using CommunityToolkit.Mvvm.ComponentModel;
using DB_app.Core.Contracts.Services;
using DB_app.Models;
using DB_app.ViewModels.ObjectWrappers;
using Microsoft.UI.Xaml;
using Microsoft.Windows.AppNotifications;
using Microsoft.Windows.AppNotifications.Builder;

namespace DB_app.ViewModels;

public partial class MedicineDetailsViewModel : ObservableObject
{
    private readonly IRepositoryControllerService _repositoryControllerService
         = App.GetService<IRepositoryControllerService>();

    [ObservableProperty]
    private bool _isEditDisabled;

    [ObservableProperty]
    private bool _isEditEnabled;



    public MedicineDetailsViewModel()
    {
        IsEditDisabled = true;
        IsEditEnabled = false;
        type = "";
        name = "";
    }

    public void StartEdit_Click(object sender, RoutedEventArgs e)
    {
        IsEditDisabled = false;
        IsEditEnabled = true;
    }

    private MedicineWrapper? _currentMedicine;

    public MedicineWrapper? CurrentMedicine
    {
        get => _currentMedicine;
        set
        {
            _currentMedicine = value;
            if (_currentMedicine != null)
            {
                _isEditEnabled = true;
                _isEditDisabled = false;
                Name = _currentMedicine.MedicineData.Name;
                Type = _currentMedicine.MedicineData.Type;
            }
            else
            {
                _isEditEnabled = false;
                _isEditDisabled = true;
                Name = "";
                Type = "";
            }

        }
    }

    [ObservableProperty]
    public string name;

    [ObservableProperty]
    public string type;



    /// <summary>
    /// Saves customer data that has been edited.
    /// </summary>
    public async void SaveAsync(object sender, RoutedEventArgs e)
    {

        if (IsEditDisabled) // Create new medicine
        {
            Medicine newMedicine = new()
            {
                Name = this.name,
                Type = this.type
            };
            await _repositoryControllerService.Medicines.InsertAsync(newMedicine);

        }
        else // Update existing medicine
        {
            if (_currentMedicine != null) // TODO WTF is happening here - its redundant but I can't remove it 
            {
                _currentMedicine.Name = this.name;
                _currentMedicine.Type = this.name;
                _currentMedicine.IsModified = true;

                await _repositoryControllerService.Medicines.UpdateAsync(_currentMedicine.MedicineData);
            }

           
        }

    }
}

