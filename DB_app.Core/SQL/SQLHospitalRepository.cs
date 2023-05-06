using DB_app.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace DB_app.Repository.SQL;

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


    public async Task<IEnumerable<Hospital>> GetAsync()
    {
        return await _db.Hospitals
            .Include(hospital => hospital.Locations)
            .Where(hospital => hospital.IsActive)
            .ToListAsync();
    }


    public async Task<Hospital> GetAsync(int id)
    {
        return await _db.Hospitals
           .Include(hospital => hospital.Locations)
           .FirstOrDefaultAsync(hospital => hospital.Id == id);
    }

    public async Task InsertAsync(Hospital hospital)
    {
        Hospital foundHospital = await _db.Hospitals
               .FirstOrDefaultAsync(existHospital => existHospital.Id == hospital.Id);

        if (foundHospital != null)
        {
            throw new RecordAlreadyExistsException();
        }

        if (hospital.Locations == null || hospital.Locations.Count == 0)
        {
            hospital.IsActive = false;
        }

        _db.Hospitals.Add(hospital);
        await _db.SaveChangesAsync();
    }



    public async Task UpdateAsync(Hospital hospital)
    {
        Hospital foundHospital = await _db.Hospitals
                .FirstOrDefaultAsync(existHospital => existHospital.Id == hospital.Id);


        if (foundHospital == null)
        {
            Debug.WriteLine("UpdateAsync - Hospital : attempt to update hospital failed - no hospital found to update");
            return;
        }

        _db.Entry(foundHospital).CurrentValues.SetValues(hospital);

        await _db.SaveChangesAsync();
        Debug.WriteLine("UpdateAsync - Hospital : " + foundHospital.Id + " was succesfully updated in the Database");
    }


    public async Task DeleteAsync(int id)
    {
        var foundHospital = await _db.Hospitals.FirstOrDefaultAsync(_hospital => _hospital.Id == id);
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

    public async Task<IEnumerable<Hospital>> GetAllAsync()
    {
        return await _db.Hospitals
            .Include(hospital => hospital.Locations)
            .ToListAsync();
    }

    public async Task<IEnumerable<Hospital>> GetInactiveAsync()
    {
        return await _db.Hospitals
            .Where(hospital => !hospital.IsActive)
            .ToListAsync();
    }
}
