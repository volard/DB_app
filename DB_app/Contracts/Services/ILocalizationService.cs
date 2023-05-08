using DB_app.Models;

namespace DB_app.Contracts.Services;

public interface ILocalizationService
{
    List<LanguageItem> Languages { get; }

    LanguageItem GetCurrentLanguageItem();
    Task InitializeAsync();
    Task SetLanguageAsync(LanguageItem languageItem);
}