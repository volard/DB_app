using DB_app.Models;
using Microsoft.EntityFrameworkCore;

namespace DB_app.Repository.SQL;

/// <summary>
/// Main database context to link models and actual database
/// </summary>
public class SQLContext : DbContext
{

    public SQLContext(DbContextOptions<SQLContext> options) : base(options)
    { }

    public DbSet<Hospital>  Hospitals    { get; set; }
    public DbSet<Order>     Orders       { get; set; }
    public DbSet<Product>   Products     { get; set; }
    public DbSet<Pharmacy>  Pharmacies   { get; set; }
    public DbSet<Address>   Addresses    { get; set; }
    public DbSet<Medicine>  Medicines    { get; set; }
    public DbSet<OrderItem> OrderItems   { get; set; }
}
