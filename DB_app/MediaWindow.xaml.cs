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


namespace DB_app;

public sealed partial class MediaWindow : WindowEx
{
    public MediaWindow()
    {
        InitializeComponent();
        this.Activated += MediaWindow_Activated;
        this.Closed += MediaWindow_Closed;
    }

    private void MediaWindow_Closed(object sender, WindowEventArgs args)
    {
        MeaningOfMyLife.MediaPlayer.Pause();
    }

    private void MediaWindow_Activated(object sender, WindowActivatedEventArgs args)
    {
        MeaningOfMyLife.MediaPlayer.IsMuted = false;
        MeaningOfMyLife.MediaPlayer.Volume = 100;
        MeaningOfMyLife.MediaPlayer.Play(); 
    }



    private void MeaningOfMyLife_Loaded(object sender, RoutedEventArgs e)
    {
        MeaningOfMyLife.MediaPlayer.IsLoopingEnabled = true;
    }
}
