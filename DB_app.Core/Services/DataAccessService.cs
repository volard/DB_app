﻿using DB_app.Core.Contracts.Services;
using Microsoft.EntityFrameworkCore;
using DB_app.Models;
using DB_app.Repository;

namespace DB_app.Core.Services;

/// <summary>
/// Entity Framework Core DbContext for Contoso.
/// </summary>
public class DataAccessService : DbContext, IDataAccessService
{
    /// <summary>
    /// Creates a new Main DbContext.
    /// </summary>
    public DataAccessService(DbContextOptions<DataAccessService> options) : base(options)
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
