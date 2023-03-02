using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using DB_app.Contracts.ViewModels;
using DB_app.Core.Contracts.Services;
using DB_app.Entities;
using DB_app.Services.Messages;

namespace DB_app.ViewModels;

public partial class ProductDetailsViewModel : ObservableRecipient, INavigationAware
{
    private readonly IRepositoryControllerService _repositoryControllerService
         = App.GetService<IRepositoryControllerService>();

    public ProductDetailsViewModel(ProductWrapper? product = null)
    {
        if (product == null)
        {
            CurrentProduct = new()
            {
                IsNew = true
            };
        }
        else { CurrentProduct = product; }

        AvailablePharmacies = _repositoryControllerService.Pharmacies.GetAsync().Result.ToList();
        AvailableMedicines = _repositoryControllerService.Medicines.GetAsync().Result.ToList();
    }

    public void OnNavigatedTo(object? parameter)
    {
        if (parameter is ProductWrapper model)
        {
            CurrentProduct = model;
            CurrentProduct.Backup();
        }
    }

    public void OnNavigatedFrom()
    {
        // Not used
    }



    public List<Pharmacy> AvailablePharmacies;
    public List<Medicine> AvailableMedicines;

    /// <summary>
    /// Current ProductWrapper to edit
    /// </summary>
    public ProductWrapper CurrentProduct { get; set; } = new();


    /// <summary>
    /// Saves customer data that was edited.
    /// </summary>
    public async Task SaveAsync()
    {

        if (CurrentProduct.IsNew)
        {
            await _repositoryControllerService.Products.InsertAsync(CurrentProduct.ProductData);
        }
        else
        {
            await _repositoryControllerService.Products.UpdateAsync(CurrentProduct.ProductData);
        }
    }

}

