using DB_app.Contracts.Services;
using DB_app.Models;
using Microsoft.Windows.ApplicationModel.Resources;

namespace DB_app.Services;

public class LocalizationService : ILocalizationService
{
    private const string LocalizationTagSettingsKey = "LocalizationTag";

    private readonly ILocalSettingsService _localSettingsService;

    private readonly ResourceManager _resourceManager;

    private readonly ResourceContext _resourceContext;

    private LanguageItem _currentLanguageItem = new("en-US", "English"); // TODO fix hardcoded value

    public List<LanguageItem> Languages { get; } = new();


    public LocalizationService(ILocalSettingsService localSettingsService)
    {
        _localSettingsService = localSettingsService;
        _resourceManager = new();
        _resourceContext = _resourceManager.CreateResourceContext();
    }


    public LanguageItem GetCurrentLanguageItem() => _currentLanguageItem;

    /// <summary>
    /// Initialized service and its properties
    /// </summary>
    public async Task InitializeAsync()
    {
        RegisterLanguagesFromResource();

        string? languageTag = await GetLanguageTagFromSettingsAsync();

        if (languageTag is not null && GetLanguageItem(languageTag) is LanguageItem languageItem)
        {
            await SetLanguageAsync(languageItem);
        }
        else
        {
            await SetLanguageAsync(_currentLanguageItem);
        }
    }



    /// <summary>
    /// Apply provided language
    /// </summary>
    /// <param name="languageItem">Provided <c><see cref="LanguageItem"/></c> instance</param>
    public async Task SetLanguageAsync(LanguageItem languageItem)
    {
        if (Languages.Contains(languageItem) is not true) return;
        
        _currentLanguageItem = languageItem;

        Windows.Globalization.ApplicationLanguages.PrimaryLanguageOverride = languageItem.Tag;
        _resourceContext.QualifierValues["Language"] = languageItem.Tag;

        await _localSettingsService.SaveSettingAsync(LocalizationTagSettingsKey, languageItem.Tag);
        
    }


    /// <summary>
    /// Gets <c><see cref="LanguageItem"/></c> instance from <c><see cref="Languages"/></c> list
    /// or null if no Registered <c><see cref="LanguageItem"/></c> exist with <paramref name="languageTag"/>
    /// </summary>
    private LanguageItem? GetLanguageItem(string languageTag)
    {
        return Languages.FirstOrDefault(item => item.Tag == languageTag);
    }


    /// <summary>
    /// Gets applyed and saved in settings language <c><see cref="LanguageItem.Tag"/></c>
    /// </summary>
    /// <returns></returns>
    private async Task<string?> GetLanguageTagFromSettingsAsync()
    {
        return await _localSettingsService.ReadSettingAsync<string>(LocalizationTagSettingsKey);
    }


    /// <summary>
    /// Initializes <c><see cref="Languages"/></c> list with <c><see cref="LanguageItem"/></c> instances created
    /// with retrieved from <c>LanguagesList.resw</c> language tags and display names
    /// </summary>
    private void RegisterLanguagesFromResource()
    {
        ResourceMap resourceMap = _resourceManager.MainResourceMap.GetSubtree("LanguageList");

        for (uint i = 0; i < resourceMap.ResourceCount; i++)
        {
            var resource = resourceMap.GetValueByIndex(i);
            Languages.Add(new LanguageItem(resource.Key, resource.Value.ValueAsString));
        }
    }
}
