using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DB_app.Models;
using Microsoft.EntityFrameworkCore;

namespace DB_app.Repository.PosgresMain;

/// <summary>
/// Contains methods for interacting with the addresses backend using 
/// SQL via Entity Framework Core.
/// </summary>
public class SQLAddressRepository : IAddressRepository
{
    private readonly SQLContext _db;

    public SQLAddressRepository(SQLContext db)
    {
        _db = db;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Address>> GetAsync()
    {
        return await _db.Addresses.ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<Address> GetAsync(int id)
    {
        return await _db.Addresses
           .FirstOrDefaultAsync(address => address.id_address == id);
    }

    /// <inheritdoc/>
    public async Task InsertAsync(Address address)
    {
        _db.Addresses.Add(address);
        await _db.SaveChangesAsync();
        Debug.WriteLine("InsertAsync - Address : " + address.id_address + "was succesfully inserted in the Database");
    }

    /// <inheritdoc/>
    public async Task UpdateAsync(Address address)
    {
        Address foundAddress = await _db.Addresses
                .FirstOrDefaultAsync(existAddress => existAddress.id_address == address.id_address);

        if (foundAddress != null)
        {
            _db.Entry(foundAddress).CurrentValues.SetValues(address);
            await _db.SaveChangesAsync();
            Debug.WriteLine("UpdateAsync - Address : " + foundAddress.id_address + "was succesfully updated in the Database");
        }
    }

    /// <inheritdoc/>
    public async Task DeleteAsync(int id)
    {
        var foundAddress = await _db.Addresses.FirstOrDefaultAsync(_address => _address.id_address == id);
        if (null != foundAddress)
        {
            _db.Addresses.Remove(foundAddress);
            await _db.SaveChangesAsync();
            Debug.WriteLine("DeleteAsync - Address : " + foundAddress + "was succesfully deleted from the Database");
        }
        else
        {
            Debug.WriteLine("DeleteAsync - Address : No address under specified id was found in the Database");
        }
    }
}
