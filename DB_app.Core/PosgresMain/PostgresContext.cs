using DB_app.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_app.Repository.PosgresMain
{
    /// <summary>
    /// Main database context to link models and actual database
    /// </summary>
    public class PostgresContext : DbContext
    {

        /// <summary>
        /// Creates a new Main Postgres DbContext.
        /// </summary>
        public PostgresContext(DbContextOptions<PostgresContext> options) : base(options)
        { }

        /// <summary>
        /// Gets the hospital DbSet.
        /// </summary>
        public DbSet<Hospital> Hospitals { get; set; }

        /// <summary>
        /// Gets the orders DbSet.
        /// </summary>
        public DbSet<Order> Orders { get; set; }

        /// <summary>
        /// Gets the products DbSet.
        /// </summary>
        public DbSet<Product> Products { get; set; }

        /// <summary>
        /// Gets the Pharmacy items DbSet.
        /// </summary>
        public DbSet<Pharmacy> Pharmacies { get; set; }

        /// <summary>
        /// Gets the Address items DbSet.
        /// </summary>
        public DbSet<Address> Addresses { get; set; }

        /// <summary>
        /// Gets the products DbSet.
        /// </summary>
        public DbSet<Medicine> Medicines { get; set; }
    }
}
