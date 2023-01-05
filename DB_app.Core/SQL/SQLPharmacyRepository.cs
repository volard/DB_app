using DB_app.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_app.Repository.PosgresMain
{
    /// <summary>
    /// Contains methods for interacting with the pharmacies backend using 
    /// SQL via Entity Framework Core.
    /// </summary>
    public class SQLPharmacyRepository : IPharmacyRepository
    {
        private readonly SQLContext _db;

        public SQLPharmacyRepository(SQLContext db)
        {
            _db = db;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Pharmacy>> GetAsync()
        {
            return await _db.Pharmacies.AsNoTracking().ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<Pharmacy> GetAsync(int id)
        {
            return await _db.Pharmacies
               .AsNoTracking()
               .FirstOrDefaultAsync(pharmacy => pharmacy.id_pharmacy == id);
        }

        /// <inheritdoc/>
        public async Task InsertAsync(Pharmacy pharmacy)
        {
            _db.Pharmacies.Add(pharmacy);
            await _db.SaveChangesAsync();
            Debug.WriteLine("InsertAsync: " + pharmacy.Name + "was succesfully inserted in the Database");
        }

        /// <inheritdoc/>
        public async Task UpdateAsync(Pharmacy pharmacy)
        {
            Pharmacy foundPharmacy = await _db.Pharmacies
                    .FirstOrDefaultAsync(existPharmacy => existPharmacy.id_pharmacy == pharmacy.id_pharmacy);

            if (foundPharmacy != null)
            {
                _db.Entry(foundPharmacy).CurrentValues.SetValues(pharmacy);
                await _db.SaveChangesAsync();
                Debug.WriteLine("UpdateAsync: " + foundPharmacy.Name + "was succesfully updated in the Database");
            }
        }

        /// <inheritdoc/>
        public async Task DeleteAsync(int id)
        {
            var foundPharmacy = await _db.Pharmacies.FirstOrDefaultAsync(_pharmacy => _pharmacy.id_pharmacy == id);
            if (null != foundPharmacy)
            {
                _db.Pharmacies.Remove(foundPharmacy);
                await _db.SaveChangesAsync();
                Debug.WriteLine("DeleteAsync: " + foundPharmacy.Name + "was succesfully deleted from the Database");
            }
            else
            {
                Debug.WriteLine("DeleteAsync: No medicine under specified id was found in the Database");
            }
        }
    }
}
