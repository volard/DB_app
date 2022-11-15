using DB_app.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_app.Repository
{
    public interface IMedicineRepository
    {
        /// <summary>
        /// Returns all medicines. 
        /// </summary>
        Task<IEnumerable<Medicine>> GetAsync();
    }
}
