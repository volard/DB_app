using CommunityToolkit.Mvvm.ComponentModel;
using DB_app.Contracts.ViewModels;
using DB_app.Core.Contracts.Services;
using Microsoft.UI.Xaml;

namespace DB_app.ViewModels;

public partial class AddressDetailsViewModel : ObservableRecipient, INavigationAware
{

    /// <summary>
    /// Current AddressWrapper to edit
    /// </summary>
    public AddressWrapper CurrentAddress { get; set; } = new();



    /// <summary>
    /// Represents the page's title
    /// </summary>
    [ObservableProperty]
    private string _pageTitle = "New address";



    public void OnNavigatedTo(object? parameter)
    {
        if (parameter is AddressWrapper model)
        {
            CurrentAddress = model;
            //this.PageTitle = "Edit address";
            CurrentAddress.Backup();
        }
    }

    public void OnNavigatedFrom()
    {
        // Not used
    }
}

