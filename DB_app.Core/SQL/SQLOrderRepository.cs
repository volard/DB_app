using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
}
