using CommunityToolkit.Mvvm.ComponentModel;
using DB_app.Contracts.ViewModels;
using DB_app.Helpers;

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
    private string _pageTitle;



    public void OnNavigatedTo(object? parameter)
    {
        if (parameter is AddressWrapper model)
        {
            CurrentAddress = model;
            CurrentAddress.Backup();
        }

        if (CurrentAddress.IsNew)
            PageTitle = "New_Hospital".GetLocalizedValue();
        else
            PageTitle = "Address/Text".GetLocalizedValue() + " #" + CurrentAddress.Id;
    }

    public void OnNavigatedFrom() { /* Not used */ }
}

