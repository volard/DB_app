using DB_app.Entities;
using Microsoft.EntityFrameworkCore;

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


    public async Task<IEnumerable<Pharmacy>> GetAsync()
    {
        return await _db.Pharmacies
            .Include(pharmacy => pharmacy.Locations)
            .Where(pharmacy => pharmacy.IsActive)
            .ToListAsync();
    }



    public async Task<Pharmacy> GetAsync(int id)
    {
        return await _db.Pharmacies
           .Include(pharmacy => pharmacy.Locations)
           .FirstOrDefaultAsync(Pharmacy => Pharmacy.Id == id);
    }



    public async Task InsertAsync(Pharmacy pharmacy)
    {

        Pharmacy foundPharmacy = await _db.Pharmacies
                .FirstOrDefaultAsync(existPharmacy => existPharmacy.Id == pharmacy.Id);

        if (foundPharmacy != null)
        {
            throw new SaveDublicateRecordException();
        }

        if ((pharmacy.Locations == null || pharmacy.Locations.Count == 0) && pharmacy.IsActive)
        {
            throw new ActiveOrganisationMissingLocation();
        }

        _db.Pharmacies.Add(pharmacy);
        await _db.SaveChangesAsync();
    }



    public async Task UpdateAsync(Pharmacy pharmacy)
    {
        Pharmacy foundPharmacy = await _db.Pharmacies
                .FirstOrDefaultAsync(existPharmacy => existPharmacy.Id == pharmacy.Id);



        if (foundPharmacy != null)
        {

            if (!foundPharmacy.IsActive)
            {
                throw new InactiveOrganisationReadonly();
            }

            if ((pharmacy.Locations == null || pharmacy.Locations.Count == 0) && pharmacy.IsActive)
            {
                throw new ActiveOrganisationMissingLocation();
            }

            _db.Entry(foundPharmacy).CurrentValues.SetValues(pharmacy);
            await _db.SaveChangesAsync();
        }
        else
        {
            throw new RecordNotFound();
        }
    }



    public async Task DeleteAsync(int id)
    {
        var foundPharmacy = await _db.Pharmacies.FirstOrDefaultAsync(_Pharmacy => _Pharmacy.Id == id);
        if (foundPharmacy != null)
        {
            // if pharmacy linked to orders, its required to disable pharmacy instead of delete it
            // TODO here is problerm
            //if (_db.Orders.Any(order => order.Items == id))
            //{
            //    throw new RecordLinkedWithOrder();
            //}

            _db.Pharmacies.Remove(foundPharmacy);
            var _data = _db.Products.Where(product => product.Pharmacy.Id == id).ToList();
            foreach (var item in _data)
            {
                _db.Products.Remove(item);
            }
            await _db.SaveChangesAsync();
        }
        else
        {
            throw new RecordNotFound();
        }
    }



    public async Task<IEnumerable<Pharmacy>> GetAllAsync()
    {
        return await _db.Pharmacies
           .ToListAsync();
    }


    public async Task<IEnumerable<Pharmacy>> GetInactiveAsync()
    {
        return await _db.Pharmacies
           .Where(pharmacy => !pharmacy.IsActive)
           .ToListAsync();
    }

}
