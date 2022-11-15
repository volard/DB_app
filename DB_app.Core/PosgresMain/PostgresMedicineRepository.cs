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
    /// Contains methods for interacting with the medicines backend using 
    /// SQL via Entity Framework Core.
    /// </summary>
    public class PostgresMedicineRepository : IMedicineRepository
    {
        private readonly PostgresContext _db;

        public PostgresMedicineRepository(PostgresContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Medicine>> GetAsync()
        {
            return await _db.Medicines.AsNoTracking().ToListAsync();
        }
    }
}
