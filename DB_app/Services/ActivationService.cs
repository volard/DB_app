﻿using System.ComponentModel;
using System.Diagnostics.Metrics;
using System.Reflection.Metadata;
using DB_app.Activation;
using DB_app.Contracts.Services;
using DB_app.Views;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace DB_app.Services;

public class ActivationService : IActivationService
{
    private readonly ActivationHandler<LaunchActivatedEventArgs> _defaultHandler;
    private readonly IEnumerable<IActivationHandler> _activationHandlers; // collection of ActivationHandlers
    private readonly IThemeSelectorService _themeSelectorService;
    private UIElement? _shell = null;

    public ActivationService(ActivationHandler<LaunchActivatedEventArgs> defaultHandler, 
        IEnumerable<IActivationHandler> activationHandlers, IThemeSelectorService themeSelectorService)
    {
        _defaultHandler = defaultHandler;
        _activationHandlers = activationHandlers;
        _themeSelectorService = themeSelectorService;
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
    
    /**
     * Contains services initialization for services that are going to be used as ActivationHandler.
     * This method is called before the window is activated. Only code that needs to be executed before app 
     * activation should be placed here, as the splash screen is shown while this code is executed.
     */
    private async Task InitializeAsync()
    {
        await _themeSelectorService.InitializeAsync().ConfigureAwait(false);
        await Task.CompletedTask;
    }

    /**
     * Contains initializations of other classes that do not need to happen before app activation 
     * and starts processes that will be run after the Window is activated.
     */
    private async Task StartupAsync()
    {
        await _themeSelectorService.SetRequestedThemeAsync();
        await Task.CompletedTask;
    }
}
