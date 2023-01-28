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

public partial class MedicinesGridViewModel : ObservableRecipient, INavigationAware, IRecipient<AddRecordMessage<MedicineWrapper>>
{
    private readonly IRepositoryControllerService _repositoryControllerService
        = App.GetService<IRepositoryControllerService>();

    /// <summary>
    /// DataGrid's data collection
    /// </summary>
    public ObservableCollection<MedicineWrapper> Source { get; set; }
        = new ObservableCollection<MedicineWrapper>();


    /// <summary>
    /// Creates a new <see cref="MedicinesGridViewModel"/> instance with new <see cref="MedicineWrapper"/>
    /// </summary>
    public MedicinesGridViewModel()
    {
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
    /// Represents selected by user MedicineWrapper object
    /// </summary>
    private MedicineWrapper? _selectedItem;
    public MedicineWrapper? SelectedItem
    {
        get => _selectedItem;
        set
        {
            SetProperty(ref _selectedItem, value);
            IsGridItemSelected = Converters.IsNotNull(value);
        }
    }



    public async void deleteItem_Click(object sender, RoutedEventArgs e)
    {
        if (_selectedItem != null)
        {
            int id = _selectedItem.MedicineData.Id;
            await _repositoryControllerService.Medicines.DeleteAsync(id);
            Source.Remove(_selectedItem);
            InfoBarMessage = "Medicine was deleted";
            InfoBarSeverity = Microsoft.UI.Xaml.Controls.InfoBarSeverity.Success;
            IsInfoBarOpened = true;
        }
    }


    public void SendPrikol()
    {
        WeakReferenceMessenger.Default.Send(new ShowRecordDetailsMessage<MedicineWrapper>(_selectedItem));
    }



    public async void OnNavigatedTo(object parameter)
    {
        if (Source.Count < 1)
        {
            Debug.WriteLine("Collection was recreated from db");
            Source.Clear();
            var data = await _repositoryControllerService.Medicines.GetAsync();

            foreach (var item in data){ Source.Add(new MedicineWrapper(item)); }
        }
    }

    public void OnNavigatedFrom()
    {
    }

    public void Receive(AddRecordMessage<MedicineWrapper> message)
    {
        
    }
}
