using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Reflection.Metadata;
using DB_app.Activation;
using DB_app.Contracts.Services;
using DB_app.Views;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Documents;
using static ABI.System.Windows.Input.ICommand_Delegates;

namespace DB_app.Services;

public class ActivationService : IActivationService
{
    private readonly ActivationHandler<LaunchActivatedEventArgs> _defaultHandler;
    private readonly IEnumerable<IActivationHandler> _activationHandlers; // collection of ActivationHandlers
    private readonly IThemeSelectorService _themeSelectorService;
    private readonly ILocalizationService _localizationService;
    private UIElement? _shell = null;

    public ActivationService(
        ActivationHandler<LaunchActivatedEventArgs> defaultHandler, 
        IEnumerable<IActivationHandler> activationHandlers, 
        IThemeSelectorService themeSelectorService,
        ILocalizationService localizationService)
    {
        _defaultHandler = defaultHandler;
        _activationHandlers = activationHandlers;
        _themeSelectorService = themeSelectorService;
        _localizationService = localizationService;
    }

    /**
     * The entry point for the application lifecycle event `OnLaunched`
     */
    public async Task ActivateAsync(object activationArgs)
    {
        // Execute tasks before activation.
        await InitializeAsync();

        // Set the MainWindow Content.
        if (App.MainWindow.Content == null)
        {
            _shell = App.GetService<ShellPage>();
            App.MainWindow.Content = _shell ?? new Frame();
        }

        // Handle activation via ActivationHandlers.
        await HandleActivationAsync(activationArgs);

        // Activate the MainWindow.
        App.MainWindow.Activate();

        // Execute tasks after activation.
        await StartupAsync();
    }

    /**
     * Gets the first ActivationHandler that can handle the arguments of the current activation 
     * otherwise it returns default one
     */
    private async Task HandleActivationAsync(object activationArgs)
    {
        var activationHandler = _activationHandlers.FirstOrDefault(h => h.CanHandle(activationArgs));

        if (activationHandler != null)
        {
            await activationHandler.HandleAsync(activationArgs);
        }

        if (_defaultHandler.CanHandle(activationArgs))
        {
            await _defaultHandler.HandleAsync(activationArgs);
        }
    }



    /// <summary>
    /// Contains services initialization for services that are going to be used as ActivationHandler.
    /// This method is called before the window is activated.Only code that needs to be executed before app
    /// activation should be placed here, as the splash screen is shown while this code is executed.
    /// </summary>
    /// <returns></returns>
    private async Task InitializeAsync()
    {
        await _localizationService.InitializeAsync().ConfigureAwait(false);
        await _themeSelectorService.InitializeAsync().ConfigureAwait(false);
        await Task.CompletedTask;
    }



    /// <summary>
    /// Contains initializations of other classes that do not need to happen before app activation 
    /// and starts processes that will be run after the Window is activated.
    /// </summary>
    /// <returns></returns>
    private async Task StartupAsync()
    {
        await _themeSelectorService.SetRequestedThemeAsync();
        await Task.CompletedTask;
    }
}
