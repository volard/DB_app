using DB_app.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_app.Repository.PosgresMain;

/// <summary>
/// Contains methods for interacting with the hospitals backend using 
/// SQL via Entity Framework Core.
/// </summary>
public class SQLHospitalRepository : IHospitalRepository
{

    private readonly SQLContext _db;

    public SQLHospitalRepository(SQLContext db)
    {
        _db = db;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Hospital>> GetAsync()
    {
        return await _db.Hospitals.ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<Hospital> GetAsync(int id)
    {
        return await _db.Hospitals
           .FirstOrDefaultAsync(hospital => hospital.id_hospital == id);
    }

    /// <inheritdoc/>
    public async Task InsertAsync(Hospital hospital)
    {
        _db.Hospitals.Add(hospital);
        await _db.SaveChangesAsync();
        Debug.WriteLine("InsertAsync - Hospital : " + hospital.id_hospital + "was succesfully inserted in the Database");
    }

    /// <inheritdoc/>
    public async Task UpdateAsync(Hospital hospital)
    {
        Hospital foundHospital = await _db.Hospitals
                .FirstOrDefaultAsync(existHospital => existHospital.id_hospital == hospital.id_hospital);

        if (foundHospital != null)
        {
            _db.Entry(foundHospital).CurrentValues.SetValues(hospital);
            await _db.SaveChangesAsync();
            Debug.WriteLine("UpdateAsync - Hospital : " + foundHospital.id_hospital + " was succesfully updated in the Database");
        }
        else
        {
            Debug.WriteLine("UpdateAsync - Hospital : attempt to update hospital failed - no hospital found to update");
        }
        
    }

    /// <inheritdoc/>
    public async Task DeleteAsync(int id)
    {
        var foundHospital = await _db.Hospitals.FirstOrDefaultAsync(_hospital => _hospital.id_hospital == id);
        if (null != foundHospital)
        {
            _db.Hospitals.Remove(foundHospital);
            await _db.SaveChangesAsync();
            Debug.WriteLine("DeleteAsync - Hospital : " + foundHospital + "was succesfully deleted from the Database");
        }
        else
        {
            Debug.WriteLine("DeleteAsync - Hospital : No hospital under specified id was found in the Database");
        }
    }
}
