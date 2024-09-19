using Microsoft.Windows.ApplicationModel.Resources;

namespace DB_app.Helpers;

public static class ResourceExtensions
{
    private static readonly ResourceLoader _resourceLoader = new();

    public static string GetLocalizedValue(this string resourceKey) => _resourceLoader.GetString(resourceKey);
}
