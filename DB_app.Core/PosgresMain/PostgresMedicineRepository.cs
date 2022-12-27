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

        public async Task<IEnumerable<Medicine>> UpsertAsync()
        {
            return await _db.Medicines.AsNoTracking().ToListAsync();
        }

        public async void upsertAsync(Medicine medicine)
        {
            _db.Medicines.Add(medicine);
            await _db.SaveChangesAsync();

        }

        public async Task DeleteAsync(int id)
        {
            var foundMedicine = await _db.Medicines.FirstOrDefaultAsync(_medicine => _medicine.id_medicine == id);
            if (null != foundMedicine)
            {
                _db.Medicines.Remove(foundMedicine);
                await _db.SaveChangesAsync();
            }
        }
    }
}
