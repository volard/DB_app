using CommunityToolkit.Mvvm.ComponentModel;
using DB_app.Core.Contracts.Services;
using DB_app.Models;
using DB_app.ViewModels.ObjectWrappers;
using Microsoft.UI.Xaml;
using Microsoft.Windows.AppNotifications;
using Microsoft.Windows.AppNotifications.Builder;

namespace DB_app.ViewModels;

public partial class MedicineDetailsViewModel : ObservableObject
{
    private readonly IRepositoryControllerService _repositoryControllerService
         = App.GetService<IRepositoryControllerService>();


    private bool _isEditDisabled;

    public bool IsEditDisabled
    {
        get => _isEditDisabled;
        set => _isEditDisabled = value;
    }

    private bool _isEditEnabled;

    public bool IsEditEnabled
    {
        get => _isEditEnabled;
        set => _isEditEnabled = value;
    }


    public MedicineDetailsViewModel()
    {
        IsEditDisabled = true;
        IsEditEnabled = false;
        type = "";
        name = "";
    }

    public void StartEdit_Click(object sender, RoutedEventArgs e)
    {
        IsEditDisabled = false;
        IsEditEnabled = true;
    }

    private MedicineWrapper? _currentMedicine;

    public MedicineWrapper? CurrentMedicine
    {
        get => _currentMedicine;
        set
        {
            _currentMedicine = value;
            if (_currentMedicine != null)
            {
                _isEditEnabled = true;
                _isEditDisabled = false;
                Name = _currentMedicine.MedicineData.Name;
                Type = _currentMedicine.MedicineData.Type;
            }
            else
            {
                _isEditEnabled = false;
                _isEditDisabled = true;
                Name = "";
                Type = "";
            }

        }
    }

    [ObservableProperty]
    public string name;

    [ObservableProperty]
    public string type;



    /// <summary>
    /// Saves customer data that has been edited.
    /// </summary>
    public void SaveAsync(object sender, RoutedEventArgs e)
    {

        Medicine newMedicine = new()
        {
            Name = this.name,
            Type = this.type
        };
        var builder = new AppNotificationBuilder();

        if (IsEditDisabled)
        {
            _repositoryControllerService.Medicines.InsertAsync(newMedicine);

            builder
                .SetAppLogoOverride(new Uri("ms-appx:///images/reminder.png"), AppNotificationImageCrop.Circle)
                .AddArgument("conversationId", "9813")
                .AddText($"Medicine under name '{this.name}' and type '{this.type}' was created")
                .SetAudioUri(new Uri("ms-appx:///Sound.mp3"));
        }
        else
        {
            _repositoryControllerService.Medicines.UpdateAsync(newMedicine);
            
            

            builder
                .SetAppLogoOverride(new Uri("ms-appx:///images/reminder.png"), AppNotificationImageCrop.Circle)
                .AddArgument("conversationId", "9813")
                .AddText($"Medicine under name '{this.name}' and type '{this.type}' was updated")
                .SetAudioUri(new Uri("ms-appx:///Sound.mp3"));
        }
        

        AppNotificationManager.Default.Show(builder.BuildNotification());
    }
}

