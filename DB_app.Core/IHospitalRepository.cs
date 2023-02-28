using DB_app.Entities;

namespace DB_app.Repository;
public interface IHospitalRepository
{
    /// <summary>
    /// Deletes specified hospital
    /// </summary>
    public Task DeleteAsync(int id);

    /// <summary>
    /// Returns all active hospitals. 
    /// </summary>
    public Task<IEnumerable<Hospital>> GetAsync();

    /// <summary>
    /// Returns specific hospital. 
    /// </summary>
    public Task<Hospital> GetAsync(int id);

    /// <summary>
    /// Returns all hospitals. 
    /// </summary>
    public Task<IEnumerable<Hospital>> GetAllAsync();


    /// <summary>
    /// Returns all inactive hospitals. 
    /// </summary>
    public Task<IEnumerable<Hospital>> GetInactiveAsync();

    /// <summary>
    /// Inserts new hospital
    /// </summary>
    public Task InsertAsync(Hospital hospital);

    /// <summary>
    /// Updates existing hospital
    /// </summary>
    public Task UpdateAsync(Hospital hospital);
}
