﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using DB_app.Core.Contracts.Services;
using DB_app.Entities;
using DB_app.Services.Messages;

namespace DB_app.ViewModels;

public partial class ProductDetailsViewModel : ObservableRecipient, IRecipient<ShowRecordDetailsMessage<ProductWrapper>>
{
    private readonly IRepositoryControllerService _repositoryControllerService
         = App.GetService<IRepositoryControllerService>();

    public ProductDetailsViewModel()
    {
        CurrentProduct = new()
        {
            IsNew = true
        };
        Initialize();


    }

    public void Initialize()
    {
        AvailablePharmacies = _repositoryControllerService.Pharmacies.GetAsync().Result.ToList();
        AvailableMedicines = _repositoryControllerService.Medicines.GetAsync().Result.ToList();
        WeakReferenceMessenger.Default.Register(this);
    }

    public ProductDetailsViewModel(ProductWrapper product)
    {
        CurrentProduct = product;

        Initialize();
    }

    

    public void Receive(ShowRecordDetailsMessage<ProductWrapper> message)
    {
        CurrentProduct = message.Value;
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

        if (CurrentProduct.IsNew)
        {
            await _repositoryControllerService.Products.InsertAsync(CurrentProduct.ProductData);
        }
        else
        {
            await _repositoryControllerService.Products.UpdateAsync(CurrentProduct.ProductData);
        }
    }

    public void NotifyGridAboutChange() => WeakReferenceMessenger.Default.Send(new AddRecordMessage<ProductWrapper>(CurrentProduct));
}

