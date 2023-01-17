using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

namespace DB_app;

public sealed partial class OrderDetailsWindow : WindowEx
{
    public OrderDetailsWindow(UIElement myContent)
    {
        this.InitializeComponent();
        AppWindow.SetIcon(Path.Combine(AppContext.BaseDirectory, "Assets/WindowIcon.ico"));
        Title = "Order details";
        ExtendsContentIntoTitleBar = true;
        SetTitleBar(AppTitleBar);
        NavigationFrame.Content = myContent;
    }
}
