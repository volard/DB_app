using CommunityToolkit.WinUI;
using Microsoft.Windows.ApplicationModel.Resources;

namespace DB_app;

public sealed partial class MainWindow : WindowEx
{
    public MainWindow()
    {
        InitializeComponent();

        AppWindow.SetIcon(Path.Combine(AppContext.BaseDirectory, "Assets/WindowIcon.ico"));
        Content = null;

        ResourceLoader _resourceLoader = new();

        Title = _resourceLoader.GetString("AppDisplayName");
    }
}
