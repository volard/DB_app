namespace DB_app.Models;


/// <summary>
/// Represents language instance by wrapping up language <see cref="Tag"/> and language <see cref="DisplayName"/>
/// </summary>
public class LanguageItem
{
    public string Tag { get; set; }
    public string DisplayName { get; set; }

    public LanguageItem(string tag, string displayName)
    {
        Tag = tag;
        DisplayName = displayName;
    }

    public override string ToString() => DisplayName;
}