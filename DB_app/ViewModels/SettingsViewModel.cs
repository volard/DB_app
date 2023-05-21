using System.Reflection;
using CommunityToolkit.Mvvm.ComponentModel;
using DB_app.Contracts.Services;
using DB_app.Helpers;
using DB_app.Models;
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
    private readonly ILocalizationService _localizationService;
    
    /// <summary>
    /// Occurs when <c><see cref="CommunityToolkit.WinUI.UI.Controls.InAppNotification"/></c> is displaying
    /// </summary>
    public event EventHandler<NotificationConfigurationEventArgs>? DisplayInAppNotification;



    public SettingsViewModel()
    {
        AppElementTheme = _themeSelectorService.Theme;
        _localizationService = App.GetService<ILocalizationService>();
        AvailableLanguages = _localizationService.Languages;
        SelectedLanguage = _localizationService.GetCurrentLanguageItem();
    }


    public async void theme_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        ComboBox? comboBox = sender as ComboBox;
        if (comboBox == null || comboBox.SelectedIndex == -1) { return; }
        ComboBoxItem? item = comboBox.SelectedItem as ComboBoxItem;
        if (item == null) { return; }
        string? tag = item.Tag.ToString();
        if (string.IsNullOrEmpty(tag)) {  return; }
        ElementTheme param = (ElementTheme)Enum.Parse(typeof(ElementTheme), tag);

        if (AppElementTheme != param)
        {
            AppElementTheme = param;
            await _themeSelectorService.SetThemeAsync(param);
        }
    }


    private LanguageItem _selectedLanguage;
    public LanguageItem SelectedLanguage
    {
        get { return _selectedLanguage; }
        set { SetProperty(ref _selectedLanguage, value); }
    }


    private bool _isLocalizationChanged;
    public bool IsLocalizationChanged
    {
        get { return _isLocalizationChanged; }
        set { SetProperty(ref _isLocalizationChanged, value); }
    }


    private List<LanguageItem> _availableLanguages;
    public List<LanguageItem> AvailableLanguages
    {
        get { return _availableLanguages; }
        set { SetProperty(ref _availableLanguages, value); }
    }


    public void language_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        //DisplayInAppNotification?.Invoke(this, new("Not implemented yet", NotificationType.Info));
        if (sender is not ComboBox comboBox)                          { return; }
        if(comboBox.SelectedItem is not LanguageItem language)        { return; }
        if(language == _localizationService.GetCurrentLanguageItem()) { return; }

        IsLocalizationChanged = true;
        _localizationService.SetLanguageAsync(language);
    }
    

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