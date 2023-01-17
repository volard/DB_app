using AppUIBasics.Helper;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using DB_app.Contracts.ViewModels;
using DB_app.Core.Contracts.Services;
using DB_app.Services.Messages;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace DB_app.ViewModels;

public partial class OrdersGridViewModel : ObservableRecipient, INavigationAware, IRecipient<AddRecordMessage<OrderWrapper>>
{
    private readonly IRepositoryControllerService _repositoryControllerService
        = App.GetService<IRepositoryControllerService>();

    /// <summary>
    /// DataGrid's data collection
    /// </summary>
    public ObservableCollection<OrderWrapper> Source { get; set; }
        = new ObservableCollection<OrderWrapper>();


    /// <summary>
    /// Creates a new <see cref="OrdersGridViewModel"/> instance with new <see cref="OrderWrapper"/>
    /// </summary>
    public OrdersGridViewModel()
    {
        WeakReferenceMessenger.Default.Register(this);
    }

    /// <summary>
    /// Indicates whether user selected Product item in the grid
    /// </summary>
    [ObservableProperty]
    private bool _isGridItemSelected = false;

    [ObservableProperty]
    private bool _isInfoBarOpened = false;

    [ObservableProperty]
    private int selectedGridIndex;

    [ObservableProperty]
    private bool isNewWindowClosed = !WindowHelper.ActiveWindows.Any();


    [ObservableProperty]
    private InfoBarSeverity _infoBarSeverity = InfoBarSeverity.Informational;
    // Error
    // Informational
    // Warning
    // Success

    [ObservableProperty]
    private string _infoBarMessage = "";


    /// <summary>
    /// Represents selected by user OrderWrapper object
    /// </summary>
    private OrderWrapper? _selectedItem;
    public OrderWrapper? SelectedItem
    {
        get => _selectedItem;
        set
        {
            SetProperty(ref _selectedItem, value);
            IsGridItemSelected = Converters.IsNotNull(value);
        }
    }



    public void deleteItem_Click(object sender, RoutedEventArgs e)
    {
        if (_selectedItem != null)
        {
            int id = _selectedItem.OrderData.Id;
            //await _repositoryControllerService.Medicines.DeleteAsync(id);
            //Source.Remove(_selectedItem);

            InfoBarMessage = "Medicine was deleted";
            InfoBarSeverity = Microsoft.UI.Xaml.Controls.InfoBarSeverity.Success;
            IsInfoBarOpened = true;

        }
    }

    public void InsertToGridNewWrapper(OrderWrapper givenOrderWrapper)
    {
        givenOrderWrapper.isNew = false;
        Source.Insert(0, givenOrderWrapper);
        selectedGridIndex = 0;
    }

    public void SendPrikol()
    {
        WeakReferenceMessenger.Default.Send(new ShowRecordDetailsMessage<OrderWrapper>(_selectedItem));
    }


    /// <summary>
    /// Saves any modified OrderWrappers and reloads the OrderWrapper list from the database.
    /// </summary>
    public void UpdateGridWithEditedWrapper(OrderWrapper givenOrderWrapper)
    {
        // TODO rename it or something IDK it's just looks terrible imo
        //var foundInSource = Source.FirstOrDefault(wrapper => wrapper.MedicineData.Id == givenOrderWrapper.MedicineData.Id);
        //if (foundInSource != null)
        //{
        //    givenOrderWrapper.IsModified = false;
        //    int index = Source.IndexOf(foundInSource);
        //    Source[index] = givenOrderWrapper;

        //    Debug.WriteLine($"so index = {index} and wrapper is {givenOrderWrapper}");
        //    selectedGridIndex = index;
        //}
        SelectedItem = givenOrderWrapper;
        OnPropertyChanged("SelectedItem");
    }



    public async void OnNavigatedTo(object parameter)
    {
        if (Source.Count < 1)
        {
            Source.Clear();
            var data = await _repositoryControllerService.Orders.GetAsync();

            foreach (var item in data)
            {
                Source.Add(new OrderWrapper(item));
            }
        }
    }

    public void OnNavigatedFrom()
    {
    }

    public void Receive(AddRecordMessage<OrderWrapper> message)
    {
        var givenOrderWrapper = message.Value;
        if (givenOrderWrapper.isNew)
        {
            InsertToGridNewWrapper(givenOrderWrapper);
        }
        else
        {
            UpdateGridWithEditedWrapper(givenOrderWrapper);
        }
    }
}
