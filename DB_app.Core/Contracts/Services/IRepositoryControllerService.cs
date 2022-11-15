
using DB_app.Repository;
using Microsoft.EntityFrameworkCore;

namespace DB_app.Core.Contracts.Services;

/// <summary>
/// Defines methods for interacting with the app backend.
/// </summary>
public interface IRepositoryControllerService
{
    /// <summary>
    /// Returns the addresses repository.
    /// </summary>
    IAddressRepository Addresses { get; }

    /// <summary>
    /// Returns the orders repository.
    /// </summary>
    IOrderRepository Orders { get; }

    /// <summary>
    /// Returns the products repository.
    /// </summary>
    IProductRepository Products { get; }

    /// <summary>
    /// Returns the pharmacies repository.
    /// </summary>
    IPharmacyRepository Pharmacies { get; }

    /// <summary>
    /// Returns the hospitals repository.
    /// </summary>
    IHospitalRepository Hospitals { get; }

    /// <summary>
    /// Returns the medicines repository.
    /// </summary>
    IMedicineRepository Medicines { get; }
}