using DB_app.Models;

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

        /// <summary>
        /// Get all hospitals containing provided medicine
        /// </summary>
        public Task<IEnumerable<Hospital>> GetHospitalsContaining(Medicine medicine);

        /// <summary>
        /// Get all pharmacies selling provided medicine
        /// </summary>
        public Task<IEnumerable<Pharmacy>> GetPharmaciesContaining(Medicine medicine);

        /// <summary>
        /// Get only medicine by unique name
        /// </summary>
        public Task<IEnumerable<Medicine>> GetUnique();

        public Task<IEnumerable<string>> GetTypes();
    }
}
