
using DB_app.Behaviors;
using DB_app.Models;
using DB_app.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
namespace DB_app.Views
{

    public sealed partial class MedicineDetailsPage : Page
    {
        public MedicineDetailsPageViewModel ViewModel { get; }

        public MedicineDetailsPage()
        {
            ViewModel = App.GetService<MedicineDetailsPageViewModel>();
            InitializeComponent();
            SetBinding(NavigationViewHeaderBehavior.HeaderContextProperty, new Binding
            {
                Source = ViewModel,
                Mode = BindingMode.OneWay
            });
        }

        /// <summary>
        /// Navigate to the previous page when the user cancels the creation of a new record.
        /// </summary>
        private void AddNewMedicineCanceled(object sender, RoutedEventArgs e) => Frame.GoBack();


    }
}
