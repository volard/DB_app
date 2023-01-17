using CommunityToolkit.Mvvm.ComponentModel;
using DB_app.Core.Contracts.Services;
using DB_app.Entities;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace DB_app.ViewModels;

public partial class OrderWrapper : ObservableValidator, IEditableObject, IEquatable<OrderWrapper>
{

    #region Constructors

    public OrderWrapper(Order Order)
    {
        OrderData = Order;
        ErrorsChanged += Suspect_ErrorsChanged;
        NotifyAboutProperties();
    }

    public OrderWrapper()
    {
        OrderData = new();
        ErrorsChanged += Suspect_ErrorsChanged;
        NotifyAboutProperties();
    }

    #endregion


    #region Properties

    private readonly IRepositoryControllerService _repositoryControllerService
        = App.GetService<IRepositoryControllerService>();


    public Order OrderData { get; set; }


    [Required(ErrorMessage = "Hospital-customer is Required")]
    public Hospital HospitalCustomer
    {
        get => OrderData.HospitalCustomer;
        set
        {
            ValidateProperty(value);
            if (!GetErrors(nameof(HospitalCustomer)).Any())
            {
                OrderData.HospitalCustomer = value;
                OnPropertyChanged();
                IsModified = true;
                OnPropertyChanged(nameof(CanSave));
            }
        }
    }


    [Required(ErrorMessage = "Shipping address is Required")]
    public Address ShippingAddress
    {
        get => OrderData.ShippingAddress;
        set
        {
            ValidateProperty(value);
            if (!GetErrors(nameof(ShippingAddress)).Any())
            {
                OrderData.ShippingAddress = value;
                OnPropertyChanged();
                IsModified = true;
                OnPropertyChanged(nameof(CanSave));
            }
        }
    }

    private Pharmacy pharmacySeller;

    [Required(ErrorMessage = "Pharmacy-seller is Required")]
    public Pharmacy PharmacySeller
    {
        get => pharmacySeller;
        set
        {
            ValidateProperty(value);
            if (!GetErrors(nameof(PharmacySeller)).Any())
            {
                pharmacySeller = value;
                OnPropertyChanged();
                IsModified = true;
                OnPropertyChanged(nameof(CanSave));
            }

        }
    }


    public int Id { get => OrderData.Id; }

    public ObservableCollection<OrderItem> ObservableItems
    {
        get => new(OrderData.Items);
        set
        {
            OrderData.Items = value.ToList();
            IsModified = true;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// Gets the order's total price.
    /// </summary>
    public double Total => ObservableItems.Sum(item => item.Product.Price * item.Quantity);


    // TODO that looks disgusting. I wonder if functions in xaml bindings works properly for me
    public string Errors
        => string.Join(Environment.NewLine, from ValidationResult e in GetErrors(null) select e.ErrorMessage);
    public string HospitalCustomerErrors
        => string.Join(Environment.NewLine, from ValidationResult e in GetErrors(nameof(HospitalCustomer)) select e.ErrorMessage);
    public string ShippingAddressErrors
        => string.Join(Environment.NewLine, from ValidationResult e in GetErrors(nameof(ShippingAddress)) select e.ErrorMessage);
    public string PharmacySellerErrors
        => string.Join(Environment.NewLine, from ValidationResult e in GetErrors(nameof(PharmacySeller)) select e.ErrorMessage);


    public bool HasHospitalCustomerErrors
        => GetErrors(nameof(HospitalCustomer)).Any();
    public bool HasShippingAddressErrors
        => GetErrors(nameof(ShippingAddress)).Any();
    public bool HasPharmacySellerErrors
        => GetErrors(nameof(PharmacySeller)).Any();

    public bool AreNoErrors
        => !HasErrors;
    public bool CanSave
        => !HasErrors && (isModified || isNew);


    // TODO implement cancel button on notification popup
    // TODO maybe it will be better to create another Order object instead of 
    // keeping the bunch of backuped properties
    public Hospital? BackupedHospitalCustomer;
    public Address? BackupedShippingAddress;
    public Pharmacy?   BackupedPharmacySeller;
    public List<OrderItem>?      BackupedItems;


    /// <summary>
    /// Indicates about changes that is not synced with UI DataGrid
    /// </summary>
    [ObservableProperty]
    private bool isModified = false;

    /// <summary>
    /// Indicates whether its a new object
    /// </summary>
    public bool isNew = false;

    #endregion


    #region Modification methods


    public void NotifyAboutProperties()
    {
        OnPropertyChanged(nameof(Errors));
        OnPropertyChanged(nameof(HospitalCustomerErrors));
        OnPropertyChanged(nameof(ShippingAddressErrors));
        OnPropertyChanged(nameof(PharmacySellerErrors));

        OnPropertyChanged(nameof(HospitalCustomer));
        OnPropertyChanged(nameof(ShippingAddress));
        OnPropertyChanged(nameof(PharmacySeller));
        OnPropertyChanged(nameof(ObservableItems));

        OnPropertyChanged(nameof(HasHospitalCustomerErrors));
        OnPropertyChanged(nameof(HasShippingAddressErrors));
        OnPropertyChanged(nameof(HasPharmacySellerErrors));

        OnPropertyChanged(nameof(CanSave));
    }

    public void Suspect_ErrorsChanged(object sender, DataErrorsChangedEventArgs e)
    {
        NotifyAboutProperties();
    }

    public override string ToString()
        => $"OrderWrapper with OrderData - [ {OrderData} ]";


    public void BuckupData()
    {
        BackupedHospitalCustomer = HospitalCustomer;
        BackupedShippingAddress = ShippingAddress;
        BackupedPharmacySeller = PharmacySeller;
        BackupedItems = ObservableItems.ToList();
    }

    public void ApplyChanges() => IsModified = true;

    public void UndoChanges()
    {
        if (
                BackupedHospitalCustomer != null &&
                BackupedShippingAddress != null &&
                BackupedPharmacySeller != null &&
                BackupedItems != null
           )
        {
            HospitalCustomer = BackupedHospitalCustomer;
            ShippingAddress = BackupedShippingAddress;
            PharmacySeller = BackupedPharmacySeller;
            ObservableItems = new(BackupedItems);

            isModified = true;
        }
        
    }


    #endregion



    #region IEditable implementation
    // TODO figure out how to use this interface correctly...
    public void BeginEdit()
    {
        isModified = true;
        BuckupData();
    }

    public void CancelEdit()
    {
        
        isModified = false;
    }

    public async void EndEdit()
    {
        await _repositoryControllerService.Orders.UpdateAsync(OrderData);
    }

    public bool Equals(OrderWrapper? other) =>
        Id == other?.Id;

    #endregion

    /// <summary>
    /// City of the current OrderWrapper's data object
    /// </summary>
    public string Surename_main_doctor { get => OrderData.HospitalCustomer.MainDoctorSurename; }

    /// <summary>
    /// Street of the current OrderWrapper's data object
    /// </summary>
    public string PharmacyName { get => "placeholder"; }

    public DateTime DatePlaced { get => OrderData.DatePlaced; }
}
