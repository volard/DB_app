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

        public PostgresContext(DbContextOptions<PostgresContext> options) : base(options)
        { }

        public DbSet<Hospital> Hospitals    { get; set; }
        public DbSet<Order>    Orders       { get; set; }
        public DbSet<Product>  Products     { get; set; }
        public DbSet<Pharmacy> Pharmacies   { get; set; }
        public DbSet<Address>  Addresses    { get; set; }
        public DbSet<Medicine> Medicines    { get; set; }
    }
}
