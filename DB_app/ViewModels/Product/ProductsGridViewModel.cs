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

public partial class ProductsGridViewModel : ObservableRecipient, INavigationAware, IRecipient<AddProductMessage>
{
    private readonly IRepositoryControllerService _repositoryControllerService
        = App.GetService<IRepositoryControllerService>();

    /// <summary>
    /// DataGrid's data collection
    /// </summary>
    public ObservableCollection<ProductWrapper> Source { get; set; }
        = new ObservableCollection<ProductWrapper>();


    /// <summary>
    /// Creates a new <see cref="ProductsGridViewModel"/> instance with new <see cref="ProductWrapper"/>
    /// </summary>
    public ProductsGridViewModel()
    {
        //_model = new ProductWrapper();
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

    // TODO this shit isn't working See this to solve - https://xamlbrewer.wordpress.com/2021/01/04/introducing-the-winui-infobar-control/
    [ObservableProperty]
    private InfoBarSeverity _infoBarSeverity = InfoBarSeverity.Informational;
    // Error
    // Informational
    // Warning
    // Success

    [ObservableProperty]
    private string _infoBarMessage = "";


    /// <summary>
    /// Represents selected by user ProductWrapper object
    /// </summary>
    private ProductWrapper? _selectedItem;
    public ProductWrapper? SelectedItem
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
    /// Represents current ProductWrapper object
    /// </summary>
    //public ProductWrapper _model { get; set; }

    ///// <summary>
    ///// City of the current ProductWrapper's data object
    ///// </summary>
    //public string PharmacyName { get => _model.SellingPharmacy.Name; }

    ///// <summary>
    ///// Street of the current ProductWrapper's data object
    ///// </summary>
    //public string MedicineName { get => _model.ContainingMedicine.Name; }

    ///// <summary>
    ///// Building of the current ProductWrapper's data object
    ///// </summary>
    //public string MedicineType { get => _model.ContainingMedicine.Type; }

    ///// <summary>
    ///// Building of the current ProductWrapper's data object
    ///// </summary>
    //public double Price { get => _model.Price; }

    ///// <summary>
    ///// Building of the current ProductWrapper's data object
    ///// </summary>
    //public int Quantity { get => _model.Quantity; }

    #endregion



    public void deleteItem_Click(object sender, RoutedEventArgs e)
    {
        if (_selectedItem != null)
        {
            int id = _selectedItem.ProductData.Id;
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

    public void InsertToGridNewWrapper(ProductWrapper givenProductWrapper)
    {
        givenProductWrapper.isNew = false;
        Source.Insert(0, givenProductWrapper);
        Debug.WriteLine($"so new wrapper is {givenProductWrapper}");
        selectedGridIndex = 0;
    }

    public void SendPrikol()
    {
        WeakReferenceMessenger.Default.Send(new ShowProductDetailsMessage(_selectedItem));
    }


    /// <summary>
    /// Saves any modified ProductWrappers and reloads the ProductWrapper list from the database.
    /// </summary>
    public void UpdateGridWithEditedWrapper(ProductWrapper givenProductWrapper)
    {
        // TODO rename it or something IDK it's just looks terrible imo
        //var foundInSource = Source.FirstOrDefault(wrapper => wrapper.MedicineData.Id == givenProductWrapper.MedicineData.Id);
        //if (foundInSource != null)
        //{
        //    givenProductWrapper.IsModified = false;
        //    int index = Source.IndexOf(foundInSource);
        //    Source[index] = givenProductWrapper;

        //    Debug.WriteLine($"so index = {index} and wrapper is {givenProductWrapper}");
        //    selectedGridIndex = index;
        //}
        SelectedItem = givenProductWrapper;
        OnPropertyChanged("SelectedItem");
    }



    public async void OnNavigatedTo(object parameter)
    {
        if (Source.Count < 1)
        {
            Source.Clear();
            var data = await _repositoryControllerService.Products.GetAsync();

            foreach (var item in data)
            {
                Source.Add(new ProductWrapper(item));
            }
        }
    }

    public void OnNavigatedFrom()
    {
    }

    public void Receive(AddProductMessage message)
    {
        var givenProductWrapper = message.Value;
        if (givenProductWrapper.isNew)
        {
            InsertToGridNewWrapper(givenProductWrapper);
        }
        else
        {
            UpdateGridWithEditedWrapper(givenProductWrapper);
        }
    }
}
