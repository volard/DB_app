namespace DB_app.Contracts.ViewModels;

/**
 * All ViewModels can implement this interface to execute code on navigation events.
 */
public interface INavigationAware
{
    // Code when the app navigates to this page
    void OnNavigatedTo(object parameter);

    // Code when the app navigates away from this page
    void OnNavigatedFrom();
}
