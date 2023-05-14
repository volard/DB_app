using Microsoft.UI.Xaml;

namespace DB_app;

public sealed partial class OrderDetailsWindow : WindowEx
{
    public OrderDetailsWindow(UIElement myContent)
    {
        InitializeComponent();
        AppWindow.SetIcon(Path.Combine(AppContext.BaseDirectory, "Assets/WindowIcon.ico"));
        Title = "Order details";
        ExtendsContentIntoTitleBar = true;
        SetTitleBar(AppTitleBar);
        NavigationFrame.Content = myContent;
    }
}
