using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using DB_app.Contracts.ViewModels;
using DB_app.Core.Contracts.Services;
using DB_app.Helpers;
using DB_app.Models;
using DB_app.Services.Messages;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace DB_app.ViewModels;

public partial class OrderDetailsViewModel : ObservableValidator, INavigationAware
{


    public void OnNavigatedTo(object? parameter)
    {
        if (parameter is OrderWrapper model)
        {
            CurrentOrder = model;
            CurrentOrder.Backup();

            if (CurrentOrder.IsInEdit)
            {
                
            }
        }

        //if (CurrentOrder.IsNew)
        //    PageTitle = "New_Order".GetLocalizedValue();
        //else
        //    PageTitle = "Order/Text".GetLocalizedValue() + " #" + CurrentOrder.Id;

    }

    public void OnNavigatedFrom() {  /* Not used */ }

    #region Members




    #endregion



    #region Properties


    public OrderWrapper CurrentOrder { get; set; } = new OrderWrapper { IsNew = true, IsInEdit = true };



    [ObservableProperty]
    private string? _pageTitle;


    #endregion

}