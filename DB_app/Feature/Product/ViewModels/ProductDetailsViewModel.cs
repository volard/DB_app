using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using DB_app.Contracts.ViewModels;
using DB_app.Core.Contracts.Services;
using DB_app.Models;
using DB_app.Services.Messages;

namespace DB_app.ViewModels;

public partial class ProductDetailsViewModel : ObservableRecipient, INavigationAware
{
    private readonly IRepositoryControllerService _repositoryControllerService
         = App.GetService<IRepositoryControllerService>();

    public ProductDetailsViewModel()
    {
        AvailablePharmacies = _repositoryControllerService.Pharmacies.GetAsync().Result.ToList();
        AvailableMedicines  = _repositoryControllerService.Medicines.GetAsync().Result.ToList();
    }

    public void OnNavigatedTo(object? parameter)
    {
        if (parameter is ProductWrapper model)
        {
            CurrentProduct = model;
            CurrentProduct.Backup();

            if (CurrentProduct.IsInEdit)
            {
                PageTitle = "Edit hospital #" + CurrentProduct.Id;
            }
            else
                PageTitle = "Hospital #" + CurrentProduct.Id;
        }

    }

    public void OnNavigatedFrom(){ /* Not used */ }


    public List<Pharmacy> AvailablePharmacies;

    public List<Medicine> AvailableMedicines;

    /// <summary>
    /// Current_value ProductWrapper to edit
    /// </summary>
    public ProductWrapper CurrentProduct { get; set; } = new ProductWrapper{ IsNew = true, IsInEdit = true };

    [ObservableProperty]
    private string _pageTitle = "New product";


}

