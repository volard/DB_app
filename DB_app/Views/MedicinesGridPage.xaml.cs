using DB_app.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using System.Diagnostics;

namespace DB_app.Views;


public sealed partial class MedicinesGridPage : Page
{
    public MedicinesGridViewModel ViewModel{ get; }

    public MedicinesGridPage()
    {
        ViewModel = App.GetService<MedicinesGridViewModel>();
        InitializeComponent();
    }

    private void AddNewMedicine_Click(object sender, RoutedEventArgs e) =>
            Frame.Navigate(typeof(MedicineDetailsPage), null, new DrillInNavigationTransitionInfo());

}
