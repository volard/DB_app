using DB_app.Behaviors;
using DB_app.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;

namespace DB_app.Views;


public sealed partial class PharmaciesGridPage : Page
{
    public PharmaciesGridViewModel ViewModel { get; }

    public PharmaciesGridPage()
    {
        ViewModel = App.GetService<PharmaciesGridViewModel>();
        InitializeComponent();
        SetBinding(NavigationViewHeaderBehavior.HeaderContextProperty, new Binding
        {
            Source = ViewModel,
            Mode = BindingMode.OneWay
        });
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        var givenPharmacy = (PharmacyWrapper)e.Parameter;
        if (givenPharmacy != null)
        {
            if (givenPharmacy.isNew)
            {
                ViewModel.InsertToGridNewWrapper(givenPharmacy);
            }
            else if (givenPharmacy.IsModified)
            {
                ViewModel.UpdateGridWithEditedWrapper(givenPharmacy);
            }
        }
        base.OnNavigatedTo(e);

    }

    private void AddNewPharmacy_Click(object sender, RoutedEventArgs e) =>
        Frame.Navigate(typeof(PharmacyDetailsPage), null, new DrillInNavigationTransitionInfo());

    private void EditExistingPharmacy_Click(object sender, RoutedEventArgs e)
    {
        Frame.Navigate(typeof(PharmacyDetailsPage), null, new DrillInNavigationTransitionInfo());
        ViewModel.SendPrikol();
    }
}
