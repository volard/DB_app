using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.ComponentModel;

using DB_app.Contracts.ViewModels;
using DB_app.Core.Contracts.Services;
using DB_app.Models;

namespace DB_app.ViewModels;

public class HospitalsGridViewModel : ObservableRecipient, INavigationAware
{
    private readonly IRepositoryControllerService _sampleDataService
        = App.GetService<IRepositoryControllerService>();


    public ObservableCollection<Hospital> Source { get; } = new ObservableCollection<Hospital>();

    public HospitalsGridViewModel()
    {

    }

    public void OnNavigatedTo(object parameter)
    {

    }

    public void OnNavigatedFrom()
    {
    }
}
