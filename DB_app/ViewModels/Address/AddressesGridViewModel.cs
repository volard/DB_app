using CommunityToolkit.Mvvm.ComponentModel;
using DB_app.Contracts.ViewModels;
using DB_app.Core.Contracts.Services;
using DB_app.ViewModels;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_app.ViewModels
{
    public partial class AddressesGridViewModel: ObservableObject, INavigationAware
    {
        private readonly IRepositoryControllerService _repositoryControllerService
    = App.GetService<IRepositoryControllerService>();

        /// <summary>
        /// DataGrid's data collection
        /// </summary>
        public ObservableCollection<AddressWrapper> Source { get; set; }
            = new ObservableCollection<AddressWrapper>();


        /// <summary>
        /// Creates a new <see cref="AddressesGridViewModel"/> instance with new <see cref="AddressWrapper"/>
        /// </summary>
        public AddressesGridViewModel()
        {
            _model = new AddressWrapper();
        }

        /// <summary>
        /// Returns true if the specified value is not null; otherwise, returns false.
        /// </summary>
        public static bool IsNotNull(object? value)
        {
            if (value == null)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Indicates whether user selected Medicine item in the grid
        /// </summary>
        // TODO this shit isn't working See this to solve - https://xamlbrewer.wordpress.com/2021/01/04/introducing-the-winui-infobar-control/
        [ObservableProperty]
        private bool _isGridItemSelected = false;

        [ObservableProperty]
        private bool _isInfoBarOpened = false;

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
        /// Name of the current AddressWrapper's data object
        /// </summary>
        public string City { get => _model.City; }

        /// <summary>
        /// Name of the current AddressWrapper's data object
        /// </summary>
        public string Building { get => _model.Building; }

        /// <summary>
        /// Type of the current AddressWrapper's data object
        /// </summary>
        public string Street { get => _model.Street; }

        #endregion



        public async void deleteItem_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedItem != null)
            {
                int id = _selectedItem.AddressData.id_address;
                await _repositoryControllerService.Medicines.DeleteAsync(id);
                Source.Remove(_selectedItem);

                _infoBarMessage = "Medicine was deleted";
                _infoBarSeverity = Microsoft.UI.Xaml.Controls.InfoBarSeverity.Success;
                _isInfoBarOpened = true;
            }
            else
            {
                Debug.WriteLine(new ArgumentNullException(nameof(_selectedItem)).Message);
            }
        }

        /// <summary>
        /// Saves any modified MedicineWrappers and reloads the MedicineWrapper list from the database.
        /// </summary>
        // TODO add script when new wrapper given
        public void SyncGridWithGivenAddressWrapper(AddressWrapper addressWrapper)
        {
            if (addressWrapper.IsModified)
            {
                // TODO rename it or something IDK it's just looks terrible imo
                var foundInSource = Source.First(wrapper => wrapper.AddressData.id_address == addressWrapper.AddressData.id_address);
                if (foundInSource != null)
                {
                    addressWrapper.IsModified = false;
                    int index = Source.IndexOf(foundInSource);
                    Source[index] = addressWrapper;
                }
            }
            else
            {
                Source.Add(addressWrapper);
            }
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
    }
}
