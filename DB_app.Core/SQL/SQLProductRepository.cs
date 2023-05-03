using DB_app.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace DB_app.Repository.SQL;

/// <summary>
/// Contains methods for interacting with the products backend using 
/// SQL via Entity Framework Core.
/// </summary>
public class SQLProductRepository : IProductRepository
{
    private readonly SQLContext _db;

    public SQLProductRepository(SQLContext db)
    {
        _db = db;
    }

    public async Task DeleteAsync(int id)
    {
        var foundProduct = await _db.Products.FirstOrDefaultAsync(_Product => _Product.Id == id);
        if (foundProduct != null)
        {


            _db.Products.Remove(foundProduct);
            await _db.SaveChangesAsync();
        }
        else
        {
            throw new RecordNotFoundException();
        }
    }

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await _db.Products
                .Include(product => product.Pharmacy)
                .Include(product => product.Medicine)
                .ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetAsync()
    {
        return await _db.Products
                .Include(product => product.Pharmacy)
                .Include(product => product.Medicine)
                .Where(product => product.Quantity > 0)
                .ToListAsync();
    }


    public async Task<Product> GetAsync(int id)
    {
        return await _db.Products
                .Include(product => product.Pharmacy)
                .Include(product => product.Medicine)
                .FirstOrDefaultAsync(product => product.Id == id);
    }

    public async Task<IEnumerable<Product>> GetOutOfStockAsync()
    {
        return await _db.Products
                .Include(product => product.Pharmacy)
                .Include(product => product.Medicine)
                .Where(product => product.Quantity == 0)
                .ToListAsync();
    }

    public async Task InsertAsync(Product product)
    {
        Product foundProduct = await _db.Products
               .FirstOrDefaultAsync(existProduct => existProduct == product);


        if (foundProduct != null)
        {
            Debug.WriteLine("UpdateAsync - Product : insertion failed, product already exists");
        }
        else
        {
            _db.Products.Add(product);
            await _db.SaveChangesAsync();
            Debug.WriteLine("InsertAsync - Product : " + product.Id + "was succesfully inserted in the Database");
        }
    }

    /// <summary>
    /// Returns all products from pharmacy
    /// </summary>
    public async Task<IEnumerable<Product>> GetFromPharmacy(int id)
    {
        return await _db.Products
                .Include(product => product.Pharmacy)
                .Include(product => product.Medicine)
                .Where(product => product.Pharmacy.Id == id)
                .ToListAsync();
    }

    public async Task UpdateAsync(Product product)
    {
        Product foundProduct = await _db.Products
               .FirstOrDefaultAsync(existProduct => existProduct.Id == product.Id);

        if (foundProduct != null)
        {
            _db.Entry(foundProduct).CurrentValues.SetValues(product);
            await _db.SaveChangesAsync();
        }
        else
        {
            throw new RecordNotFoundException();
        }
    }
}
