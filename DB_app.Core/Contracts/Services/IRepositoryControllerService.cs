
using DB_app.Repository;
using Microsoft.EntityFrameworkCore;

namespace DB_app.Core.Contracts.Services;


public interface IRepositoryControllerService
{

    IAddressRepository  Addresses  { get; }
    IOrderRepository    Orders     { get; }
    IProductRepository  Products   { get; }
    IPharmacyRepository Pharmacies { get; }
    IHospitalRepository Hospitals  { get; }
    IMedicineRepository Medicines  { get; }

    public void SetupDataBase();
}