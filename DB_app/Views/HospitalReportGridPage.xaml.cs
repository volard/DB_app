﻿using DB_app.ViewModels;

using Microsoft.UI.Xaml.Controls;

namespace DB_app.Views;

public sealed partial class HospitalReportGridPage : Page
{
    public HospitalReportGridViewModel ViewModel
    {
        get;
    }

    public HospitalReportGridPage()
    {
        ViewModel = App.GetService<HospitalReportGridViewModel>();
        InitializeComponent();
    }
}
