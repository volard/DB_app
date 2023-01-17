using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using DB_app.Contracts.ViewModels;
using DB_app.Core.Contracts.Services;
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
    private bool isInactiveShowed = false;

    public bool IsInactiveShowed
    {
        get => isInactiveShowed;
        set
        {
            isInactiveShowed = value;
            if (value)
            {
                _ = AddInactive();
            }
            else
            {
                RemoveInactive();
            }
        }
    }

    public async Task AddInactive()
    {
        var _outOfStock = await _repositoryControllerService.Hospitals.GetInactiveAsync();
        foreach (var item in _outOfStock)
        {
            Source.Add(new HospitalWrapper(item));
        }
    }

    public void RemoveInactive()
    {
        var _data = new List<HospitalWrapper>();
        foreach (var item in Source)
        {
            if (!item.HospitalData.IsActive)
            {
                _data.Add(item);
            }
        }



        foreach (var item in _data)
        {
            Source.Remove(item);
        }
    }


    public async void deleteItem_Click(object sender, RoutedEventArgs e)
    {
        if (_selectedItem != null)
        {
            int id = _selectedItem.HospitalData.Id;
            await _repositoryControllerService.Hospitals.DeleteAsync(id);
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