using DB_app.Models;

namespace DB_app.Repository;
public interface IPharmacyRepository
{
    /// <summary>
    /// Deletes specified pharmacy
    /// </summary>
    public Task DeleteAsync(int id);

    /// <summary>
    /// Returns all pharmacies
    /// </summary>
    Task<IEnumerable<Pharmacy>> GetAsync();

    /// <summary>
    /// Returns specific pharmacy. 
    /// </summary>
    public Task<Pharmacy> GetAsync(int id);

    /// <summary>
    /// Inserts new pharmacy
    /// </summary>
    public Task InsertAsync(Pharmacy pharmacy);

    /// <summary>
    /// Updates existing pharmacy
    /// </summary>
    public Task UpdateAsync(Pharmacy pharmacy);
}
