using DB_app.Behaviors;
using DB_app.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.Windows.AppNotifications.Builder;
using Microsoft.Windows.AppNotifications;
using System.Diagnostics;

namespace DB_app.Views;


public sealed partial class MedicinesGridPage : Page
{
    public MedicinesGridViewModel ViewModel{ get; }

    public MedicinesGridPage()
    {
        ViewModel = App.GetService<MedicinesGridViewModel>();
        InitializeComponent();
        SetBinding(NavigationViewHeaderBehavior.HeaderContextProperty, new Binding
        {
            Source = ViewModel,
            Mode = BindingMode.OneWay
        });
    }

    private void AddNewMedicine_Click(object sender, RoutedEventArgs e) =>
            Frame.Navigate(typeof(MedicineDetailsPage), null, new DrillInNavigationTransitionInfo());

    public void testNoutification(object sender, RoutedEventArgs e)
    {
        var builder = new AppNotificationBuilder()
            .SetAppLogoOverride(new Uri("ms-appx:///images/reminder.png"))
            .AddArgument("conversationId", "9813") 
            .AddText("Some text")

            .SetAudioUri(new Uri("ms-appx:///Sound.mp3"));

        AppNotificationManager.Default.Show(builder.BuildNotification());
    }

    private void EditExistingMedicine(object sender, RoutedEventArgs e)
    {
        Frame.Navigate(typeof(MedicineDetailsPage), ViewModel.SelectedMedicine);
        //Frame.Navigate(typeof(MedicineDetailsPage), ViewModel.SelectedMedicine, new DrillInNavigationTransitionInfo());
    }


}
