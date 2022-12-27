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
        /// Deletes specified medicine
        /// </summary>
        public Task DeleteAsync(int id);

        /// <summary>
        /// Returns all medicines. 
        /// </summary>
        Task<IEnumerable<Medicine>> GetAsync();

        /// <summary>
        /// Upserts new medicine
        /// </summary>
        public void upsertAsync(Medicine medicine);
    }
}
