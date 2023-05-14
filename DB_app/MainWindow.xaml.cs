using Microsoft.Windows.ApplicationModel.Resources;

namespace DB_app;

public sealed partial class MainWindow : WindowEx
{
    public MainWindow()
    {
        InitializeComponent();

        //AppWindow.SetIcon(Path.Combine(AppContext.BaseDirectory, "Assets/WindowIcon.ico"));
        AppWindow.SetIcon(Path.Combine(AppContext.BaseDirectory, "Assets/Square44x44Logo.png"));
        
        Content = null;

        ResourceLoader _resourceLoader = new();

        Title = _resourceLoader.GetString("AppDisplayName");
    }
}
