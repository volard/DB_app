using DB_app.Behaviors;
using DB_app.Core.Contracts.Services;
using DB_app.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Navigation;

namespace DB_app.Views;

// TODO change "New Address" page header to dynamic one
public sealed partial class AddressDetailsPage : Page
{
    public AddressDetailsViewModel ViewModel { get; }

    public AddressDetailsPage()
    {
        ViewModel = App.GetService<AddressDetailsViewModel>();
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

    private async void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        bool result = await ViewModel.CurrentAddress.SaveAsync();

    }

    /// <summary>
    /// Navigate to the previous page when the user cancels the creation of a new record.
    /// </summary>
    private void CancelButton_Click(object? sender, RoutedEventArgs e)
    {
        ViewModel.CurrentAddress.CancelEdit();
        ViewModel.CurrentAddress.IsInEdit = false;
    }


    private async void DeleteButton_Click(object? sender, RoutedEventArgs e)
    {
        Frame.GoBack();
        await App.GetService<IRepositoryControllerService>().Addresses.DeleteAsync(ViewModel.CurrentAddress.Id);
    }

    private void AddButton_Click(object? sender, RoutedEventArgs e)
    {
        if (ViewModel.CurrentAddress.IsInEdit)
        {
            // TODO Display notification
            ViewModel.CurrentAddress.IsInEdit = false;
        }
    }


    /// <summary>
    /// Check whether there are unsaved changes and warn the user.
    /// </summary>
    protected override async void OnNavigatingFrom(NavigatingCancelEventArgs e)
    {

        if (ViewModel.CurrentAddress.IsModified)
        {
            var saveDialog = new SaveChangesDialog
            {
                Title = $"Save changes?",
                Content = $"This address " +
                    "has unsaved changes that will be lost. Do you want to save your changes?",
                XamlRoot = this.Content.XamlRoot
            };
            await saveDialog.ShowAsync();
            SaveChangesDialogResult result = saveDialog.Result;

            switch (result)
            {
                case SaveChangesDialogResult.Save:
                    await ViewModel.CurrentAddress.SaveAsync();
                    break;
                case SaveChangesDialogResult.DontSave:
                    break;
                case SaveChangesDialogResult.Cancel:
                    if (e.NavigationMode == NavigationMode.Back)
                    {
                        Frame.GoForward();
                    }
                    else
                    {
                        Frame.GoBack();
                    }
                    e.Cancel = true;

                    // This flag gets cleared on navigation, so restore it. 
                    ViewModel.CurrentAddress.IsModified = true;
                    break;
            }
        }

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
