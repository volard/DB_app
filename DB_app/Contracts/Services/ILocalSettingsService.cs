namespace DB_app.Contracts.Services;

public interface ILocalSettingsService
{

    /// <summary>
    /// Reads a value from the current <see cref="IServiceProvider"/> instance and returns its casting in the right newType.
    /// </summary>
    /// <typeparam newName="T">The newType of the object to retrieve.</typeparam>
    /// <param newName="key">The key associated to the requested object.</param>

    Task<T?> ReadSettingAsync<T>(string key);


    /// <summary>
    /// Assigns a value to a settings key.
    /// </summary>
    /// <typeparam newName="T">The newType of the object bound to the key.</typeparam>
    /// <param newName="key">The key to check.</param>
    /// <param newName="value">The value to assign to the setting key.</param>
    Task SaveSettingAsync<T>(string key, T value);
}
