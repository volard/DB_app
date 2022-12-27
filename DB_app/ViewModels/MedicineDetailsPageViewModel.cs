using CommunityToolkit.Mvvm.ComponentModel;
using DB_app.Contracts.ViewModels;
using DB_app.Core.Contracts.Services;
using DB_app.Models;
using DB_app;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Navigation;
using System.ComponentModel;
using Microsoft.UI.Xaml;
using System.Diagnostics;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Windows.AppNotifications.Builder;
using Microsoft.Windows.AppNotifications;
using System.Reflection.Metadata;

namespace DB_app.ViewModels;

public class MedicineDetailsPageViewModel : ObservableRecipient, INavigationAware
{
    private readonly IRepositoryControllerService _repositoryControllerService;


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


    public MedicineDetailsPageViewModel()
    {
        _repositoryControllerService = App.GetService<IRepositoryControllerService>();
        IsEditDisabled = true;
        IsEditEnabled = false;
    }

    public void StartEdit_Click(object sender, RoutedEventArgs e)
    {
        IsEditDisabled = false;
        IsEditEnabled = true;
    }

    private Medicine? _currentMedicine;

    public Medicine? CurrentMedicine
    {
        get => _currentMedicine;
        set=> _currentMedicine = value;
    }


    /// <summary>
    /// Saves customer data that has been edited.
    /// </summary>
    public void SaveAsync(object sender, RoutedEventArgs e)
    {

        Random rnd = new();

        List<string> names = new()
        {
            "Philipp",
            "Eric",
            "Steven",
            "Peter",
            "Alex",
            "Meg"
        };

        List<string> types = new()
        {
            "Box", 
            "Spray",
            "Shit",
            "Funny Stuff",
            "Idiotizm"
        };

        string name = names.ElementAt(rnd.Next(names.Count));

        Medicine test = new()
        {
            Name = name,
            Type = types.ElementAt(rnd.Next(types.Count))
        };

        _repositoryControllerService.Medicines.upsertAsync(test);
        var builder = new AppNotificationBuilder()
            .SetAppLogoOverride(new Uri("ms-appx:///images/reminder.png"))
            .AddArgument("conversationId", "9813")
            .AddText("Medicine under name '${name}' was created")

            .SetAudioUri(new Uri("ms-appx:///Sound.mp3"));

        AppNotificationManager.Default.Show(builder.BuildNotification());
    }

    protected void OnNavigatedTo(object parameter)
    {
        CurrentMedicine = parameter.Parameter as Medicine;
        IsEditDisabled = false;
        IsEditEnabled = true;
    }

    public void OnNavigatedFrom()
    {
        throw new NotImplementedException();
    }
}

