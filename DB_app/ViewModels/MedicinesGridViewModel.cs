using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using CommunityToolkit.Mvvm.ComponentModel;

using DB_app.Contracts.ViewModels;
using DB_app.Core.Contracts.Services;
using DB_app.Models;
using DB_app.Repository;
using Microsoft.UI.Xaml;
using Microsoft.Windows.AppNotifications;
using Microsoft.Windows.AppNotifications.Builder;
using Windows.Web.AtomPub;

namespace DB_app.ViewModels;

public partial class MedicinesGridViewModel : ObservableRecipient, INavigationAware
{
    private readonly IRepositoryControllerService _repositoryControllerService;
    public ObservableCollection<Medicine> Source { get; set; } = new ObservableCollection<Medicine>();

    private Medicine _model;



    /// <summary>
    /// Creates a new <see cref="MedicinesGridViewModel"/> instance.
    /// </summary>
    public MedicinesGridViewModel(Medicine model)
    {
        _repositoryControllerService = App.GetService<IRepositoryControllerService>();
        _model = model;
        _isGridItemSelected = false;
    }


    public MedicinesGridViewModel()
    {
        _repositoryControllerService = App.GetService<IRepositoryControllerService>();
        _model = new Medicine();
    }


    #region Properties


    #region IsGridItemSelected property

    // NOTE for some reason code generators and NavigationViewHeaderBehavior
    // (https://github.com/microsoft/TemplateStudio/blob/main/docs/UWP/projectTypes/navigationpane.md)
    // can't work toghether in proper way so I have to write everything by hand

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

    private Medicine? _selectedMedicine;
    public Medicine? SelectedMedicine
    {
        get => _selectedMedicine;
        set
        {
            SetProperty(ref _selectedMedicine, value);
            Debug.WriteLine("hui sosi syka");
            IsGridItemSelected = Converters.IsNotNull(value);
        }
    }

    #endregion


    #region Model property

    /// <summary>
    /// Represents current Medicine object
    /// </summary>
    public Medicine Model
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
            int id = _selectedMedicine.id_medicine;
            await _repositoryControllerService.Medicines.DeleteAsync(id);
            Source.Remove(_selectedMedicine);
        }
    }




    public async void OnNavigatedTo(object parameter)
    {
        if (Source.Count < 1) 
        {
            Source.Clear();
            var data = await _repositoryControllerService.Medicines.GetAsync();

            foreach (var item in data)
            {
                Source.Add(item);
            }
        }
    }

    public void OnNavigatedFrom()
    {
    }
}
