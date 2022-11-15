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
    /// Returns all addresses. 
    /// </summary>
    Task<IEnumerable<Address>> GetAsync();
}
