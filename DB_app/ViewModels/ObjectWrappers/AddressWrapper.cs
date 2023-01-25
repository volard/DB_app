﻿using CommunityToolkit.Mvvm.ComponentModel;
using DB_app.Core.Contracts.Services;
using DB_app.Entities;
using Microsoft.UI.Xaml;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DB_app.ViewModels;

/// <summary>
/// Provides wrapper for the Address model class, encapsulating various services for access by the UI.
/// </summary>
public sealed partial class AddressWrapper : ObservableValidator, IEditableObject
{

    public AddressWrapper(Address? address = null)
    {
        if (address == null)
        {
            IsNew = true;
            AddressData = new();
        }
        else { AddressData = address; }
        ErrorsChanged += Handler_ErrorsChanged;
    }

    private void Handler_ErrorsChanged(object? sender, DataErrorsChangedEventArgs e)
    => NotifyAboutProperties();

    public override string ToString() => 
        $"AddressWrapper with addressData {AddressData}";

    #region Properties

    private Address _addressData = null!;

    public Address AddressData 
    {
        get => _addressData;
        set 
        {   
            _addressData = value;
            City = _addressData.City;
            Street = _addressData.Street;
            Building = _addressData.Building;
            NotifyAboutProperties();
        } 
    }


    [ObservableProperty]
    [NotifyDataErrorInfo]
    [NotifyPropertyChangedFor(nameof(CanSave))]
    [Required(ErrorMessage = "City is Required")]
    private string _city;

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [NotifyPropertyChangedFor(nameof(CanSave))]
    [Required(ErrorMessage = "Street is Required")]
    private string _street;

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [NotifyPropertyChangedFor(nameof(CanSave))]
    [Required(ErrorMessage = "Building is Required")]
    private string _building;

    public void NotifyAboutProperties()
    {
        OnPropertyChanged(nameof(City));
        OnPropertyChanged(nameof(Street));
        OnPropertyChanged(nameof(Building));
        OnPropertyChanged(nameof(HasCityErrors));
    }
        
    public string GetPropertyErrors(string type)
    => string.Join(Environment.NewLine, from ValidationResult e in GetErrors(type) select e.ErrorMessage);

    public string Errors 
    => string.Join(Environment.NewLine, from ValidationResult e in GetErrors(null) select e.ErrorMessage);

    public bool HasCityErrors
    => GetErrors(nameof(City)).Any();

    public bool CanSave => !HasErrors;

    public Visibility HasPropertyErrors(string type)
    => Converters.VisibleIf(GetErrors(type).Any());

    

    public int Id { get => _addressData.Id; }


    /// <summary>
    /// Indicates about changes that is not synced with UI DataGrid
    /// </summary>
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CanSave))]
    private bool isModified = false;

    /// <summary>
    /// indicates whether its a new object
    /// </summary>
    public bool IsNew { get; private set; } = false;

    #endregion


    #region Modification methods


    /// <summary>
    /// Go back to prevoius data after updating
    /// </summary>
    public async Task Revert()
    {
        if (_backupData != null)
        {
            AddressData = _backupData;
            await App.GetService<IRepositoryControllerService>().Addresses.UpdateAsync(_addressData);
        }
    }

    private Address? _backupData;

    public void Backup() =>
        _backupData = _addressData;

    #endregion


    #region IEditable implementation


    public void BeginEdit()
    => Backup();

    public void CancelEdit()
    {
        City = _addressData.City;
        Street = _addressData.Street;
        Building = _addressData.Building;
        IsModified = false;
    }

    public void EndEdit()
    {
        _addressData.City = City;
        _addressData.Street = Street;
        _addressData.Building = Building;
    }


    public async Task<bool> SaveAsync()
    {
        ValidateAllProperties();
        if (HasErrors) return false;
        EndEdit();
        if (IsNew)
        {
            await App.GetService<IRepositoryControllerService>().Addresses.InsertAsync(AddressData);
        }
        else
        {
            await App.GetService<IRepositoryControllerService>().Addresses.UpdateAsync(AddressData);
        }
        return true;
    }



    public override bool Equals(object? obj)
    {
        if (obj is not AddressWrapper other) return false;
        return
            City == other?.City &&
            Street == other?.Street &&
            Building == other?.Building;
    }



    // TOOD GetHashCode() implementation
    


    #endregion
}