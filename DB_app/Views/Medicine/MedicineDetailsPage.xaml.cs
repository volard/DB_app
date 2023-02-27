using DB_app.Behaviors;
using DB_app.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Navigation;
using System.Diagnostics;
using WinUIEx.Messaging;

namespace DB_app.Views;

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

    private async void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        await ViewModel.SaveAsync();
        ViewModel.NotifyGridAboutChange();
        
        Frame.Navigate(typeof(MedicinesGridPage), null);
    }

    /// <summary>
    /// Navigate to the previous page when the user cancels the creation of a new record.
    /// </summary>
    private void CancelEdit_Click(object sender, RoutedEventArgs e) => Frame.GoBack();

    /// <summary>
    /// Check whether there are unsaved changes and warn the user.
    /// </summary>
    protected async override void OnNavigatingFrom(NavigatingCancelEventArgs e)
    {
        if (ViewModel.CurrentMedicine.IsModified)
        {
            // Cancel the navigation immediately, otherwise it will continue at the await call. 
            e.Cancel = true;

            void resumeNavigation()
            {
                if (e.NavigationMode == NavigationMode.Back)
                {
                    Frame.GoBack();
                }
                else
                {
                    Frame.Navigate(e.SourcePageType, e.Parameter, e.NavigationTransitionInfo);
                }
            }

            var saveDialog = new SaveChangesDialog() { Title = $"Save changes?" };
            saveDialog.XamlRoot = this.Content.XamlRoot;
            await saveDialog.ShowAsync();
            SaveChangesDialogResult result = saveDialog.Result;

            switch (result)
            {
                case SaveChangesDialogResult.Save:
                    await ViewModel.SaveAsync();
                    resumeNavigation();
                    break;
                //case SaveChangesDialogResult.DontSave:
                //    await ViewModel.RevertChangesAsync();
                //    resumeNavigation();
                //    break;
                case SaveChangesDialogResult.Cancel:
                    break;
            }
        }

        base.OnNavigatingFrom(e);
    }


    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        ViewModel.CurrentMedicine.Backup();
        base.OnNavigatedTo(e);
    }

    private void NameText_TextChanged(object sender, TextChangedEventArgs e)
    {
         ViewModel.CurrentMedicine.Name = Name.Text;
    }

    private void TypeText_TextChanged(object sender, TextChangedEventArgs e)
    {
          ViewModel.CurrentMedicine.Type = Type.Text;
    }
}
