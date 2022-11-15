using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.ComponentModel;

using DB_app.Contracts.ViewModels;
using DB_app.Core.Contracts.Services;
using DB_app.Repository;

namespace DB_app.ViewModels;

public class MedicinesGridViewModel : ObservableRecipient, INavigationAware
{
    private readonly IRepositoryControllerService _repositoryControllerService;



    public ObservableCollection<> Source { get; }


    public MedicinesGridViewModel()
    {
        _repositoryControllerService = App.GetService<IRepositoryControllerService>();
        Source = new ObservableCollection<_repositoryControllerService.Medicines>();
        
    }

    public void OnNavigatedTo(object parameter)
    {
        
    }

    public void OnNavigatedFrom()
    {
    }
}
