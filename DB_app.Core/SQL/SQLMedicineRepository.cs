using DB_app.Models;
using Microsoft.EntityFrameworkCore;

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
        Medicine foundMedicine = await _db.Medicines
                .FirstOrDefaultAsync(existMedicine => existMedicine == medicine);

        if (foundMedicine != null)
        {
            throw new RecordAlreadyExistsException();
        }

        _db.Medicines.Add(medicine);
        await _db.SaveChangesAsync();
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
        }
        else
        {
            throw new RecordNotFoundException();
        }
    }



    /// <inheritdoc/>
    public async Task DeleteAsync(int id)
    {
        var foundMedicine = await _db.Medicines.FirstOrDefaultAsync(_medicine => _medicine.Id == id);
        if (foundMedicine != null)
        {


            _db.Medicines.Remove(foundMedicine);
            await _db.SaveChangesAsync();
        }
        else
        {
            throw new RecordNotFoundException();
        }
    }
}
