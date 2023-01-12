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
/// Contains methods for interacting with the medicines backend using 
/// SQL via Entity Framework Core.
/// </summary>
public class SQLMedicineRepository : IMedicineRepository
{
    private readonly SQLContext _db;

    public SQLMedicineRepository(SQLContext db)
    {
        _db = db;
    }



    /// <inheritdoc/>
    public async Task<IEnumerable<Medicine>> GetAsync()
    {
        return await _db.Medicines.ToListAsync();
    }



    /// <inheritdoc/>
    public async Task<Medicine> GetAsync(int id)
    {
        return await _db.Medicines
           .FirstOrDefaultAsync(medicine => medicine.Id == id);
    }



    /// <inheritdoc/>
    public async Task InsertAsync(Medicine medicine)
    {
        _db.Medicines.Add(medicine);
        await _db.SaveChangesAsync();
        Debug.WriteLine("InsertAsync - Medicine : " + medicine.Name + "was succesfully inserted in the Database");
    }



    /// <inheritdoc/>
    public async Task UpdateAsync(Medicine medicine)
    {
        Medicine foundMedicine = await _db.Medicines
                .FirstOrDefaultAsync(existMedicine => existMedicine.Id == medicine.Id);

        if (foundMedicine != null)
        {
            _db.Entry(foundMedicine).CurrentValues.SetValues(medicine);
            await _db.SaveChangesAsync();
            Debug.WriteLine("UpdateAsync - Medicine : " + foundMedicine.Name + "was succesfully updated in the Database");
        }
    }



    /// <inheritdoc/>
    public async Task DeleteAsync(int id)
    {
        var foundMedicine = await _db.Medicines.FirstOrDefaultAsync(_medicine => _medicine.Id == id);
        if (null != foundMedicine)
        {
            _db.Medicines.Remove(foundMedicine);
            await _db.SaveChangesAsync();
            Debug.WriteLine("DeleteAsync - Medicine : " + foundMedicine.Name + "was succesfully deleted from the Database");
        }
        else
        {
            Debug.WriteLine("DeleteAsync - Medicine : No medicine under specified id was found in the Database");
        }
    }
}
