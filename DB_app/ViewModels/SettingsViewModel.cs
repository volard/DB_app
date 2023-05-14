using System.Reflection;
using System.Windows.Input;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DB_app.Contracts.Services;
using DB_app.Helpers;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Windows.ApplicationModel;

namespace DB_app.ViewModels;


// Reduces warning noise on parameters that are needed for signature requirements
#pragma warning disable IDE0060

public partial class SettingsViewModel : ObservableRecipient
{
    private ElementTheme _appElementTheme;


    public ElementTheme AppElementTheme
    {
        get => _appElementTheme;
        set => SetProperty(ref _appElementTheme, value);
    }


    private string _version = GetVersion();
    public string Version
    {
        get => _versionDescription;
        set => SetProperty(ref _version, value);
    }


    private string _versionDescription = GetVersionDescription();
    public string VersionDescription
    {
        get => _versionDescription;
        set => SetProperty(ref _versionDescription, value);
    }


    private readonly IThemeSelectorService _themeSelectorService = App.GetService<IThemeSelectorService>();


    

    public SettingsViewModel(IThemeSelectorService themeSelectorService)
    {
        AppElementTheme = _themeSelectorService.Theme;
    }


    public async void theme_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var comboBox = sender as ComboBox;
        if (comboBox == null || comboBox.SelectedIndex == -1) { return; }
        var item = comboBox.SelectedItem as ComboBoxItem;
        if (item == null) { return; }
        var tag = item.Tag.ToString();
        if (string.IsNullOrEmpty(tag)) {  return; }
        ElementTheme param = (ElementTheme)Enum.Parse(typeof(ElementTheme), tag);

        if (AppElementTheme != param)
        {
            AppElementTheme = param;
            await _themeSelectorService.SetThemeAsync(param);
        }
    }



    /// <summary>
    /// Occurs when <c><see cref="CommunityToolkit.WinUI.UI.Controls.InAppNotification"/></c> is displaying
    /// </summary>
    public event EventHandler<NotificationConfigurationEventArgs>? DisplayInAppNotification;


    private static string GetVersion()
    {
        Version version;

        if (RuntimeHelper.IsMSIX)
        {
            var packageVersion = Package.Current.Id.Version;

            version = new(packageVersion.Major, packageVersion.Minor, packageVersion.Build, packageVersion.Revision);
        }
        else
        {
            version = Assembly.GetExecutingAssembly().GetName().Version!;
        }

        return $"{version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
    }

    private static string GetVersionDescription()
    {
        return $"{"AppDisplayName".GetLocalizedValue()} - {GetVersion()}";
    }
}

#pragma warning restore IDE0060