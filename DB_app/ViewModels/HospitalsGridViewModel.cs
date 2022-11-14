using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.ComponentModel;

using DB_app.Contracts.ViewModels;
using DB_app.Core.Contracts.Services;
using DB_app.Models;

namespace DB_app.ViewModels;

public class HospitalsGridViewModel : ObservableRecipient, INavigationAware
{
    private readonly IDataAccessService _sampleDataService;

    //public ObservableCollection<SampleOrder> Source { get; } = new ObservableCollection<SampleOrder>();

    public HospitalsGridViewModel()
    {
        _sampleDataService = App.GetService<IDataAccessService>();
    }

    //public async void OnNavigatedTo(object parameter)
    public void OnNavigatedTo(object parameter)
    {
        //Source.Clear();

        // TODO: Replace with real data.
        //var data = await _sampleDataService.GetGridDataAsync();

        //foreach (var item in data)
        //{
        //    Source.Add(item);
        //}
    }

    public void OnNavigatedFrom()
    {
    }
}
