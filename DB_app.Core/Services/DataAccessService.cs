using DB_app.Core.Contracts.Services;
using Microsoft.EntityFrameworkCore;
using DB_app.Models;

namespace DB_app.Core.Services;


public class DataAccessService : DbContext, IDataAccessService
{
    public DbSet<Address> Addresses { get; set; } = null!;
    public DbSet<Hospital> Hospitals { get; set; } = null!;
    public DbSet<Medicine> Medicines { get; set; } = null!;
    public DbSet<Order> Orders { get; set; } = null!;
    public DbSet<Pharmacy> Pharmacies { get; set; } = null!;
    public DbSet<Product> Products { get; set; } = null!;

    
}
