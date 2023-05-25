using DB_app.Models;

namespace DB_app.Repository;
public interface IPharmacyRepository
{
    /// <summary>
    /// Deletes specified pharmacy
    /// </summary>
    public Task DeleteAsync(int id);

    /// <summary>
    /// Returns all active pharmacies
    /// </summary>
    public Task<IEnumerable<Pharmacy>> GetAsync();

    /// <summary>
    /// Returns specific pharmacy. 
    /// </summary>
    public Task<Pharmacy> GetAsync(int id);

    public Task<double> GetPharmacyBudget(int pharmacyId);

    /// <summary>
    /// Returns all pharmacies
    /// </summary>
    public Task<IEnumerable<Pharmacy>> GetAllAsync();


    /// <summary>
    /// Returns inactive pharmacies
    /// </summary>
    public Task<IEnumerable<Pharmacy>> GetInactiveAsync();
    
    /// <summary>
    /// Inserts new pharmacy
    /// </summary>
    public Task InsertAsync(Pharmacy pharmacy);

    /// <summary>
    /// Updates existing pharmacy
    /// </summary>
    public Task UpdateAsync(Pharmacy pharmacy);
}
