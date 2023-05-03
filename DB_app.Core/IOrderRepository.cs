using DB_app.Models;

namespace DB_app.Repository;
public interface IOrderRepository
{
    /// <summary>
    /// Deletes specified order
    /// </summary>
    public Task DeleteAsync(int id);

    /// <summary>
    /// Returns all orders. 
    /// </summary>
    Task<IEnumerable<Order>> GetAsync();

    /// <summary>
    /// Returns specific order. 
    /// </summary>
    public Task<Order> GetAsync(int id);

    /// <summary>
    /// Inserts new order
    /// </summary>
    public Task InsertAsync(Order order);

    /// <summary>
    /// Updates existing order
    /// </summary>
    public Task UpdateAsync(Order order);
}
