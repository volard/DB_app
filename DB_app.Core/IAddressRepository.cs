using DB_app.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_app.Repository;
public interface IAddressRepository
{
    /// <summary>
    /// Deletes specified address
    /// </summary>
    // TODO implement all this restriction stuff or how is it called
    public Task DeleteAsync(int id);

    /// <summary>
    /// Returns all addresses. 
    /// </summary>
    Task<IEnumerable<Address>> GetAsync();

    /// <summary>
    /// Inserts new address
    /// </summary>
    public Task InsertAsync(Address address);

    /// <summary>
    /// Updates existing address
    /// </summary>
    public Task UpdateAsync(Address address);
}
