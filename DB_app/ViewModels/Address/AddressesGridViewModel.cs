using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using DB_app.Contracts.ViewModels;
using DB_app.Core.Contracts.Services;
using DB_app.Services.Messages;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Collections.ObjectModel;
using System.Diagnostics;
using static System.Net.Mime.MediaTypeNames;

namespace DB_app.ViewModels;

public partial class AddressesGridViewModel : ObservableRecipient, INavigationAware, IRecipient<Messages>
{
    private readonly IRepositoryControllerService _repositoryControllerService
        = App.GetService<IRepositoryControllerService>();

    /// <summary>
    /// DataGrid's data collection
    /// </summary>
    public ObservableCollection<AddressWrapper> Source { get; set; }
        = new ObservableCollection<AddressWrapper>();


    /// <summary>
    /// Creates a new <see cref="MedicinesGridViewModel"/> instance with new <see cref="AddressWrapper"/>
    /// </summary>
    public AddressesGridViewModel()
    {
        _model = new AddressWrapper();
        WeakReferenceMessenger.Default.Register(this);
    }

    /// <summary>
    /// Indicates whether user selected Medicine item in the grid
    /// </summary>
    [ObservableProperty]
    private bool _isGridItemSelected = false;

    [ObservableProperty]
    private bool _isInfoBarOpened = false;

    [ObservableProperty]
    private int selectedGridIndex;


    [ObservableProperty]
    private InfoBarSeverity _infoBarSeverity = InfoBarSeverity.Informational;
    // Error
    // Informational
    // Warning
    // Success

    [ObservableProperty]
    private string _infoBarMessage = "";


    /// <summary>
    /// Represents selected by user AddressWrapper object
    /// </summary>
    private AddressWrapper? _selectedItem;
    public AddressWrapper? SelectedItem
    {
        get => _selectedItem;
        set
        {
            SetProperty(ref _selectedItem, value);
            IsGridItemSelected = Converters.IsNotNull(value);
        }
    }


    #region Required for DataGrid

    /// <summary>
    /// Represents current AddressWrapper object
    /// </summary>
    public AddressWrapper _model { get; set; }

    /// <summary>
    /// City of the current AddressWrapper's data object
    /// </summary>
    public string City { get => _model.City; }

    /// <summary>
    /// Street of the current AddressWrapper's data object
    /// </summary>
    public string Street { get => _model.Street; }

    /// <summary>
    /// Building of the current AddressWrapper's data object
    /// </summary>
    public string Building { get => _model.Building; }

    #endregion



    public async void DeleteItem_Click()
    {
        if (_selectedItem != null)
        {
            int id = _selectedItem.AddressData.Id;
            await _repositoryControllerService.Addresses.DeleteAsync(id);
            Source.Remove(_selectedItem);

            InfoBarMessage = "Medicine was deleted";
            InfoBarSeverity = Microsoft.UI.Xaml.Controls.InfoBarSeverity.Success;
            IsInfoBarOpened = true;
        }
        else
        {
            Debug.WriteLine(new ArgumentNullException(nameof(_selectedItem)).Message);
        }
    }

    public void InsertToGridNewWrapper(AddressWrapper givenAddressWrapper)
    {
        givenAddressWrapper.isNew = false;
        Source.Insert(0, givenAddressWrapper);
        Debug.WriteLine($"so new wrapper is {givenAddressWrapper}");
        selectedGridIndex = 0;
    }

    public void SendPrikol()
    {
        WeakReferenceMessenger.Default.Send(new ShowAddressDetailsMessage(_selectedItem));
    }


    /// <summary>
    /// Saves any modified AddressWrappers and reloads the AddressWrapper list from the database.
    /// </summary>
    public void UpdateGridWithEditedWrapper(AddressWrapper givenAddressWrapper)
    {
        // TODO rename it or something IDK it's just looks terrible imo
        //var foundInSource = Source.FirstOrDefault(wrapper => wrapper.MedicineData.Id == givenAddressWrapper.MedicineData.Id);
        //if (foundInSource != null)
        //{
        //    givenAddressWrapper.IsModified = false;
        //    int index = Source.IndexOf(foundInSource);
        //    Source[index] = givenAddressWrapper;

        //    Debug.WriteLine($"so index = {index} and wrapper is {givenAddressWrapper}");
        //    selectedGridIndex = index;
        //}
        SelectedItem = givenAddressWrapper;
        OnPropertyChanged("SelectedItem");
    }



    public async void OnNavigatedTo(object parameter)
    {
        if (Source.Count < 1)
        {
            Source.Clear();
            var data = await _repositoryControllerService.Addresses.GetAsync();

            foreach (var item in data)
            {
                Source.Add(new AddressWrapper(item));
            }
        }
    }

    public void OnNavigatedFrom()
    {
    }

    public void Receive(Messages message)
    {
        var givenAddressWrapper = message.Value;
        if (givenAddressWrapper.isNew)
        {
            InsertToGridNewWrapper(givenAddressWrapper);
        }
        else
        {
            UpdateGridWithEditedWrapper(givenAddressWrapper);
        }
    }
}
