using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.ComponentModel;

using DB_app.Contracts.ViewModels;
using DB_app.Core.Contracts.Services;
using DB_app.Models;
using DB_app.Services.Messages;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;
using CommunityToolkit.Mvvm.Messaging;
using System.Diagnostics;

namespace DB_app.ViewModels;

public partial class PharmaciesGridViewModel : ObservableRecipient, INavigationAware, IRecipient<AddPharmacyMessage>
{
    private readonly IRepositoryControllerService _repositoryControllerService
        = App.GetService<IRepositoryControllerService>();

    /// <summary>
    /// DataGrid's data collection
    /// </summary>
    public ObservableCollection<PharmacyWrapper> Source { get; set; }
        = new ObservableCollection<PharmacyWrapper>();


    /// <summary>
    /// Creates a new <see cref="PharmaciesGridViewModel"/> instance with new <see cref="PharmacyWrapper"/>
    /// </summary>
    public PharmaciesGridViewModel()
    {
        _model = new PharmacyWrapper();
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
    /// Represents selected by user PharmacyWrapper object
    /// </summary>
    private PharmacyWrapper? _selectedItem;
    public PharmacyWrapper? SelectedItem
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
    /// Represents current PharmacyWrapper object
    /// </summary>
    public PharmacyWrapper _model { get; set; }

    /// <summary>
    /// Name of the current PharmacyWrapper's data object
    /// </summary>
    public string Surename_main_doctor { get => _model.Name; }


    /// <summary>
    /// Type of the current PharmacyWrapper's data object
    /// </summary>
    public string INN { get => _model.INN; }

    #endregion



    public void deleteItem_Click(object sender, RoutedEventArgs e)
    {
        if (_selectedItem != null)
        {
            int id = _selectedItem.PharmacyData.Id;
            //await _repositoryControllerService.Medicines.DeleteAsync(id);
            //Source.Remove(_selectedItem);

            InfoBarMessage = "Medicine was deleted";
            InfoBarSeverity = Microsoft.UI.Xaml.Controls.InfoBarSeverity.Success;
            IsInfoBarOpened = true;

        }
        else
        {
            Debug.WriteLine(new ArgumentNullException(nameof(_selectedItem)).Message);
        }
    }

    public void InsertToGridNewWrapper(PharmacyWrapper givenPharmacyWrapper)
    {
        givenPharmacyWrapper.isNew = false;
        Source.Insert(0, givenPharmacyWrapper);
        selectedGridIndex = 0;
    }

    public void SendPrikol()
    {
        WeakReferenceMessenger.Default.Send(new ShowPharmacyDetailsMessage(_selectedItem));
    }


    /// <summary>
    /// Saves any modified PharmacyWrappers and reloads the PharmacyWrapper list from the database.
    /// </summary>
    public void UpdateGridWithEditedWrapper(PharmacyWrapper givenPharmacyWrapper)
    {
        // TODO rename it or something IDK it's just looks terrible imo
        //var foundInSource = Source.FirstOrDefault(wrapper => wrapper.PharmacyData.Id == givenPharmacyWrapper.PharmacyData.Id);
        //if (foundInSource != null)
        //{
        //    givenPharmacyWrapper.IsModified = false;
        //    int index = Source.IndexOf(foundInSource);
        //    Source[index] = givenPharmacyWrapper;

        //    Debug.WriteLine($"so index = {index} and wrapper is {givenPharmacyWrapper}");
        //    selectedGridIndex = index;
        //}
        SelectedItem = givenPharmacyWrapper;
        OnPropertyChanged("SelectedItem");
    }



    public async void OnNavigatedTo(object parameter)
    {
        if (Source.Count < 1)
        {
            Source.Clear();
            var data = await _repositoryControllerService.Pharmacies.GetAsync();

            foreach (var item in data)
            {
                Source.Add(new PharmacyWrapper(item));
            }
        }
    }

    public void OnNavigatedFrom()
    {
    }

    public void Receive(AddPharmacyMessage message)
    {
        var givenPharmacyWrapper = message.Value;
        if (givenPharmacyWrapper.isNew)
        {
            InsertToGridNewWrapper(givenPharmacyWrapper);
        }
        else
        {
            UpdateGridWithEditedWrapper(givenPharmacyWrapper);
        }
    }
}