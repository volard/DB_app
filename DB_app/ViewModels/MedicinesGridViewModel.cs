using CommunityToolkit.Mvvm.ComponentModel;
using DB_app.Contracts.ViewModels;
using DB_app.Core.Contracts.Services;
using DB_app.Models;
using DB_app.ViewModels.ObjectWrappers;
using Microsoft.UI.Xaml;
using System.Collections.ObjectModel;

namespace DB_app.ViewModels;

public partial class MedicinesGridViewModel : ObservableObject, INavigationAware
{
    private readonly IRepositoryControllerService _repositoryControllerService
        = App.GetService<IRepositoryControllerService>();
    public ObservableCollection<MedicineWrapper> Source { get; set; } = new ObservableCollection<MedicineWrapper>();

    
    

    /// <summary>
    /// Creates a new <see cref="MedicinesGridViewModel"/> instance.
    /// </summary>
    //public MedicinesGridViewModel(MedicineWrapper model)
    //{
    //    _model = model;
    //    _isGridItemSelected = false;
    //}

    /// <summary>
    /// Creates a new <see cref="MedicinesGridViewModel"/> instance with new <see cref="MedicineWrapper"/>
    /// </summary>
    public MedicinesGridViewModel()
    {
        _model = new MedicineWrapper();
    }


    #region Properties


    #region IsGridItemSelected property

    /// <summary>
    /// Indicates whether user selected Medicine item in the grid
    /// </summary>
    private bool _isGridItemSelected;
    public bool IsGridItemSelected
    {
        get => _isGridItemSelected;
        set
        {
            SetProperty(ref _isGridItemSelected, value);
        }
    }

    #endregion


    #region SelectedMedicine property

    private MedicineWrapper? _selectedMedicine;
    public MedicineWrapper? SelectedMedicine
    {
        get => _selectedMedicine;
        set
        {
            SetProperty(ref _selectedMedicine, value);
            IsGridItemSelected = Converters.IsNotNull(value);
        }
    }

    #endregion


    #region Model property

    private MedicineWrapper _model;

    /// <summary>
    /// Represents current Medicine object
    /// </summary>
    public MedicineWrapper Model
    {
        get => _model;
        set
        {
            _model = value;
        }
    }
    public string Name
    {
        get => _model.Name;
    }
    public string Type
    {
        get => _model.Type;
    }

    #endregion


    #endregion

    public async void deleteItem_Click(object sender, RoutedEventArgs e)
    {
        if (_selectedMedicine != null)
        {
            int id = _selectedMedicine.MedicineData.id_medicine;
            await _repositoryControllerService.Medicines.DeleteAsync(id);
            Source.Remove(_selectedMedicine);
        }
    }


    /// <summary>
    /// Saves any modified MedicineWrappers and reloads the MedicineWrapper list from the database.
    /// </summary>
    public void SyncDataGridWithModified(MedicineWrapper modifiedMedicineWrapper) // TODO WTF brah
    {
        // TODO rename it or something IDK it's just looks terrible imo
        var foundInSource = Source.First(wrapper => wrapper.MedicineData.id_medicine == modifiedMedicineWrapper.MedicineData.id_medicine);
        modifiedMedicineWrapper.IsModified = false; // TODO why do we even store this??
        int index = Source.IndexOf(foundInSource);
        Source[index] = modifiedMedicineWrapper;
    }




    public async void OnNavigatedTo(object parameter)
    {
        if (Source.Count < 1)
        {
            Source.Clear();
            var data = await _repositoryControllerService.Medicines.GetAsync();

            foreach (var item in data)
            {
                Source.Add(new MedicineWrapper(item));
            }
        }
    }

    public void OnNavigatedFrom()
    {
    }
}
