using CommunityToolkit.Mvvm.ComponentModel;
using DB_app.Contracts.ViewModels;
using DB_app.Core.Contracts.Services;

namespace DB_app.ViewModels;

public partial class AddressDetailsViewModel : ObservableRecipient, INavigationAware
{
    private readonly IRepositoryControllerService _repositoryControllerService
         = App.GetService<IRepositoryControllerService>();

    /// <summary>
    /// Current AddressWrapper to edit
    /// </summary>
    public AddressWrapper? CurrentAddress { get; set; }


    /// <summary>
    /// Saves customer data that was edited.
    /// </summary>
    


    public void OnNavigatedTo(object? parameter)
    {
        if (parameter != null)
        {
            CurrentAddress = (AddressWrapper)parameter;
            CurrentAddress.Backup();
        }
        else
        {
            CurrentAddress = new();

        }
    }

    public void OnNavigatedFrom()
    {
        // Not used
    }
}

