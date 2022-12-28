using CommunityToolkit.Mvvm.ComponentModel;
using DB_app.Core.Contracts.Services;
using DB_app.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_app.ViewModels.ObjectWrappers
{
    /// <summary>
    /// Provides wrapper for the Order model class, encapsulating various services for access by the UI.
    /// </summary>
    public class MedicineWrapper : ObservableObject, IEditableObject
    {
        private readonly IRepositoryControllerService _repositoryControllerService 
            = App.GetService<IRepositoryControllerService>();

        public MedicineWrapper(Medicine medicine)
        {
            _medicineData = medicine;
        }

        public MedicineWrapper()
        {
            _medicineData = new Medicine();
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


        //private Medicine? customizedData;
        //private Medicine? backupedData;

        private bool isModified = false;

        public bool IsModified
        {
            get => isModified;
            set => isModified = value;
        }


        #region IEditable implementation

        public void BeginEdit()
        {
            Debug.WriteLine("Look at me! Im soooo lazy to implement BeginEdit");
            isModified= true;
        }

        public void CancelEdit()
        {
            Debug.WriteLine("Look at me! Im soooo lazy to implement CancelEdit");
            isModified= false;
        }

        public void EndEdit()
        {
            Debug.WriteLine("Look at me! Im soooo lazy to implement EndEdit");
        }

        #endregion
    }
}
