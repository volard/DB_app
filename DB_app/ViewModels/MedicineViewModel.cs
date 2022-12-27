using DB_app.Core.Contracts.Services;
using DB_app.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_app.ViewModels
{
    public class MedicineViewModel : IEditableObject
    {
        private readonly IRepositoryControllerService _repositoryControllerService;
        public MedicineViewModel(Medicine medicine) 
        {
            _repositoryControllerService = App.GetService<IRepositoryControllerService>();
            _medicineData = medicine;
        }


        private Medicine _medicineData;

        public Medicine MedicineData
        {
            get => _medicineData;
            set => _medicineData = value;
        }

        public string Name
        {
            get => _medicineData.Name;
        }
        public string Type
        {
            get => _medicineData.Type;
        }


        private Medicine? customizedData;
        private Medicine? backupedData;
        private bool inTxn = false;


        #region IEditable implementation

        public void BeginEdit()
        {
            Debug.WriteLine("Look at me! Im soooo lazy to implement BeginEdit");
        }

        public void CancelEdit()
        {
            Debug.WriteLine("Look at me! Im soooo lazy to implement CancelEdit");
        }

        public void EndEdit()
        {
            Debug.WriteLine("Look at me! Im soooo lazy to implement EndEdit");
        }

        #endregion
    }
}
