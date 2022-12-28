using CommunityToolkit.Mvvm.ComponentModel;
using DB_app.Core.Contracts.Services;
using DB_app.Models;
using Microsoft.Windows.AppNotifications;
using Microsoft.Windows.AppNotifications.Builder;
using System.ComponentModel;
using System.Diagnostics;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace DB_app.ViewModels.ObjectWrappers
{
    /// <summary>
    /// Provides wrapper for the Order model class, encapsulating various services for access by the UI.
    /// </summary>
    public class MedicineWrapper : ObservableObject, IEditableObject, IEquatable<MedicineWrapper>
    {
        private readonly IRepositoryControllerService _repositoryControllerService
            = App.GetService<IRepositoryControllerService>();

        public MedicineWrapper(Medicine medicine)
        {
            _medicineData = medicine;
        }

        public override string ToString() => $"MedicineWrapper with MedicineData '{Name}' under '{Type}' type";

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
            set => _medicineData.Name = value;
        }
        public string Type
        {
            get => _medicineData.Type;
            set => _medicineData.Type = value;
        }

        public int Id
        {
            get => _medicineData.id_medicine;
        }


        //private Medicine? customizedData;
        //private Medicine? backupedData;
        private struct backupedData
        {
            public string Name;
            public string Type;
        }

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
            isModified = true;
            Debug.WriteLine($"For now the name of editable medicineWrapper is {this.Name}");
        }

        public void CancelEdit()
        {
            Debug.WriteLine("Look at me! Im soooo lazy to implement CancelEdit");
            isModified = false;
        }

        public async void EndEdit()
        {
            Debug.WriteLine("Look at me! Im soooo lazy to implement EndEdit");
            Debug.WriteLine($"For now the name of editable medicineWrapper is {this.Name}");
            await _repositoryControllerService.Medicines.UpdateAsync(_medicineData);


            // TODO wrap somehow this notifications and implement smth like notification manager
            var builder = new AppNotificationBuilder();
            builder
                .SetAppLogoOverride(new Uri("ms-appx:///images/reminder.png"), AppNotificationImageCrop.Circle)
                .AddArgument("conversationId", "9813")
                .AddText($"Medicine under name '{this.Name}' and type '{this.Type}' was saved")
                .AddButton(new AppNotificationButton("Cancel")
                    .AddArgument("action", "OpenApp")
                )
                .SetAudioUri(new Uri("ms-appx:///Sound.mp3"));

            AppNotificationManager.Default.Show(builder.BuildNotification());
        }

        public bool Equals(MedicineWrapper? other) =>
            Name == other?.Name &&
            Type == other?.Type;

        #endregion
    }
}
