using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_app.Repository.PosgresMain
{
    /// <summary>
    /// Contains methods for interacting with the hospitals backend using 
    /// SQL via Entity Framework Core.
    /// </summary>
    public class PostgresHospitalRepository : IHospitalRepository
    {

        private readonly PostgresContext _db;

        public PostgresHospitalRepository(PostgresContext db)
        {
            _db = db;
        }
    }
}
