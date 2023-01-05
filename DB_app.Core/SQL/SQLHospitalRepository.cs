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
    public class SQLHospitalRepository : IHospitalRepository
    {

        private readonly SQLContext _db;

        public SQLHospitalRepository(SQLContext db)
        {
            _db = db;
        }
    }
}
