using CommunityToolkit.Mvvm.ComponentModel;
using DB_app.Contracts.ViewModels;
using DB_app.Core.Contracts.Services;
using DB_app.ViewModels.ObjectWrappers;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Xml.Linq;

namespace DB_app.ViewModels;

public partial class MedicinesGridViewModel : ObservableObject, INavigationAware
{
    private readonly IRepositoryControllerService _repositoryControllerService
        = App.GetService<IRepositoryControllerService>();

    /// <summary>
    /// DataGrid's data collection
    /// </summary>
    public ObservableCollection<MedicineWrapper> Source { get; set; }
        = new ObservableCollection<MedicineWrapper>();


    /// <summary>
    /// Creates a new <see cref="MedicinesGridViewModel"/> instance with new <see cref="MedicineWrapper"/>
    /// </summary>
    public MedicinesGridViewModel()
    {
        _model = new MedicineWrapper();
    }

    /// <summary>
    /// Returns true if the specified value is not null; otherwise, returns false.
    /// </summary>
    public static bool IsNotNull(object? value)
    {
        if (value == null)
        {
            return false;
        }
        return true;
    }

    /// <summary>
    /// Indicates whether user selected Medicine item in the grid
    /// </summary>
    // TODO this shit isn't working See this to solve - https://xamlbrewer.wordpress.com/2021/01/04/introducing-the-winui-infobar-control/
    [ObservableProperty]
    private bool _isGridItemSelected = false;

    [ObservableProperty]
    private bool _isInfoBarOpened = false;

    [ObservableProperty]
    private InfoBarSeverity _infoBarSeverity;
    // Error
    // Informational
    // Warning
    // Success

    [ObservableProperty]
    private string _infoBarMessage;


    /// <summary>
    /// Represents selected by user MedicineWrapper object
    /// </summary>
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


    #region Required for DataGrid

    /// <summary>
    /// Represents current MedicineWrapper object
    /// </summary>
    public MedicineWrapper _model { get; set; }

    /// <summary>
    /// Name of the current MedicineWrapper's data object
    /// </summary>
    public string Name { get => _model.Name; }

    /// <summary>
    /// Type of the current MedicineWrapper's data object
    /// </summary>
    public string Type { get => _model.Type; }

    #endregion



    public async void deleteItem_Click(object sender, RoutedEventArgs e)
    {
        if (_selectedMedicine != null)
        {
            int id = _selectedMedicine.MedicineData.id_medicine;
            await _repositoryControllerService.Medicines.DeleteAsync(id);
            Source.Remove(_selectedMedicine);

            _infoBarMessage = "Medicine was deleted";
            _infoBarSeverity = Microsoft.UI.Xaml.Controls.InfoBarSeverity.Success;
            _isInfoBarOpened = true;
        }
        else
        {
            Debug.WriteLine(new ArgumentNullException(nameof(_selectedMedicine)).Message);
        }
    }

    /// <summary>
    /// Saves any modified MedicineWrappers and reloads the MedicineWrapper list from the database.
    /// </summary>
    public void SyncDataGridWithModified(MedicineWrapper modifiedMedicineWrapper) // TODO WTF brah
    {
        // TODO rename it or something IDK it's just looks terrible imo
        var foundInSource = Source.First(wrapper => wrapper.MedicineData.id_medicine == modifiedMedicineWrapper.MedicineData.id_medicine);
        int index = Source.IndexOf(foundInSource);
        modifiedMedicineWrapper.IsModified = false; // TODO why do we even store this??
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
