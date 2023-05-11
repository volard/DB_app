using CommunityToolkit.Mvvm.Messaging;
using DB_app.Behaviors;
using DB_app.Core.Contracts.Services;
using DB_app.Services.Messages;
using DB_app.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Navigation;

namespace DB_app.Views;

// Reduces warning noise on parameters that are needed for signature requirements
#pragma warning disable IDE0060

public sealed partial class AddressDetailsPage : Page
{
    public AddressDetailsViewModel ViewModel { get; }= App.GetService<AddressDetailsViewModel>();

    public AddressDetailsPage()
    {
        InitializeComponent();
        SetBinding(NavigationViewHeaderBehavior.HeaderContextProperty, new Binding
        {
            Source = ViewModel,
            Mode = BindingMode.OneWay
        });

        City.CustomTextChanged += new TextChangedEventHandler(Text_TextChanged);
        Street.CustomTextChanged += new TextChangedEventHandler(Text_TextChanged);
        Building.CustomTextChanged += new TextChangedEventHandler(Text_TextChanged);
    }

    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        _ = ViewModel.CurrentAddress.SaveAsync();
    }

    /// <summary>
    /// Navigate to the previous page when the user cancels the creation of a new record.
    /// </summary>
    private void CancelButton_Click(object? sender, RoutedEventArgs e)
    {
        ViewModel.CurrentAddress.CancelEdit();
    }


    private async void DeleteButton_Click(object? sender, RoutedEventArgs e)
    {
        try
        {
            await App.GetService<IRepositoryControllerService>().Addresses.DeleteAsync(ViewModel.CurrentAddress.Id);

            // Sync with grid
            Frame.GoBack();
            WeakReferenceMessenger.Default.Send(new DeleteRecordMessage<AddressWrapper>(ViewModel.CurrentAddress));
        } 
        catch (Exception)
        {
            var message = "жоская ошебка";
            Notification.Content = message;
            Notification.Show(2000);
        }
    }

    private void AddButton_Click(object? sender, RoutedEventArgs e)
    {
        if (ViewModel.CurrentAddress.IsInEdit)
        {
            ViewModel.CurrentAddress.IsInEdit = false;
        }
        Frame.Navigate(typeof(AddressDetailsPage));
        Frame.BackStack.Remove(Frame.BackStack.Last());
    }


    
    /// <summary>
    /// Check whether there are unsaved changes and warn the user.
    /// </summary>
    protected override async void OnNavigatingFrom(NavigatingCancelEventArgs e)
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

    private void Text_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (ViewModel.CurrentAddress.IsInEdit) 
        {
            ViewModel.CurrentAddress.IsModified = true;
        }
    }


}

#pragma warning restore IDE0060