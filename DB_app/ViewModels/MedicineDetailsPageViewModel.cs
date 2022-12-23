using CommunityToolkit.Mvvm.ComponentModel;
using DB_app.Contracts.ViewModels;
using DB_app.Core.Contracts.Services;
using DB_app.Models;
using DB_app;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Navigation;
using System.ComponentModel;
using Microsoft.UI.Xaml;

namespace DB_app.ViewModels;

public class MedicineDetailsPageViewModel : ObservableRecipient, INavigationAware
{
    public bool IsModified { get; set; }

    private readonly IRepositoryControllerService _repositoryControllerService;

    private bool _isNewCustomer;

    public MedicineDetailsPageViewModel()
    {
        _repositoryControllerService = App.GetService<IRepositoryControllerService>();
    }

    public void SayFuckYou(object sender, RoutedEventArgs e) => Console.WriteLine("fuck u dude");

    /// <summary>
    /// Gets or sets a value that indicates whether this is a new customer.
    /// </summary>
    //public bool IsNewCustomer
    //{
    //    get => _isNewCustomer;
    //    set => Set(ref _isNewCustomer, value);
    //}

    /// <summary>
    /// Cancels any in progress edits.
    /// </summary>



    public void OnNavigatedFrom()
    {
        Console.WriteLine("padf");
    }

    public void OnNavigatedTo(object parameter)
    {
        Console.WriteLine("sasdfasdf");
    }

    /// <summary>
    /// Saves customer data that has been edited.
    /// </summary>
    public void SaveAsync()
    {
        //await _repositoryControllerService.Medicines.upsertAsync();
        Console.WriteLine("Im motherfucker to myself. Im selffucker");
    }

}

