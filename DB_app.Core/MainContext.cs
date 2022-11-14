using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DB_app.Models;
using Microsoft.EntityFrameworkCore;

namespace DB_app.Repository;

/// <summary>
/// Entity Framework Core DbContext for Contoso.
/// </summary>
internal class MainContext : DbContext
{
    /// <summary>
    /// Creates a new Main DbContext.
    /// </summary>
    public MainContext(DbContextOptions<MainContext> options) : base(options)
    {
    }

    /// <summary>
    /// Gets the hospital DbSet.
    /// </summary>
    public DbSet<Hospital> Hospitals
    {
        get; set;
    }

    /// <summary>
    /// Gets the orders DbSet.
    /// </summary>
    public DbSet<Order> Orders
    {
        get; set;
    }

    /// <summary>
    /// Gets the products DbSet.
    /// </summary>
    public DbSet<Product> Products
    {
        get; set;
    }

    /// <summary>
    /// Gets the Pharmacy items DbSet.
    /// </summary>
    public DbSet<Pharmacy> Pharmacies
    {
        get; set;
    }

    /// <summary>
    /// Gets the Address items DbSet.
    /// </summary>
    public DbSet<Address> Addresses
    {
        get; set;
    }
}
