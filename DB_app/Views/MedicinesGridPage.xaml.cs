using DB_app.ViewModels;

using Microsoft.UI.Xaml.Controls;

namespace DB_app.Views;


public sealed partial class MedicinesGridPage : Page
{
    public MedicinesGridViewModel ViewModel
    {
        get;
    }

    public MedicinesGridPage()
    {
        ViewModel = App.GetService<MedicinesGridViewModel>();
        InitializeComponent();
    }
}
