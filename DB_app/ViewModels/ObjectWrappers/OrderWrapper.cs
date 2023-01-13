using CommunityToolkit.Mvvm.ComponentModel;
using DB_app.Core.Contracts.Services;
using DB_app.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace DB_app.ViewModels;

public partial class OrderWrapper : ObservableValidator, IEditableObject, IEquatable<OrderWrapper>
{

    /*
     * HospitalCustomer ✅
     * ShippingAddress  ✅
     * PharmacySeller   ✅
     * DatePlaced       ✅
     * Items            ✅
     * 
     * -----
     * Totalcost        ✅
     */


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


    [Required(ErrorMessage = "Pharmacy-seller is Required")]
    public Pharmacy PharmacySeller
    {
        get => OrderData.PharmacySeller;
        set
        {
            ValidateProperty(value);
            if (!GetErrors(nameof(PharmacySeller)).Any())
            {
                OrderData.PharmacySeller = value;
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
    public double Total => ObservableItems.Sum(item => item.UnderlyingProduct.Price * item.Quantity);


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
        Debug.WriteLine("Impossible to undo changes - backuped data is empty");
    }


    #endregion



    #region IEditable implementation
    // TODO figure out how to use this interface correctly...
    public void BeginEdit()
    {
        isModified = true;
        BuckupData();
        Debug.WriteLine($"BeginEdit : For now the editable OrderWrapper = {this}");
    }

    public void CancelEdit()
    {
        Debug.WriteLine("Look at me! Im soooo lazy to implement CancelEdit");
        isModified = false;
    }

    public async void EndEdit()
    {
        Debug.WriteLine($"EndEdit : For now the editable OrderWrapper = {this}");
        await _repositoryControllerService.Orders.UpdateAsync(OrderData);
    }

    public bool Equals(OrderWrapper? other) =>
        Id == other?.Id;

    #endregion

    /// <summary>
    /// City of the current OrderWrapper's data object
    /// </summary>
    public string Surename_main_doctor { get => OrderData.HospitalCustomer.Surename_main_doctor; }

    /// <summary>
    /// Street of the current OrderWrapper's data object
    /// </summary>
    public string PharmacyName { get => OrderData.PharmacySeller.Name; }

    public DateTime DatePlaced { get => OrderData.DatePlaced; }
}
