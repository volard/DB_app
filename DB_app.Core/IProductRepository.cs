using DB_app.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_app.Repository;
public interface IProductRepository
{
    /// <summary>
    /// Deletes specified product
    /// </summary>
    public Task DeleteAsync(int id);

    /// <summary>
    /// Returns all out of stock products.
    /// </summary>
    public Task<IEnumerable<Product>> GetOutOfStockAsync();

    /// <summary>
    /// Returns all products from pharmacy
    /// </summary>
    public Task<IEnumerable<Product>> GetFromPharmacy(int id);

    /// <summary>
    /// Returns all products.
    /// </summary>
    public Task<IEnumerable<Product>> GetAllAsync();

    /// <summary>
    /// Returns all available for purchasing products. 
    /// </summary>
    public Task<IEnumerable<Product>> GetAsync();

    /// <summary>
    /// Returns specific product. 
    /// </summary>
    public Task<Product> GetAsync(int id);

    /// <summary>
    /// Inserts new product
    /// </summary>
    public Task InsertAsync(Product product);

    /// <summary>
    /// Updates existing product
    /// </summary>
    public Task UpdateAsync(Product product);
}
