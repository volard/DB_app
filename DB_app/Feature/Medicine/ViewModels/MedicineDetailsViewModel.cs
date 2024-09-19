using CommunityToolkit.Mvvm.ComponentModel;
using DB_app.Contracts.ViewModels;
using DB_app.Helpers;
using Microsoft.UI.Xaml.Controls;

namespace DB_app.ViewModels;

public partial class MedicineDetailsViewModel : ObservableRecipient, INavigationAware
{

    /// <summary>
    /// Current_value AddressWrapper to edit
    /// </summary>
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(PageTitle))]
    private MedicineWrapper _currentMedicine = new MedicineWrapper { IsNew = true, IsInEdit = true };


    /// <summary>
    /// Represents the page's title
    /// </summary>
    public string PageTitle
    {
        get
        {
            if (CurrentMedicine.IsNew)
                return "New_Medicine".GetLocalizedValue();
            return "Medicine/Text".GetLocalizedValue() + " #" + CurrentMedicine.Id;
        }
    }


    public void OnNavigatedTo(object? parameter)
    {
        if (parameter is not MedicineWrapper model) return;
        CurrentMedicine = model;
        CurrentMedicine.Backup();
    }

    public void OnNavigatedFrom() { /* Not used */ }
}

