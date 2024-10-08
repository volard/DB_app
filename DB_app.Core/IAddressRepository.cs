﻿using DB_app.Models;

namespace DB_app.Repository;
public interface IAddressRepository
{
    /// <summary>
    /// Deletes specified address
    /// </summary>
    /// <param name="id">Address' id to delete</param>
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

    /// <summary>
    /// Get all addresses available for binding
    /// </summary>
    public Task<IEnumerable<Address>> GetFreeAddressesAsync();
}
