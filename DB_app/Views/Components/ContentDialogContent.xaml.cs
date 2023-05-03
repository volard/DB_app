using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using CommunityToolkit.Mvvm.ComponentModel;

namespace DB_app.Views.Components;

public sealed partial class ContentDialogContent : Page
{
    public int Min { get; set; } = 1;
    public int Max { get; set; }

    public ContentDialogContentViewModel ViewModel {get; set; } = new();

    public ContentDialogContent(int max)
    {
        this.InitializeComponent();
        Max = max;

    }

    public ContentDialogContent(int max, int current)
    {
        this.InitializeComponent();
        Max = max;
        ViewModel.Current = current;

    }

    private void Slider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
    {
    }
}

public partial class ContentDialogContentViewModel : ObservableObject
{
    [ObservableProperty]
    public int current = 1;
}
