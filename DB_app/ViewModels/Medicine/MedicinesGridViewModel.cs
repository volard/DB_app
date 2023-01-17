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
        _model = new MedicineWrapper();
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


    #region Required for DataGrid

    /// <summary>
    /// Represents current MedicineWrapper object
    /// </summary>
    public MedicineWrapper _model { get; set; }

    /// <summary>
    /// Name of the current MedicineWrapper's data object
    /// </summary>
    public string Name { get => _model.Name; }

    /// <summary>
    /// Type of the current MedicineWrapper's data object
    /// </summary>
    public string Type { get => _model.Type; }

    #endregion



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

    public void InsertToGridNewWrapper(MedicineWrapper givenMedicineWrapper)
    {
        givenMedicineWrapper.isNew = false;
        Source.Insert(0, givenMedicineWrapper);
        Debug.WriteLine($"so new wrapper is {givenMedicineWrapper}");
        selectedGridIndex = 0;
    }

    public void SendPrikol()
    {
        WeakReferenceMessenger.Default.Send(new ShowRecordDetailsMessage<MedicineWrapper>(_selectedItem));
    }


    /// <summary>
    /// Saves any modified MedicineWrappers and reloads the MedicineWrapper list from the database.
    /// </summary>
    public void UpdateGridWithEditedWrapper(MedicineWrapper givenMedicineWrapper)
    {
        // TODO rename it or something IDK it's just looks terrible imo
        //var foundInSource = Source.FirstOrDefault(wrapper => wrapper.MedicineData.Id == givenMedicineWrapper.MedicineData.Id);
        //if (foundInSource != null)
        //{
        //    givenMedicineWrapper.IsModified = false;
        //    int index = Source.IndexOf(foundInSource);
        //    Source[index] = givenMedicineWrapper;

        //    Debug.WriteLine($"so index = {index} and wrapper is {givenMedicineWrapper}");
        //    selectedGridIndex = index;
        //}
        SelectedItem = givenMedicineWrapper;
        OnPropertyChanged("SelectedItem");
    }



    public async void OnNavigatedTo(object parameter)
    {
        if (Source.Count < 1)
        {
            Source.Clear();
            var data = await _repositoryControllerService.Medicines.GetAsync();

            foreach (var item in data)
            {
                Source.Add(new MedicineWrapper(item));
            }
        }
    }

    public void OnNavigatedFrom()
    {
    }

    public void Receive(AddRecordMessage<MedicineWrapper> message)
    {
        var givenMedicineWrapper = message.Value;
        if (givenMedicineWrapper.isNew)
        {
            InsertToGridNewWrapper(givenMedicineWrapper);
        }
        else
        {
            UpdateGridWithEditedWrapper(givenMedicineWrapper);
        }
    }
}
