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

namespace DB_app.ViewModels;

public class MedicineDetailsPageViewModel : BindableBase, INavigationAware, IEditableObject
{
    public bool IsModified { get; set; }

    private bool _isNewCustomer;

    /// <summary>
    /// Gets or sets a value that indicates whether this is a new customer.
    /// </summary>
    public bool IsNewCustomer
    {
        get => _isNewCustomer;
        set => Set(ref _isNewCustomer, value);
    }

    /// <summary>
    /// Raised when the user cancels the changes they've made to the customer data.
    /// </summary>
    public event EventHandler AddNewCustomerCanceled;

    /// <summary>
    /// Cancels any in progress edits.
    /// </summary>
    public CancelEditsAsync()
    {
        if (IsNewCustomer)
        {
            AddNewCustomerCanceled?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            IsModified = false;
        }
    }



    public void OnNavigatedFrom()
    {
        Console.WriteLine("padf");
    }

    public void OnNavigatedTo(object parameter)
    {
        Console.WriteLine("sasdfasdf");
    }

    public void BeginEdit()
    {
        throw new NotImplementedException();
    }

    public void CancelEdit()
    {
        throw new NotImplementedException();
    }

    public void EndEdit()
    {
        throw new NotImplementedException();
    }
}

