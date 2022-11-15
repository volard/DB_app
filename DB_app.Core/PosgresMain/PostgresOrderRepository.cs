using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_app.Repository.PosgresMain
{
    /// <summary>
    /// Contains methods for interacting with the orders backend using 
    /// SQL via Entity Framework Core.
    /// </summary>
    public class PostgresOrderRepository : IOrderRepository
    {
        private readonly PostgresContext _db;

        public PostgresOrderRepository(PostgresContext db)
        {
            _db = db;
        }
    }
}
