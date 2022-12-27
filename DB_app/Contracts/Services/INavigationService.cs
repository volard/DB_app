using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

namespace DB_app.Contracts.Services;

/// <summary>
/// The NavigationService is in charge of handling the navigation between app pages.
/// </summary>
public interface INavigationService
{
    event NavigatedEventHandler Navigated;

    bool CanGoBack
    {
        get;
    }

    Frame? Frame
    {
        get; set;
    }

    /// <summary>
    /// Method to navigate between pages. For more information, please visit <see href="https://github.com/microsoft/TemplateStudio/blob/main/docs/WinUI/navigation.md">GitHub page</see>
    /// </summary>
    /// <param name="pageKey"></param>
    /// <param name="parameter"> It's a page key. These page keys are registered in the <see cref="DB_app.Contracts.Services.IPageService"/> constructor and correspond to the Page's ViewModel FullName.</param>
    /// <param name="clearNavigation"></param>
    /// <returns></returns>
    bool NavigateTo(string pageKey, object? parameter = null, bool clearNavigation = false);

    bool GoBack();
}
