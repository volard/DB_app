using DB_app.Entities;

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
        public Task InsertAsync(Medicine medicine);

        /// <summary>
        /// Updates existing medicine
        /// </summary>
        public Task UpdateAsync(Medicine medicine);
    }
}
