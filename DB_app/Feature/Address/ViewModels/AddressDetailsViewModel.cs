using CommunityToolkit.Mvvm.ComponentModel;
using DB_app.Contracts.ViewModels;
using DB_app.Helpers;

namespace DB_app.ViewModels;

public partial class AddressDetailsViewModel : ObservableObject, INavigationAware
{

    /// <summary>
    /// Current_value AddressWrapper to edit
    /// </summary>
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(PageTitle))]
    private AddressWrapper _currentAddress = new AddressWrapper { IsNew = true, IsInEdit = true };



    /// <summary>
    /// Represents the page's title
    /// </summary>
    public string PageTitle
    {
        get
        {
            if (CurrentAddress.IsNew)
                return "New_Hospital".GetLocalizedValue();
            else
                return "Address/Text".GetLocalizedValue() + " #" + CurrentAddress.Id;
        }
    }



    public void OnNavigatedTo(object? parameter)
    {
        if (parameter is AddressWrapper model)
        {
            CurrentAddress = model;
            CurrentAddress.Backup();
        }
        
    }

    public void OnNavigatedFrom() { /* Not used */ }
}

