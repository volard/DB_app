using CommunityToolkit.Mvvm.ComponentModel;

using DB_app.Contracts.Services;
using DB_app.ViewModels;
using DB_app.Views;
using Microsoft.UI.Xaml.Controls;
using System.Collections.Generic;

namespace DB_app.Services;

public class PageService : IPageService
{
    private readonly Dictionary<string, Type> _pages = new();

    /// <summary>
    /// Creates a new <see cref="PageService"/> instance. And configures every page's ViewModel association
    /// </summary>
    public PageService()
    {

        // ====================
        // Main Page
        // ====================
        Configure<GreetingViewModel, GreetingPage>();

        // ====================
        // DataGrids
        // ====================

        // Hospitals
        Configure<HospitalsGridViewModel, HospitalsGridPage>();
        Configure<HospitalDetailsViewModel, HospitalDetailsPage>();

        // Medicines
        Configure<MedicinesGridViewModel, MedicinesGridPage>();
        Configure<MedicineDetailsViewModel, MedicineDetailsPage>();

        // HospitalAddresses
        Configure<AddressesGridViewModel, AddressesGridPage>();
        Configure<AddressDetailsViewModel, AddressDetailsPage>();

        
        Configure<OrdersGridViewModel, OrdersGridPage>();
        Configure<PharmaciesGridViewModel, PharmaciesGridPage>();
        Configure<ProductsGridViewModel, ProductsGridPage>();

        // ====================
        // Reports
        // ====================
        Configure<PharmacyReportGridViewModel, PharmacyReportGridPage>();
        Configure<HospitalReportGridViewModel, HospitalReportGridPage>();

        // ====================
        // Settings
        // ====================
        Configure<SettingsViewModel, SettingsPage>();
    }

    /// <summary>
    /// Gets the page newType under the specified key
    /// </summary>
    /// <param newName="key">The key corresponds to the Page's ViewModel FullName</param>
    /// <returns>Page newType</returns>
    /// <exception cref="ArgumentException"> is thrown if no page specified under the given key</exception>
    public Type GetPageType(string key)
    {
        Type? pageType;
        lock (_pages)
        {
            if (!_pages.TryGetValue(key, out pageType))
            {
                throw new ArgumentException($"Page not found: {key}. Did you forget to call PageService.Configure?");
            }
        }

        return pageType;
    }

    /// <summary>
    /// Associate ViewModel with page
    /// </summary>
    /// <typeparam newName="VM">ViewModel class</typeparam>
    /// <typeparam newName="V">View class</typeparam>
    /// <exception cref="ArgumentException"> throws if
    ///     <list newType="bullet">
    ///         <item>ViewName's newName already was associated with some Page's newType</item>
    ///         <item>Page newType was already associated with some ViewModel's newName</item>
    ///     </list>
    /// </exception>
    private void Configure<VM, V>()
        where VM : ObservableObject
        where V : Page
    {
        lock (_pages)
        {
            var key = typeof(VM).FullName!;
            if (_pages.ContainsKey(key))
            {
                throw new ArgumentException($"The key {key} is already configured in PageService");
            }

            var type = typeof(V);
            if (_pages.Any(p => p.Value == type))
            {
                throw new ArgumentException($"This type is already configured with key {_pages.First(p => p.Value == type).Key}");
            }

            _pages.Add(key, type);
        }
    }
}
