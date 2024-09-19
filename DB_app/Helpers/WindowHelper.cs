using Microsoft.UI.Xaml;

namespace AppUIBasics.Helper;

// Helper class to allow the app to find the Window that contains an
// arbitrary UIElement (GetWindowForElement).  To do this, we keep track
// of all active Windows.  The app code must call WindowHelper.CreateWindow
// rather than "new Window" so we can keep track of all the relevant
// windows.  In the future, we would like to support this in platform APIs.
public class WindowHelper
{
    public static Window CreateWindow()
    {
        Window newWindow = new Window();
        TrackWindow(newWindow);
        return newWindow;
    }

    public static void TrackWindow(Window window)
    {
        window.Closed += (sender, args) => {
            ActiveWindows.Remove(window);
        };
        ActiveWindows.Add(window);
    }

    public static Window? GetWindowForElement(UIElement element)
    {
        return element.XamlRoot == null ? null : ActiveWindows.FirstOrDefault(window => element.XamlRoot == window.Content.XamlRoot);
    }

    public static UIElement? FindElementByName(UIElement element, string name)
    {
        if (element.XamlRoot == null || element.XamlRoot.Content == null) return null;
        
        object? ele = (element.XamlRoot.Content as FrameworkElement)?.FindName(name);
        return ele as UIElement;
    }
    

    private static List<Window> ActiveWindows { get; } = new List<Window>();

}