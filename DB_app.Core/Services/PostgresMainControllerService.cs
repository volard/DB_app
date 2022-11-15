using DB_app.Core.Contracts.Services;
using Microsoft.EntityFrameworkCore;
using DB_app.Models;
using Microsoft.Extensions.Options;
using DB_app.Repository.PosgresMain;

namespace DB_app.Repository.Services;

/// <summary>
/// Entity Framework Core DbContext for Contoso.
/// </summary>
public class PostgresMainControllerService : IRepositoryControllerService
{
    private readonly DbContextOptions<PostgresContext> _dbOptions;

    /// <summary>
    /// Creates a new Main DbContext.
    /// </summary>
    public PostgresMainControllerService(DbContextOptions<PostgresContext> options)
    {
        _dbOptions = options;
    }


    public IHospitalRepository Hospitals => new PostgresHospitalRepository(
        new PostgresContext(_dbOptions));

    public IOrderRepository Orders => new PostgresOrderRepository(
        new PostgresContext(_dbOptions));

    public IProductRepository Products => new PostgresProductRepository(
        new PostgresContext(_dbOptions));

    public IAddressRepository Addresses => new PostgresAddressRepository(
        new PostgresContext(_dbOptions));

    public IPharmacyRepository Pharmacies => new PostgresPharmacyRepository(
        new PostgresContext(_dbOptions));

    public IMedicineRepository Medicines => new PostgresMedicineRepository(
        new PostgresContext(_dbOptions));

}
