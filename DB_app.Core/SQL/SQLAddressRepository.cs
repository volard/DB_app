﻿using DB_app.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;

namespace DB_app.Repository.SQL;

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
    public async Task<IEnumerable<Address>> GetFreeAddressesAsync()
    {
        var hospitalAddresses = await _db.HospitalLocations.Select(x => x.Address).ToListAsync();
        var pharmacyAddresses = await _db.PharmacyLocations.Select(x => x.Address).ToListAsync();

        var bindedAddresses = new List<Address>();

        bindedAddresses.AddRange(hospitalAddresses);
        bindedAddresses.AddRange(pharmacyAddresses);

        var output = await _db.Addresses.ToListAsync();

        return output.Except(bindedAddresses);
    }

    
    public async Task<IEnumerable<Address>> GetAsync()
    {
        return await _db.Addresses.ToListAsync();
    }


    /// <inheritdoc/>
    public async Task<IEnumerable<Address>> GetHospitalsAddressesAsync()
    {
        var locations = await _db.HospitalLocations.Include(l => l.Address).ToListAsync();
        IEnumerable<Address> output = locations.Select(l => l.Address);
        return output;
    }


    /// <inheritdoc/>
    public async Task<Address> GetAsync(int id)
    {
        return await _db.Addresses
           .FirstOrDefaultAsync(address => address.Id == id);
    }


    /// <inheritdoc/>
    public async Task InsertAsync(Address address)
    {
        _db.Addresses.Add(address);
        await _db.SaveChangesAsync();
        Debug.WriteLine("InsertAsync - Address : " + address.Id + "was succesfully inserted in the Database");
    }


    /// <inheritdoc/>
    public async Task UpdateAsync(Address address)
    {
        Address foundAddress = await _db.Addresses
                .FirstOrDefaultAsync(existAddress => existAddress.Id == address.Id);

        if (foundAddress != null)
        {
            _db.Entry(foundAddress).CurrentValues.SetValues(address);
            await _db.SaveChangesAsync();
            Debug.WriteLine("UpdateAsync - Address : " + foundAddress.Id + "was succesfully updated in the Database");
        }
    }


    /// <inheritdoc/>
    /// <exception cref="RecordNotFoundException">
    /// Thrown if attempt to delete non-existent record was made
    /// </exception>  
    public async Task DeleteAsync(int id)
    {
        var foundAddress = await _db.Addresses.FirstOrDefaultAsync(_address => _address.Id == id);
        if (foundAddress != null)
        {
            var isLinkedToHospital = await _db.HospitalLocations.AnyAsync(_location => _location.Address == foundAddress);
            if (isLinkedToHospital) { throw new LinkedRecordOperationException(); }

            var isLinkedToPharmacy = await _db.PharmacyLocations.AnyAsync(_location => _location.Address == foundAddress);
            if (isLinkedToPharmacy) { throw new LinkedRecordOperationException(); }

            _db.Addresses.Remove(foundAddress);
            await _db.SaveChangesAsync();
        }
        else
        {
            throw new RecordNotFoundException();
        }
    }
}
