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

namespace DB_app.ViewModels;

public class MedicinesGridViewModel : ObservableRecipient, INavigationAware, INotifyPropertyChanged
{
    private readonly IRepositoryControllerService _repositoryControllerService;

    public ObservableCollection<Medicine> Source { get; set; } = new ObservableCollection<Medicine>();

    private Medicine _model;
    public bool isGridItemSelected = false;
    private Medicine? _selectedMedicine;

    /// <summary> 
    /// Occurs when a property value changes.
    /// </summary>
    public event PropertyChangedEventHandler? PropertyChanged = delegate { };

    /// <summary>
    /// Notifies listeners that a property value has changed.
    /// </summary>
    /// <param name="propertyName">Name of the property used to notify listeners. This
    /// value is optional and can be provided automatically when invoked from compilers
    /// that support <see cref="CallerMemberNameAttribute"/>.</param>
    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    /// <summary>
    /// Gets or sets the selected medicine item.
    /// </summary>
    public Medicine? SelectedMedicine
    {
        get => _selectedMedicine;
        set
        {
            Debug.WriteLine("Hello motherfucker I wanna die already seven thousands times");
            _selectedMedicine= value;
            isGridItemSelected = !isGridItemSelected;
            OnPropertyChanged(nameof(isGridItemSelected));
            //if (Set(ref _selectedMedicine, value)) // if new value doesn't match with
            //    // one that already set
            //{
            //    // set this value to inner private property
            //    _selectedMedicine = value;
            //    if (_selectedMedicine != null) // and if the new value is not null we have to access db
            //    {
            //       // database stuff over here
            //    }
            //    OnPropertyChanged(); // TODO do I need it if in Set() its already envoked
            //}
        }
    }



    /// <summary>
    /// Returns true if the specified value is not null; otherwise, returns false.
    /// </summary>
    public bool IsNotNull(object value)
    {
        if (value == null)
        {
            return true;
        }
        return false;
    }

    public MedicinesGridViewModel(Medicine model)
    {
        _repositoryControllerService = App.GetService<IRepositoryControllerService>();
        _model = model;
    }

    public bool sosiHui = false;

    public void test(object sender, RoutedEventArgs e)
    {
        Debug.WriteLine("Hello bitch");
#pragma warning disable CS8604 // Possible null reference argument.
        Debug.WriteLine(Converters.IsNotNull(SelectedMedicine));
#pragma warning restore CS8604 // Possible null reference argument.
    }

    public MedicinesGridViewModel()
    {
        _repositoryControllerService = App.GetService<IRepositoryControllerService>();
        _model = new Medicine();
    }

    public Medicine Model
    {
        get => _model;
        set
        {
            _model = value;
        }
    }

   public int id_medicine
    {
        get => _model.id_medicine;
    }
    public string Name
    {
        get => _model.Name;
    }
    public string Type
    {
        get => _model.Type;
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
