using CommunityToolkit.Mvvm.ComponentModel;
using DB_app.Contracts.ViewModels;
using DB_app.Core.Contracts.Services;
using Microsoft.UI.Xaml;

namespace DB_app.ViewModels;

public partial class AddressDetailsViewModel : ObservableRecipient, INavigationAware
{

    /// <summary>
    /// Current_value AddressWrapper to edit
    /// </summary>
    public AddressWrapper CurrentAddress { get; set; } = new AddressWrapper { IsNew = true, IsInEdit = true };



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
            CurrentAddress.Backup();

            if (CurrentAddress.IsInEdit)
            {
                PageTitle = "Edit address #" + CurrentAddress.Id;
            }
            else
                PageTitle = "Address #" + CurrentAddress.Id;
        }
    }

    public void OnNavigatedFrom()
    {
        // Not used
    }
}

