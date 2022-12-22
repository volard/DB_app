using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.ComponentModel;

using DB_app.Contracts.ViewModels;
using DB_app.Core.Contracts.Services;
using DB_app.Models;
using DB_app.Repository;

namespace DB_app.ViewModels;

public class MedicinesGridViewModel : ObservableRecipient, INavigationAware
{
    private readonly IRepositoryControllerService _repositoryControllerService;

    public ObservableCollection<Medicine> Source { get; } = new ObservableCollection<Medicine>();


    public MedicinesGridViewModel(IRepositoryControllerService sampleDataService)
    {
        _repositoryControllerService = App.GetService<IRepositoryControllerService>();
        
    }

    public async void OnNavigatedTo(object parameter)
    {
        Source.Clear();
        var data = await _repositoryControllerService.Medicines.GetAsync();

        foreach (var item in data)
        {
            Source.Add(item);
        }

    }

    public void OnNavigatedFrom()
    {
    }
}
