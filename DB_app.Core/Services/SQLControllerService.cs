using DB_app.Core.Contracts.Services;
using Microsoft.EntityFrameworkCore;
using DB_app.Repository.SQL;

namespace DB_app.Repository.Services;


public class SQLControllerService : IRepositoryControllerService
{
    private readonly SQLContext _db;

    public IHospitalRepository  Hospitals   { get; private set; }
    public IOrderRepository     Orders      { get; private set; }
    public IProductRepository   Products    { get; private set; }
    public IAddressRepository   Addresses   { get; private set; }
    public IPharmacyRepository  Pharmacies  { get; private set; }
    public IMedicineRepository  Medicines   { get; private set; }


    public SQLControllerService(DbContextOptions<SQLContext> options)
    {
        _db = new SQLContext(options);

        _db.Database.EnsureDeleted();
        _db.Database.EnsureCreated();
        DataSeeder.Seed(_db);

        Hospitals   = new SQLHospitalRepository (_db);
        Orders      = new SQLOrderRepository    (_db);
        Products    = new SQLProductRepository  (_db);
        Addresses   = new SQLAddressRepository  (_db);
        Pharmacies  = new SQLPharmacyRepository (_db);
        Medicines   = new SQLMedicineRepository (_db);

    }
}