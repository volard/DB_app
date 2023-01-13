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

public partial class HospitalsGridViewModel : ObservableRecipient, INavigationAware, IRecipient<AddHospitalMessage>
{
    private readonly IRepositoryControllerService _repositoryControllerService
        = App.GetService<IRepositoryControllerService>();

    /// <summary>
    /// DataGrid's data collection
    /// </summary>
    public ObservableCollection<HospitalWrapper> Source { get; set; }
        = new ObservableCollection<HospitalWrapper>();


    /// <summary>
    /// Creates a new <see cref="HospitalsGridViewModel"/> instance with new <see cref="HospitalWrapper"/>
    /// </summary>
    public HospitalsGridViewModel()
    {
        _model = new HospitalWrapper();
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
    /// Represents selected by user HospitalWrapper object
    /// </summary>
    private HospitalWrapper? _selectedItem;
    public HospitalWrapper? SelectedItem
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
    /// Represents current HospitalWrapper object
    /// </summary>
    public HospitalWrapper _model { get; set; }

    /// <summary>
    /// Name of the current HospitalWrapper's data object
    /// </summary>
    public string Surename_main_doctor { get => _model.Surename_main_doctor; }

    /// <summary>
    /// Type of the current HospitalWrapper's data object
    /// </summary>
    public string Name_main_doctor { get => _model.Name_main_doctor; }

    /// <summary>
    /// Type of the current HospitalWrapper's data object
    /// </summary>
    public string Middlename_main_doctor { get => _model.Middlename_main_doctor; }

    /// <summary>
    /// Type of the current HospitalWrapper's data object
    /// </summary>
    public string INN { get => _model.INN; }

    #endregion



    public void deleteItem_Click(object sender, RoutedEventArgs e)
    {
        if (_selectedItem != null)
        {
            int id = _selectedItem.HospitalData.Id;
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

    public void InsertToGridNewWrapper(HospitalWrapper givenHospitalWrapper)
    {
        givenHospitalWrapper.isNew = false;
        Source.Insert(0, givenHospitalWrapper);
        Debug.WriteLine($"so new wrapper is {givenHospitalWrapper}");
        selectedGridIndex = 0;
    }

    public void SendPrikol()
    {
        WeakReferenceMessenger.Default.Send(new ShowHospitalDetailsMessage(_selectedItem));
    }


    /// <summary>
    /// Saves any modified HospitalWrappers and reloads the HospitalWrapper list from the database.
    /// </summary>
    public void UpdateGridWithEditedWrapper(HospitalWrapper givenHospitalWrapper)
    {
        // TODO rename it or something IDK it's just looks terrible imo
        //var foundInSource = Source.FirstOrDefault(wrapper => wrapper.HospitalData.Id == givenHospitalWrapper.HospitalData.Id);
        //if (foundInSource != null)
        //{
        //    givenHospitalWrapper.IsModified = false;
        //    int index = Source.IndexOf(foundInSource);
        //    Source[index] = givenHospitalWrapper;

        //    Debug.WriteLine($"so index = {index} and wrapper is {givenHospitalWrapper}");
        //    selectedGridIndex = index;
        //}
        SelectedItem = givenHospitalWrapper;
        OnPropertyChanged("SelectedItem");
    }



    public async void OnNavigatedTo(object parameter)
    {
        if (Source.Count < 1)
        {
            Source.Clear();
            var data = await _repositoryControllerService.Hospitals.GetAsync();

            foreach (var item in data)
            {
                Source.Add(new HospitalWrapper(item));
            }
        }
    }

    public void OnNavigatedFrom()
    {
    }

    public void Receive(AddHospitalMessage message)
    {
        var givenHospitalWrapper = message.Value;
        if (givenHospitalWrapper.isNew)
        {
            InsertToGridNewWrapper(givenHospitalWrapper);
        }
        else
        {
            UpdateGridWithEditedWrapper(givenHospitalWrapper);
        }
    }
}