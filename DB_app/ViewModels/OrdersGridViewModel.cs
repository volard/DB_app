using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.ComponentModel;

using DB_app.Contracts.ViewModels;
using DB_app.Core.Contracts.Services;
using DB_app.Models;

namespace DB_app.ViewModels;

public class OrdersGridViewModel : ObservableRecipient, INavigationAware
{
    
    public OrdersGridViewModel(IRepositoryControllerService sampleDataService)
    {
       
    }

    public async void OnNavigatedTo(object parameter)
    {
        
    }

    public void OnNavigatedFrom()
    {
    }
}
