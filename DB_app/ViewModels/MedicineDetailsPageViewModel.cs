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
using System.Diagnostics;
using Microsoft.UI.Xaml.Controls;

namespace DB_app.ViewModels;

public class MedicineDetailsPageViewModel : INavigationAware
{
    private readonly IRepositoryControllerService _repositoryControllerService;
    public bool isInEdit;

    public MedicineDetailsPageViewModel()
    {
        _repositoryControllerService = App.GetService<IRepositoryControllerService>();
    }



    public void OnNavigatedFrom()
    {
        Debug.WriteLine("Medicine Details Page was closed");
    }

    public void OnNavigatedTo(object parameter)
    {
        Debug.WriteLine("Medicine Details Page was opened");
    }

    /// <summary>
    /// Saves customer data that has been edited.
    /// </summary>
    public void SaveAsync(object sender, RoutedEventArgs e)
    {
        //await _repositoryControllerService.Medicines.upsertAsync();

        Random rnd = new();

        List<string> names = new()
        {
            "Philipp",
            "Eric",
            "Steven",
            "Peter",
            "Alex",
            "Meg"
        };

        List<string> types = new()
        {
            "Box", 
            "Spray",
            "Shit",
            "Funny Stuff",
            "Idiotizm"
        };

        Medicine test = new()
        {
            Name = names.ElementAt(rnd.Next(names.Count)),
            Type = types.ElementAt(rnd.Next(types.Count))
        };

        _repositoryControllerService.Medicines.upsertAsync(test);

    }

}

