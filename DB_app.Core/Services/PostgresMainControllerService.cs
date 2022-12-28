using DB_app.Core.Contracts.Services;
using Microsoft.EntityFrameworkCore;
using DB_app.Repository.PosgresMain;

namespace DB_app.Repository.Services;


public class PostgresMainControllerService : IRepositoryControllerService
{
    private readonly PostgresContext _db;

    public IHospitalRepository  Hospitals   { get; private set; }
    public IOrderRepository     Orders      { get; private set; }
    public IProductRepository   Products    { get; private set; }
    public IAddressRepository   Addresses   { get; private set; }
    public IPharmacyRepository  Pharmacies  { get; private set; }
    public IMedicineRepository  Medicines   { get; private set; }


    public PostgresMainControllerService(DbContextOptions<PostgresContext> options)
    {
        _db = new PostgresContext(options);
        _db.Database.EnsureCreated();

        Hospitals   = new PostgresHospitalRepository(_db);
        Orders      = new PostgresOrderRepository(_db);
        Products    = new PostgresProductRepository(_db);
        Addresses   = new PostgresAddressRepository(_db);
        Pharmacies  = new PostgresPharmacyRepository(_db);
        Medicines   = new PostgresMedicineRepository(_db);
    }
}