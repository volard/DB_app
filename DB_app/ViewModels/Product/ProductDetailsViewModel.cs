using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using DB_app.Core.Contracts.Services;
using DB_app.Models;
using DB_app.Services.Messages;
using Microsoft.UI.Xaml;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace DB_app.ViewModels;

public partial class ProductDetailsViewModel : ObservableRecipient, IRecipient<ShowProductDetailsMessage>
{
    private readonly IRepositoryControllerService _repositoryControllerService
         = App.GetService<IRepositoryControllerService>();

    public ProductDetailsViewModel()
    {
        CurrentProduct = new()
        {
            isNew = true
        };
        Initialize();


    }

    public void Initialize()
    {
        AvailablePharmacies = _repositoryControllerService.Pharmacies.GetAsync().Result.ToList();
        AvailableMedicines = _repositoryControllerService.Medicines.GetAsync().Result.ToList();
        WeakReferenceMessenger.Default.Register(this);
        CurrentProduct.NotifyAboutProperties();
    }

    public ProductDetailsViewModel(ProductWrapper ProductWrapper)
    {
        CurrentProduct = ProductWrapper;

        Initialize();
    }

    

    public void Receive(ShowProductDetailsMessage message)
    {
        CurrentProduct = message.Value;
        CurrentProduct.NotifyAboutProperties();
    }

    public List<Pharmacy> AvailablePharmacies;
    public List<Medicine> AvailableMedicines;

    /// <summary>
    /// Current ProductWrapper to edit
    /// </summary>
    public ProductWrapper CurrentProduct { get; set; }


    /// <summary>
    /// Saves customer data that was edited.
    /// </summary>
    public async Task SaveAsync()
    {

        if (CurrentProduct.isNew)
        {
            await _repositoryControllerService.Products.InsertAsync(CurrentProduct.ProductData);
        }
        else
        {
            await _repositoryControllerService.Products.UpdateAsync(CurrentProduct.ProductData);
        }
    }

    public void NotifyGridAboutChange() => WeakReferenceMessenger.Default.Send(new AddProductMessage(CurrentProduct));
}

