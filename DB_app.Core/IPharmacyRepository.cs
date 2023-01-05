using DB_app.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    /// Inserts new pharmacy
    /// </summary>
    public Task InsertAsync(Pharmacy medicine);

    /// <summary>
    /// Updates existing pharmacy
    /// </summary>
    public Task UpdateAsync(Pharmacy medicine);
}
