
using DB_app.Behaviors;
using DB_app.Models;
using DB_app.ViewModels;
using DB_app.ViewModels.ObjectWrappers;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Navigation;
namespace DB_app.Views
{

    public sealed partial class MedicineDetailsPage : Page
    {
        public MedicineDetailsViewModel ViewModel { get; }

        public MedicineDetailsPage()
        {
            ViewModel = App.GetService<MedicineDetailsViewModel>();
            InitializeComponent();
            SetBinding(NavigationViewHeaderBehavior.HeaderContextProperty, new Binding
            {
                Source = ViewModel,
                Mode = BindingMode.OneWay
            });
        }


        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.CurrentMedicine != null) 
            {
                if (ViewModel.CurrentMedicine.IsModified)
                {
                    Frame.Navigate(typeof(MedicinesGridPage), ViewModel.CurrentMedicine);
                }
                
            }
            
        }


        /// <summary>
        /// Navigate to the previous page when the user cancels the creation of a new record.
        /// </summary>
        private void AddNewMedicineCanceled(object sender, RoutedEventArgs e)
            => Frame.GoBack();

        /// <summary>
        /// Check whether there are unsaved changes and warn the user.
        /// </summary>
        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            // TODO implement this
        }

        /// <summary>
        /// Loads selected MedicineWrapper object or creates a new order.
        /// </summary>
        /// <param name="e">Info about the event.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var selectedMedicine = (MedicineWrapper)e.Parameter;
            if (selectedMedicine != null)
            {
                ViewModel.CurrentMedicine = selectedMedicine;
            }

            base.OnNavigatedTo(e);
        }

        private void NameText_TextChanged(object sender, TextChangedEventArgs e)
        {
            ViewModel.Name = Name.Text;
        }

        private void TypeText_TextChanged(object sender, TextChangedEventArgs e)
        {
            ViewModel.Type = Type.Text;
        }
    }
}
