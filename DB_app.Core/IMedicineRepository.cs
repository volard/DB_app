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
        /// Inserts new medicine
        /// </summary>
        public void InsertAsync(Medicine medicine);

        /// <summary>
        /// Updates existing medicine
        /// </summary>
        public void UpdateAsync(Medicine medicine);
    }
}
