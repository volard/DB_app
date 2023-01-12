using DB_app.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_app.Repository.SQL;

/// <summary>
/// Contains methods for interacting with the Pharmacies backend using 
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
        return await _db.Pharmacies.ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<Pharmacy> GetAsync(int id)
    {
        return await _db.Pharmacies
           .FirstOrDefaultAsync(Pharmacy => Pharmacy.Id == id);
    }

    /// <inheritdoc/>
    public async Task InsertAsync(Pharmacy pharmacy)
    {
        // TODO if already exist check
        _db.Pharmacies.Add(pharmacy);
        await _db.SaveChangesAsync();
        Debug.WriteLine("InsertAsync - Pharmacy : " + pharmacy.Id + "was succesfully inserted in the Database");
    }

    /// <inheritdoc/>
    public async Task UpdateAsync(Pharmacy pharmacy)
    {
        Pharmacy foundPharmacy = await _db.Pharmacies
                .FirstOrDefaultAsync(existPharmacy => existPharmacy.Id == pharmacy.Id);

        if (foundPharmacy != null)
        {
            _db.Entry(foundPharmacy).CurrentValues.SetValues(pharmacy);
            await _db.SaveChangesAsync();
            Debug.WriteLine("UpdateAsync - Pharmacy : " + foundPharmacy.Id + " was succesfully updated in the Database");
        }
        else
        {
            Debug.WriteLine("UpdateAsync - Pharmacy : attempt to update Pharmacy failed - no Pharmacy found to update");
        }

    }

    /// <inheritdoc/>
    public async Task DeleteAsync(int id)
    {
        var foundPharmacy = await _db.Pharmacies.FirstOrDefaultAsync(_Pharmacy => _Pharmacy.Id == id);
        if (null != foundPharmacy)
        {
            _db.Pharmacies.Remove(foundPharmacy);
            await _db.SaveChangesAsync();
            Debug.WriteLine("DeleteAsync - Pharmacy : " + foundPharmacy + "was succesfully deleted from the Database");
        }
        else
        {
            Debug.WriteLine("DeleteAsync - Pharmacy : No Pharmacy under specified id was found in the Database");
        }
    }
}
