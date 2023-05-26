using DB_app.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;

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

    public async Task<IEnumerable<string>> GetTypes()
    {
        List<Medicine> all = await _db.Medicines.ToListAsync();
        List<string> types = new();
        foreach (Medicine medicine in all.Where(medicine => !types.Contains(medicine.Type)))
        {
            types.Add(medicine.Type);
        }
        return types;
    }


    public async Task<IEnumerable<Hospital>> GetHospitalsContaining(Medicine medicine)
    {
        List<OrderItem> items = await _db.OrderItems.Where(item => item.Product.Medicine.Name == medicine.Name).ToListAsync();
        List<Hospital> output = new();
        foreach (Hospital hospital in items
                     .Select(orderItem => orderItem.RepresentingOrder.HospitalCustomer)
                     .Where(hospital => !output.Contains(hospital)))
        {
            output.Add(hospital);
        }
        return output;
    }

    public async Task<IEnumerable<Pharmacy>> GetPharmaciesContaining(Medicine medicine)
    {
        List<Product> items = await _db.Products.Where(product => product.Medicine.Name == medicine.Name).ToListAsync();
        List<Pharmacy> output = new();
        foreach (Pharmacy pharmacy in items.Select(product => product.Pharmacy)
                     .Where(pharmacy => !output.Contains(pharmacy)))
        {
            output.Add(pharmacy);
        }
        return output;
    }


    public async Task<IEnumerable<Medicine>> GetUnique()
    {
        List<Medicine> all = await _db.Medicines.ToListAsync();
        List<Medicine> unique = new();
        foreach (Medicine item in all.Where(item => !unique.Contains(item)))
        {
            unique.Add(item);
        }
        return unique;
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
