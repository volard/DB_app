using DB_app.Models;

namespace DB_app.Repository;
public interface IHospitalRepository
{
    /// <summary>
    /// Deletes specified hospital
    /// </summary>
    // TODO implement all this restriction stuff or how is it called
    public Task DeleteAsync(int id);

    /// <summary>
    /// Returns all hospitals. 
    /// </summary>
    Task<IEnumerable<Hospital>> GetAsync();


    /// <summary>
    /// Returns specific hospital. 
    /// </summary>
    public Task<Hospital> GetAsync(int id);

    /// <summary>
    /// Inserts new hospital
    /// </summary>
    public Task InsertAsync(Hospital hospital);

    /// <summary>
    /// Updates existing hospital
    /// </summary>
    public Task UpdateAsync(Hospital hospital);
}
