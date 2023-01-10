using DB_app.Behaviors;
using DB_app.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;

namespace DB_app.Views;


public sealed partial class HospitalsGridPage : Page
{
    public HospitalsGridViewModel ViewModel { get; }

    public HospitalsGridPage()
    {
        ViewModel = App.GetService<HospitalsGridViewModel>();
        InitializeComponent();
        SetBinding(NavigationViewHeaderBehavior.HeaderContextProperty, new Binding
        {
            Source = ViewModel,
            Mode = BindingMode.OneWay
        });
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        var givenHospital = (HospitalWrapper)e.Parameter;
        if (givenHospital != null)
        {
            if (givenHospital.isNew)
            {
                ViewModel.InsertToGridNewWrapper(givenHospital);
            }
            else if (givenHospital.IsModified)
            {
                ViewModel.UpdateGridWithEditedWrapper(givenHospital);
            }
        }
        base.OnNavigatedTo(e);

    }

    private void AddNewHospital_Click(object sender, RoutedEventArgs e) =>
        Frame.Navigate(typeof(HospitalDetailsPage), null, new DrillInNavigationTransitionInfo());

    private void EditExistingHospital_Click(object sender, RoutedEventArgs e)
    {
        Frame.Navigate(typeof(HospitalDetailsPage), null, new DrillInNavigationTransitionInfo());
        ViewModel.SendPrikol();
    }
}
