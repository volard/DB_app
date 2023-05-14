using Microsoft.UI.Xaml;


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
