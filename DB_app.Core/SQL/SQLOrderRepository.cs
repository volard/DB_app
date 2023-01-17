﻿using DB_app.Entities;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace DB_app.Repository.SQL;

/// <summary>
/// Contains methods for interacting with the orders backend using 
/// SQL via Entity Framework Core.
/// </summary>
public class SQLOrderRepository : IOrderRepository
{
    private readonly SQLContext _db;

    public SQLOrderRepository(SQLContext db)
    {
        _db = db;
    }

    public async Task DeleteAsync(int id)
    {
        var foundOrder = await _db.Orders.FirstOrDefaultAsync(_Order => _Order.Id == id);
        if (null != foundOrder)
        {
            _db.Orders.Remove(foundOrder);
            await _db.SaveChangesAsync();
            Debug.WriteLine("DeleteAsync - Order : " + foundOrder + "was succesfully deleted from the Database");
        }
        else
        {
            Debug.WriteLine("DeleteAsync - Order : No product under specified id was found in the Database");
        }
    }

    public async Task<IEnumerable<Order>> GetAsync()
    {
        return await _db.Orders
                .Include(order => order.HospitalCustomer)
                .Include(order => order.ShippingAddress)
                .ToListAsync();
    }

    public async Task<Order> GetAsync(int id)
    {
        return await _db.Orders
                .Include(order => order.HospitalCustomer)
                .Include(order => order.ShippingAddress)
                .FirstOrDefaultAsync(order => order.Id == id);
    }

    public async Task InsertAsync(Order order)
    {
        Order foundOrder = await _db.Orders
               .FirstOrDefaultAsync(existOrder => existOrder == order);

        if (foundOrder != null)
        {
            Debug.WriteLine("UpdateAsync - Order : insertion failed, order already exists");
        }
        else
        {
            _db.Orders.Add(order);
            await _db.SaveChangesAsync();
            Debug.WriteLine("InsertAsync - Order : " + order.Id + "was succesfully inserted in the Database");
        }
    }

    public async Task UpdateAsync(Order order)
    {
        Order foundOrder = await _db.Orders
               .FirstOrDefaultAsync(existOrder => existOrder.Id == order.Id);

        if (foundOrder != null)
        {
            _db.Entry(foundOrder).CurrentValues.SetValues(order);
            await _db.SaveChangesAsync();
            Debug.WriteLine("UpdateAsync - Order : " + foundOrder.Id + " was succesfully updated in the Database");
        }
        else
        {
            Debug.WriteLine("UpdateAsync - Order : attempt to update Order failed - no Order found to update");
        }
    }
}
