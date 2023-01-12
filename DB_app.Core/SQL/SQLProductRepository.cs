using DB_app.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        if (null != foundProduct)
        {
            _db.Products.Remove(foundProduct);
            await _db.SaveChangesAsync();
            Debug.WriteLine("DeleteAsync - Product : " + foundProduct + "was succesfully deleted from the Database");
        }
        else
        {
            Debug.WriteLine("DeleteAsync - Product : No product under specified id was found in the Database");
        }
    }

    public async Task<IEnumerable<Product>> GetAsync()
    {
        return await _db.Products
                .Include(product => product.SellingPharmacy)
                .Include(product => product.ContainingMedicine)
                .ToListAsync();
    }

    public async Task<Product> GetAsync(int id)
    {
        return await _db.Products
                .Include(product => product.SellingPharmacy)
                .Include(product => product.ContainingMedicine)
                .FirstOrDefaultAsync(product => product.Id == id);
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

    public async Task UpdateAsync(Product product)
    {
        Product foundProduct = await _db.Products
               .FirstOrDefaultAsync(existProduct => existProduct.Id == product.Id);

        if (foundProduct != null)
        {
            _db.Entry(foundProduct).CurrentValues.SetValues(product);
            await _db.SaveChangesAsync();
            Debug.WriteLine("UpdateAsync - Product : " + foundProduct.Id + " was succesfully updated in the Database");
        }
        else
        {
            Debug.WriteLine("UpdateAsync - Product : attempt to update Product failed - no Product found to update");
        }
    }
}
