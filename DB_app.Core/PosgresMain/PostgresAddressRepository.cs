using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DB_app.Models;
using Microsoft.EntityFrameworkCore;

namespace DB_app.Repository.PosgresMain
{
    /// <summary>
    /// Contains methods for interacting with the addresses backend using 
    /// SQL via Entity Framework Core.
    /// </summary>
    public class PostgresAddressRepository : IAddressRepository
    {
        private readonly PostgresContext _db;

        public PostgresAddressRepository(PostgresContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Address>> GetAsync()
        {
            return await _db.Addresses.AsNoTracking().ToListAsync();
        }
    }
}
