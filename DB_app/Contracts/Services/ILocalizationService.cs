using DB_app.Models;
using Microsoft.Windows.ApplicationModel.Resources;

namespace DB_app.Contracts.Services;

public interface ILocalizationService
{
    List<LanguageItem> Languages { get; }

    public ResourceLoader ResourceLoader { get; }

    LanguageItem GetCurrentLanguageItem();
    Task InitializeAsync();
    Task SetLanguageAsync(LanguageItem languageItem);
}