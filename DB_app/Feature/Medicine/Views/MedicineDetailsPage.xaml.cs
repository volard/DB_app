using CommunityToolkit.Mvvm.Messaging;
using DB_app.Behaviors;
using DB_app.Core.Contracts.Services;
using DB_app.Helpers;
using DB_app.Services.Messages;
using DB_app.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Navigation;
using System.Diagnostics;
using WinUIEx.Messaging;

namespace DB_app.Views;

// Reduces warning noise on parameters that are needed for signature requirements
#pragma warning disable IDE0060

public sealed partial class MedicineDetailsPage : Page
{
    public MedicineDetailsViewModel ViewModel { get; } = App.GetService<MedicineDetailsViewModel>();

    public MedicineDetailsPage()
    {
        InitializeComponent();
        SetBinding(NavigationViewHeaderBehavior.HeaderContextProperty, new Binding
        {
            Source = ViewModel,
            Mode = BindingMode.OneWay
        });
    }

    private async void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        await ViewModel.CurrentMedicine.SaveAsync();
    }


    private async void DeleteButton_Click(object? sender, RoutedEventArgs e)
    {
        try
        {
            await App.GetService<IRepositoryControllerService>().Addresses.DeleteAsync(ViewModel.CurrentMedicine.Id);
            Frame.GoBack();
            WeakReferenceMessenger.Default.Send(new DeleteRecordMessage<MedicineWrapper>(ViewModel.CurrentMedicine));

            Notification.Content = "Success";
            Notification.Style = NotificationHelper.SuccessStyle;
            Notification.Show(1500);
        } 
        catch (Exception)
        {
            Notification.Content = "Error occured";
            Notification.Style = NotificationHelper.ErrorStyle;
            Notification.Show(1500);
        }
    }

    private void AddButton_Click(object? sender, RoutedEventArgs e)
    {
        if (ViewModel.CurrentMedicine.IsInEdit)
        {
            ViewModel.CurrentMedicine.IsInEdit = false;
        }
        Frame.Navigate(typeof(AddressDetailsPage));
        Frame.BackStack.Remove(Frame.BackStack.Last());
    }



    /// <summary>
    /// Check whether there are unsaved changes and warn the user.
    /// </summary>
    protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
    {

        // TODO add confirmation feature etc
        //if (ViewModel.CurrentAddress.IsModified)
        //{
        //    var saveDialog = new SaveChangesDialog
        //    {
        //        Title = $"Save changes?",
        //        Content = $"This address " +
        //            "has unsaved changes that will be lost. Do you want to save your changes?",
        //        XamlRoot = this.Content.XamlRoot
        //    };
        //    await saveDialog.ShowAsync();
        //    SaveChangesDialogResult result = saveDialog.Result;

        //    switch (result)
        //    {
        //        case SaveChangesDialogResult.Save:
        //            await ViewModel.CurrentAddress.SaveAsync();
        //            break;
        //        case SaveChangesDialogResult.DontSave:
        //            break;
        //        case SaveChangesDialogResult.Cancel:
        //            if (e.NavigationMode == NavigationMode.Back)
        //            {
        //                Frame.GoForward();
        //            }
        //            else
        //            {
        //                Frame.GoBack();
        //            }
        //            e.Cancel = true;

        //            // This flag gets cleared on navigation, so restore it. 
        //            ViewModel.CurrentAddress.IsModified = true;
        //            break;
        //    }
        //}

        base.OnNavigatingFrom(e);
    }

    private void BeginEdit_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.CurrentMedicine.BeginEdit();
    }
}

#pragma warning restore IDE0060